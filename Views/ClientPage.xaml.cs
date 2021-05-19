using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : UserControl
    {
        public float Remise { get; set; } = 0;

        public ObservableCollection<string> ListeFidelio { get; set; } = new ObservableCollection<string>() { "Pas de fidélio" };

        public string SelectedFidelio { get; set; }

        public ObservableCollection<ClientViewModel> ListeClients = new ObservableCollection<ClientViewModel>();
        public ObservableCollection<FournisseurViewModel> ListeFournisseur = new ObservableCollection<FournisseurViewModel>();


        private List<ClientViewModel> clients = new List<ClientViewModel>();
        private List<FournisseurViewModel> fournisseurs = new List<FournisseurViewModel>();


        public ClientPage()
        {
            InitializeComponent();

            var fidel = BDDReader.Read<Fidelio>();

            fidel.ForEach(x => ListeFidelio.Add(x.ID.ToString()));

            SelectedFidelio = ListeFidelio[0];

            DatagridClients.ItemsSource = ListeClients;
            DatagridFournisseur.ItemsSource = ListeFournisseur;

            this.cb_fidelio.ItemsSource = ListeFidelio;
            this.cb_score.ItemsSource = ((Score[])typeof(Score).GetEnumValues()).Select(x => x.ToString());

            clients = BDDReader.Read<Client>().Select(x => new ClientViewModel(x)).ToList();

            clients.ForEach(x => ListeClients.Add(x));


            fournisseurs = BDDReader.Read<Fournisseurs>().Select(x => new FournisseurViewModel(x)).ToList();

            fournisseurs.ForEach(x => ListeFournisseur.Add(x));
        }



        private void DataGridClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var o = (ClientViewModel)this.TabClient.DataContext;

            var val = this.DatagridClients.SelectedItem as ClientViewModel;

            if (val != null)
            {
                o.ID = val.ID;
                o.Type = val.Type;
                o.Nom = val.Nom;
                o.Adresse = val.Adresse;
                o.Ville = val.Ville;
                o.CodePostal = val.CodePostal;
                o.Mail = val.Mail;
                o.Téléphone = val.Téléphone;
                o.Province = val.Province;
                if (o.Type == "Individuel")
                {
                    o.Prénom = val.Prénom;
                    o.DateAdhésion = val.DateAdhésion;
                    o.DateFin = val.DateFin;
                    o.ProgrammeFidélité = val.ProgrammeFidélité;

                    this.TabControl.SelectedIndex = 0;

                    this.cb_fidelio.SelectedItem = val.ProgrammeFidélité;
                }
                else
                {
                    o.NomContact = val.NomContact;
                    o.Remise = val.Remise;
                    this.TabControl.SelectedIndex = 1;
                }
            }
        }

        private void Butt_Add_Click(object sender, RoutedEventArgs e)
        {
            var o = (ClientViewModel)this.TabClient.DataContext;

            try
            {
                Adresse ad = new Adresse(null, o.Adresse, o.Ville, o.CodePostal.ToString(), o.Province);
                long id = BDDWriter.Insert(ad);

                if (TabControl.SelectedIndex == 0) //Indiv
                {
                    var fidelio = BDDReader.GetObject<Fidelio>(o.ProgrammeFidélité, "nom");

                    ClientIndividuel cli = new ClientIndividuel(null, o.Nom, o.Prénom, (int)id, o.Téléphone, o.Mail, (int)fidelio.ID, o.DateAdhésion);

                    cli.ID = BDDWriter.Insert(cli);

                    this.ListeClients.Add(new ClientViewModel(cli));

                }
                else //boutique
                {
                    ClientBoutique cli = new ClientBoutique(null, o.Nom, (int)id, o.Téléphone, o.Mail, o.NomContact, o.Remise);

                    cli.ID = BDDWriter.Insert(cli);

                    this.ListeClients.Add(new ClientViewModel(cli));

                }
            }
            catch
            {
                MessageBox.Show($"Erreur : Impossible d'ajouter le {(TabControl.SelectedItem as TabItem).Header.ToString().ToLower()} spécifié");
            }
        }

        private void Butt_Update_Click(object sender, RoutedEventArgs e)
        {
            var o = (ClientViewModel)this.TabClient.DataContext;

            try
            {
                var client = BDDReader.GetObject<Client>(o.ID);

                Adresse ad = new Adresse((int)client.Adresse.ID, o.Adresse, o.Ville, o.CodePostal.ToString(), o.Province);
                BDDWriter.Update(ad);

                if (TabControl.SelectedIndex == 0) //Indiv
                {
                    var fidelio = BDDReader.GetObject<Fidelio>(o.ProgrammeFidélité, "nom");

                    if (fidelio == null)
                        fidelio = cb_fidelio.SelectedValue as Fidelio;

                    ClientIndividuel cli = new ClientIndividuel(o.ID, o.Nom, o.Prénom, (int)client.Adresse.ID, o.Téléphone, o.Mail, (int)fidelio.ID, o.DateAdhésion);

                    if (BDDWriter.Update(cli))
                    {
                        this.clients.Remove(this.clients.Where(x => x.ID == o.ID).FirstOrDefault());
                        this.clients.Add(new ClientViewModel(cli));

                        this.DatagridClients.ItemsSource = new ObservableCollection<ClientViewModel>(from item in clients
                                                                                                     orderby item.ID ascending
                                                                                                     select item);
                    }
                }
                else //boutique
                {
                    ClientBoutique cli = new ClientBoutique(o.ID, o.Nom, (int)client.Adresse.ID, o.Téléphone, o.Mail, o.NomContact, o.Remise);

                    var b = BDDWriter.Update(cli);
                    if (b)
                    {
                        this.clients.Remove(this.clients.Where(x => x.ID == o.ID).FirstOrDefault());
                        this.clients.Add(new ClientViewModel(cli));

                        this.DatagridClients.ItemsSource = new ObservableCollection<ClientViewModel>(from item in clients
                                                                                                     orderby item.ID ascending
                                                                                                     select item);
                    }

                }
            }
            catch
            {
                MessageBox.Show($"Erreur : Impossible de modifier le {(TabControl.SelectedItem as TabItem).Header.ToString().ToLower()} spécifié");
            }
        }

        private void Butt_Delete_Click(object sender, RoutedEventArgs e)
        {
            var o = (ClientViewModel)this.TabClient.DataContext;

            try
            {
                if (MessageBox.Show("Etes vous sûr de supprimer ce client ? Sa suppression entrainera celles de toutes ses commandes et autres données à son égard", "Attention", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    if (BDDWriter.Remove<Client>(o.ID) >= 1)
                    {
                        this.clients.Remove(this.clients.Where(x => x.ID == o.ID).FirstOrDefault());

                        this.DatagridClients.ItemsSource = new ObservableCollection<ClientViewModel>(from item in clients
                                                                                                     orderby item.ID ascending
                                                                                                     select item);
                    }

                }
            }
            catch
            {
                MessageBox.Show($"Erreur : Impossible de supprimer le {(TabControl.SelectedItem as TabItem).Header.ToString().ToLower()} spécifié");
            }
        }



        private void DataGridFournisseur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var o = (FournisseurViewModel)this.TabFournisseur.DataContext;

            var val = this.DatagridFournisseur.SelectedItem as FournisseurViewModel;

            if (val != null)
            {
                o.Siret = val.Siret;
                o.Score = val.Score;
                o.Contact = val.Contact;
                o.Nom = val.Nom;
                o.Adresse = val.Adresse;
                o.Ville = val.Ville;
                o.CodePostal = val.CodePostal;
                o.Province = val.Province;

                this.cb_score.SelectedItem = val.Score;
            }
        }

        private void Butt_Add_Fournisseur_Click(object sender, RoutedEventArgs e)
        {
            var o = (FournisseurViewModel)this.TabFournisseur.DataContext;

            try
            {
                Adresse ad = new Adresse(null, o.Adresse, o.Ville, o.CodePostal.ToString(), o.Province);
                var idAd = BDDWriter.Insert(ad);

                Fournisseurs fourn = new Fournisseurs(o.Siret, o.Nom, o.Contact, (int)idAd, (int)MyHelper.StringToEnum<Score>(o.Score), 10);

                if (BDDWriter.Insert(fourn) >= 0)
                {
                    this.ListeFournisseur.Add(new FournisseurViewModel(fourn));
                }
                else
                {
                    BDDWriter.Remove(ad);
                }
            }
            catch
            {
                MessageBox.Show($"Erreur : Impossible d'ajouter le fournisseur spécifié");
            }
        }

        private void Butt_Update_Fournisseur_Click(object sender, RoutedEventArgs e)
        {
            var o = (FournisseurViewModel)this.TabFournisseur.DataContext;
            var oldidfournisseur = this.DatagridFournisseur.SelectedItem as FournisseurViewModel;

            try
            {
                var old = BDDReader.GetObject<Fournisseurs>(oldidfournisseur.Siret);

                Adresse ad = new Adresse((int)old.Adresse.ID, o.Adresse, o.Ville, o.CodePostal.ToString(), o.Province);
                BDDWriter.Update(ad);

                Fournisseurs newF = new Fournisseurs(o.Siret, o.Nom, o.Contact, (int)ad.ID, (int)MyHelper.StringToEnum<Score>(o.Score), 10);

                if (BDDWriter.Remove(old) && BDDWriter.Insert(newF) >= 0)
                {
                    this.fournisseurs.Remove(this.fournisseurs.Where(x => x.Siret.ToString() == o.Siret.ToString()).FirstOrDefault());
                    this.fournisseurs.Add(new FournisseurViewModel(newF));

                    this.DatagridFournisseur.ItemsSource = new ObservableCollection<FournisseurViewModel>(from item in this.fournisseurs
                                                                                                          orderby item.Siret ascending
                                                                                                          select item);
                }

            }
            catch
            {
                MessageBox.Show($"Erreur : Impossible de modifier le fournisseur spécifié");
            }
        }

        private void Butt_Delete_Fournisseur_Click(object sender, RoutedEventArgs e)
        {
            var o = (FournisseurViewModel)this.TabFournisseur.DataContext;

            try
            {
                if (MessageBox.Show("Etes vous sûr de supprimer ce fournisseur ? Sa suppression entrainera celles de toutes ses pièces", "Attention", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    if (BDDWriter.Remove<Fournisseurs>(o.Siret) >= 1)
                    {
                        this.fournisseurs.Remove(this.fournisseurs.Where(x => x.Siret.ToString() == o.Siret.ToString()).FirstOrDefault());

                        this.DatagridFournisseur.ItemsSource = new ObservableCollection<FournisseurViewModel>(from item in fournisseurs
                                                                                                              orderby item.Siret ascending
                                                                                                              select item);
                    }

                }
            }
            catch
            {
                MessageBox.Show($"Erreur : Impossible de supprimer le fournisseur spécifié");
            }
        }
    }
}
