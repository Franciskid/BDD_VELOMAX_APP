using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
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

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Logique d'interaction pour CommandePage.xaml
    /// </summary>
    public partial class CommandePage : UserControl
    {
        public ObservableCollection<ModeleCommandeViewModel> modeleCollec = new ObservableCollection<ModeleCommandeViewModel>();

        public ObservableCollection<PieceCommandeViewModel> pieceCollec = new ObservableCollection<PieceCommandeViewModel>();

        public ObservableCollection<PrixCheckoutViewModel> checkout = new ObservableCollection<PrixCheckoutViewModel>();


        private static Random rand = new Random(Guid.NewGuid().GetHashCode());

        public CommandePage()
        {
            InitializeComponent();

            var pieces = BDDReader.Read<Piece>();
            var modeles = BDDReader.Read<Modele>();
            var clients = BDDReader.Read<Client>();

            this.dataGridModele.ItemsSource = modeleCollec;
            this.dataGridPiece.ItemsSource = pieceCollec;

            pieces.ForEach(x => pieceCollec.Add(new PieceCommandeViewModel(x)));
            modeles.ForEach(x => modeleCollec.Add(new ModeleCommandeViewModel(x)));

            this.DataGridCheckout.ItemsSource = checkout;

            cb_Client.ItemsSource = clients.Select(x => {
                if (x is ClientBoutique bout)
                    return $"{x.ID} Boutique : {bout.NomEntreprise}";
                if (x is ClientIndividuel indiv)
                    return $"{x.ID} Individuel : {indiv.Prénom} {indiv.Nom}";

                return x.ID.ToString();
            });
        }

        private void dataGridModele_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            checkout.Clear();

            foreach (var piece in pieceCollec)
            {
                if (piece.IsSelected)
                {
                    checkout.Add(new PrixCheckoutViewModel(piece));
                }
            }
            foreach (var modele in modeleCollec)
            {
                if (modele.IsSelected)
                {
                    checkout.Add(new PrixCheckoutViewModel(modele));
                }
            }
            TB_PrixTotal.Text = checkout.Aggregate(0f, (x, y) => x += y.Prix).ToString() + "€";
        }

        private void cb_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] txt = this.cb_Client.SelectedItem.ToString().Split(' ');

            if (txt[1] == "Boutique")
            {
                this.TB_Remise.Text = BDDReader.GetObject<ClientBoutique>(txt[0]).Remise + "%";
            }
            else
            {
                this.TB_Remise.Text = BDDReader.GetObject<ClientIndividuel>(txt[0]).ProgrammeFidélité.Rabais + "%";
            }

            TB_PrixTotal.Text = (checkout.Aggregate(0f, (x, y) => x += y.Prix * y.Quantité) * (100 - int.Parse(TB_Remise.Text.Substring(0, TB_Remise.Text.Length - 1))) / 100).ToString() + "€";
        }

        private void Butt_Checkout_Click(object sender, RoutedEventArgs e)
        {
            int numCom = rand.Next();
            try
            {
                if (!int.TryParse(((string)cb_Client.SelectedItem).Split(' ').FirstOrDefault(), out int idCli))
                {
                    MessageBox.Show("Pas de client sélectionné !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                List<Commande> commande = new List<Commande>();
                foreach (var elem in checkout)
                {
                    if (elem.Type == "Pièce")
                    {
                        commande.Add(new Commande(null, numCom, idCli, elem.ID, null, DateTime.Now, DateTime.Now.AddDays(10)));
                    }
                    else if (elem.Type == "Vélo")
                    {
                        commande.Add(new Commande(null, numCom, idCli, null, int.Parse(elem.ID), DateTime.Now, DateTime.Now.AddDays(10)));
                    }
                }

                commande.ForEach(x => BDDWriter.Insert(x));

                if (SendMail(BDDReader.GetObject<Client>(idCli).AdresseMail, commande))
                {
                    MessageBox.Show("La commande a été passée avec succès. Le détail de la commande a été envoyé à l'email du client !", "Bravo !", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur interne, l'email n'a pas été envoyé, veuillez recommencer (le mail du client n'est peut etre pas valide, la commande aura toute fois bien été passée) !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Erreur interne, veuillez recommencer !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool SendMail(string to, List<Commande> com)
        {
            try
            {
                using (SmtpClient cli = new SmtpClient())
                {
                    cli.DeliveryMethod = SmtpDeliveryMethod.Network;
                    cli.UseDefaultCredentials = false;
                    cli.EnableSsl = true;
                    cli.Host = "smtp.gmail.com";
                    cli.Port = 587;
                    cli.Credentials = new NetworkCredential("velomax.noreply@gmail.com", "mdpMailSend98;");

                    MailMessage mail = new MailMessage("velomax.noreply@gmail.com", to)
                    {
                        Subject = "VELOMAX",
                        Priority = MailPriority.High,
                        IsBodyHtml = true
                    };

                    string nom = "";
                    if (com.FirstOrDefault().Client is ClientIndividuel ind)
                    {
                        nom = ind.Prénom + " " + ind.Nom;
                    }
                    else if(com.FirstOrDefault().Client is ClientBoutique bout)
                    {
                        nom = bout.NomEntreprise;
                    }


                    mail.AlternateViews.Add(GetEmbeddedImage($@"../../Images\mailHeader{rand.Next(1, 4)}.png", $@"<p>Bonjour {nom},</p><p></p><p>Voici le détail de votre commande référence 
                                        '{com.FirstOrDefault().IDCommande}' qui sera livrée le {com.FirstOrDefault().DateLivraison:dd/MM/yyyy} à l'adresse suivante : {com.FirstOrDefault().Client.Adresse}:
                                        </p><p></p><p>Merci pour votre confiance</p><p></p><p></p><p>{Mailtexte(com)}</p><p></p>"));

                    cli.Send(mail);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private AlternateView GetEmbeddedImage(String filePath, string body)
        {
            LinkedResource res = new LinkedResource(System.IO.Path.GetFullPath(filePath), MediaTypeNames.Image.Jpeg);
            res.ContentId = Guid.NewGuid().ToString();
            string htmlBody = $@"{body}<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }


        private string Mailtexte(List<Commande> a)
        {
            var client = a.FirstOrDefault().Client;
            float prixtotal = a.Aggregate(0f, (x, y) => x += (y.Piece?.Prix ?? 0) + (y.Modele?.Prix ?? 0));
            string textemail = "<ul>";
            foreach (var com in a)
            {
                if (com.Modele != null)
                {
                    textemail += $"<li><p>        {com.Modele.Ligne} de type {com.Modele.Nom} ref {com.Modele.ID} au prix de {com.Modele.Prix}€</p></li>";
                }
                else if (com.Piece != null)
                {
                    textemail += $"<li><p>        {com.Piece.Nom} {com.Piece.ID} au prix de {com.Piece.Prix}€</p></li>";
                }
                //textemail += "<p></p>";
            }

            textemail += $"</ul><p></p><p>Total TTC : {prixtotal}€</p>";

            if (client is ClientIndividuel ind && ind.ProgrammeFidélité != null)
            {
                textemail += $"<p></p><p>Total avec votre remise de {ind.ProgrammeFidélité.Rabais}% : {prixtotal * (100 - ind.ProgrammeFidélité.Rabais) / 100}€</p>";
            }

            return textemail;
        }
    }
}
