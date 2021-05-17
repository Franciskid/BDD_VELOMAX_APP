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

        public ObservableCollection<Fidelio> ListeFidelio { get; set; } = new ObservableCollection<Fidelio>() { new Fidelio(-1, "Pas de fidélio", 0, 0, 0) };

        public Fidelio SelectedFidelio { get; set; }

        public ObservableCollection<ClientViewModel> ListeClients = new ObservableCollection<ClientViewModel>();

        private List<ClientViewModel> clients = new List<ClientViewModel>();

        public ClientPage()
        {
            InitializeComponent();

            var fidel = BDDReader.Read<Fidelio>();

            fidel.ForEach(x => ListeFidelio.Add(x));

            SelectedFidelio = ListeFidelio[0];

            DatagridClients.ItemsSource = ListeClients;

            this.cb_fidelio.ItemsSource = ListeFidelio;

            clients = BDDReader.Read<Client>().Select(x => new ClientViewModel(x)).ToList();

            clients.ForEach(x => ListeClients.Add(x));
        }




        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Butt_Add_Click(object sender, RoutedEventArgs e)
        {
            var o = (ClientViewModel)this.DataContext;

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
            var o = (ClientViewModel)this.DataContext;

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
            var o = (ClientViewModel)this.DataContext;

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


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var o = (ClientViewModel)this.DataContext;

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
                }
                else
                {
                    o.NomContact = val.NomContact;
                    o.Remise = val.Remise;
                    this.TabControl.SelectedIndex = 1;
                }
            }
        }


    }
}
