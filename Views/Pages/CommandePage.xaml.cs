using BDD_VELOMAX_APP.VeloMaxExtensions;
using BDD_VELOMAX_APP.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
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
        private List<Assemblage> assemblages;

        public ObservableCollection<ModeleCommandeViewModel> modeleCollec = new ObservableCollection<ModeleCommandeViewModel>();

        public ObservableCollection<PieceCommandeViewModel> pieceCollec = new ObservableCollection<PieceCommandeViewModel>();

        public ObservableCollection<PrixCheckoutViewModel> checkout = new ObservableCollection<PrixCheckoutViewModel>();

        public ObservableCollection<CommandeViewModel> commandes = new ObservableCollection<CommandeViewModel>();


        private static Random rand = new Random(Guid.NewGuid().GetHashCode());

        public CommandePage()
        {
            InitializeComponent();

            var pieces = BDDReader.Read<Piece>();
            var modeles = BDDReader.Read<Modele>();
            assemblages = BDDReader.Read<Assemblage>();
            var clients = BDDReader.Read<Client>();
            var com = BDDReader.Read<Commande>();

            this.dataGridModele.ItemsSource = modeleCollec;
            this.dataGridPiece.ItemsSource = pieceCollec;

            pieces.ForEach(x => pieceCollec.Add(new PieceCommandeViewModel(x)));
            modeles.ForEach(x => modeleCollec.Add(new ModeleCommandeViewModel(x, assemblages.Where(y => y.ID.Equals(x.ID)).FirstOrDefault().Grandeurs)));
            com.ForEach(x => commandes.Add(new CommandeViewModel(x, x.Modele != null ? assemblages.Where(y => y.ID.Equals(x.Modele.ID)).FirstOrDefault() : null)));

            this.DataGridCheckout.ItemsSource = checkout;
            this.DataGridCommandesOld.ItemsSource = commandes;
            ICollectionView cvTasks = CollectionViewSource.GetDefaultView(DataGridCommandesOld.ItemsSource);
            if (cvTasks != null && cvTasks.CanGroup == true)
            {
                cvTasks.GroupDescriptions.Clear();
                cvTasks.GroupDescriptions.Add(new PropertyGroupDescription("IDCommande"));
            }
            this.DataGridCommandesOld.ItemsSource = cvTasks;

            cb_Client.ItemsSource = clients.Select(x =>
            {
                if (x is ClientBoutique bout)
                    return $"{MyHelper.StringLen(x.ID.ToString(), 3)} | {MyHelper.StringLen("Boutique", 10)} | {bout.NomEntreprise}";
                if (x is ClientIndividuel indiv)
                    return $"{MyHelper.StringLen(x.ID.ToString(), 3)} | {MyHelper.StringLen("Individuel", 10)} | {indiv.Prénom} {indiv.Nom}";

                return x.ID.ToString();
            });
        }

        private void dataGridModele_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (var piece in pieceCollec)
            {
                if (piece.Sélectionné && !checkout.Any(x => x.ID == piece.Id))
                {
                    checkout.Add(new PrixCheckoutViewModel(piece));
                }
            }
            foreach (var modele in modeleCollec)
            {
                if (modele.Sélectionné && !checkout.Any(x => x.ID == modele.ID.ToString()))
                {
                    checkout.Add(new PrixCheckoutViewModel(modele));
                }
            }

            foreach (var elem in checkout.ToList())
            {
                if (!pieceCollec.Any(x => x.Id == elem.ID && x.Sélectionné) && !modeleCollec.Any(x => x.ID.ToString() == elem.ID && x.Sélectionné))
                    checkout.Remove(elem);
            }

            TB_PrixTotal.Text = checkout.Aggregate(0f, (x, y) => x += y.Prix * y.Quantité).ToString() + "€";
        }

        private void cb_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] txt = this.cb_Client.SelectedItem.ToString().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (txt[2] == "Boutique")
            {
                this.TB_Remise.Text = BDDReader.Get<ClientBoutique>(txt[0]).Remise + "%";
            }
            else
            {
                var client = BDDReader.Get<ClientIndividuel>(txt[0]);

                this.TB_Remise.Text = client.ProgrammeFidélité == null ? "0%" : client.ProgrammeFidélité.Rabais + "%";
            }

            TB_PrixTotal.Text = (checkout.Aggregate(0f, (x, y) => x += y.Prix * y.Quantité) * (100 - int.Parse(TB_Remise.Text.Substring(0, TB_Remise.Text.Length - 1))) / 100).ToString() + "€";
        }

        private void Butt_Checkout_Click(object sender, RoutedEventArgs e)
        {
            int numCom = rand.Next();
            try
            {
                if (!int.TryParse(((string)(cb_Client.SelectedItem ?? "")).Split(' ').FirstOrDefault(), out int idCli))
                {
                    throw new Exception("Pas de client sélectionné !");
                }
                else if (checkout.Count() <= 0)
                {
                    throw new Exception("Pas d'éléments sélectionnés pour la commande !");
                }
                else
                {
                    List<Commande> commande = new List<Commande>();
                    foreach (var elem in checkout)
                    {
                        if (elem.Type == "Pièce")
                        {
                            commande.Add(new Commande(null, numCom, idCli, elem.ID, null, elem.Quantité, DateTime.Now, DateTime.Now.AddDays(10)));
                        }
                        else if (elem.Type == "Vélo")
                        {
                            commande.Add(new Commande(null, numCom, idCli, null, int.Parse(elem.ID), elem.Quantité, DateTime.Now, DateTime.Now.AddDays(10)));
                        }
                    }
                    var html = GetHTMLText(commande, false);
                    var confirmation = new CommandeConfirmationWindow(html.Item1, html.Item2, commande.FirstOrDefault().Client.AdresseMail);

                    confirmation.ShowDialog();
                    if (confirmation.DialogResult == true)
                    {
                        App.Current.MainWindow.Cursor = Cursors.Wait;
                        this.Cursor = Cursors.Wait;

                        foreach (var c in commande)
                        {
                            if (BDDWriter.Insert(c) < 0)
                                throw new Exception("Bozo ne peut pas faire de commande !");
                        }

                        commande.ForEach(x => commandes.Add(new CommandeViewModel(x, x.Modele != null ? assemblages.Where(y => y.ID.Equals(x.Modele.ID)).FirstOrDefault() : null)));

                        if (SendMail(BDDReader.Get<Client>(idCli), commande))
                        {
                            MyMessageBox.Show("La commande a été passée avec succès. Le détail de la commande a été envoyé à l'email du client !", "Bravo !", 4000);
                        }
                        else
                        {
                            throw new Exception("L'email n'a pas été envoyé, veuillez recommencer (la configuration de votre connexion internet peut limiter l'envoi de mails automatiques, la commande aura toute fois bien été passée) !");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur interne, veuillez recommencer !\n\nMessage : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                App.Current.MainWindow.Cursor = Cursors.Arrow;
            }
        }


        private bool SendMail(Client dest, List<Commande> com)
        {
            try
            {
                var code = GetHTMLText(com);

                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(code.Item1, null, MediaTypeNames.Text.Html);
                code.Item2.ForEach(x => alternateView.LinkedResources.Add(x));

                var pdf = MyPDF.Create(dest, com, null);
                MemoryStream stream = new MemoryStream();
                pdf.Save(stream, false);

                Attachment attach = new Attachment(stream, new ContentType(MediaTypeNames.Application.Pdf));
                attach.Name = $"Facture_commande_{com.FirstOrDefault().IDCommande}.pdf";

                //MyHelper.SendMail($"Commande VeloMax : {com.FirstOrDefault().IDCommande}", dest.AdresseMail, alternateView, new List<Attachment>() { attach });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private (string, List<MyLinkedResource>) GetHTMLText(List<Commande> com, bool mail = true)
        {
            string nom = "";
            if (com.FirstOrDefault().Client is ClientIndividuel ind)
            {
                nom = ind.Prénom + " " + ind.Nom;
            }
            else if (com.FirstOrDefault().Client is ClientBoutique bout)
            {
                nom = bout.NomEntreprise;
            }

            MyLinkedResource resVelo = new MyLinkedResource($@"../../Images/velomaxSpeedLogoWhite.png", MediaTypeNames.Image.Jpeg)
            {
                ContentId = Guid.NewGuid().ToString()
            };
            MyLinkedResource resContact = new MyLinkedResource($@"../../Images/mailHeader{rand.Next(1, 4)}.png", MediaTypeNames.Image.Jpeg)
            {
                ContentId = Guid.NewGuid().ToString()
            };

            string htmlTexte = $@"<html><head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head>";
            htmlTexte += $@"<body><div style='width: 800px; margin: auto; background-color: #542abf; padding: 90px; color: white; border-radius: 50px;position:relative;'>";
            htmlTexte += "<div>";
            htmlTexte += $@"<img src='{(mail ? $"cid:{resVelo.ContentId}" : resVelo.ResourceFileName)}' alt='velomax logo' width='110' height='110' align='right' style='margin:-2em'/></div>";
            htmlTexte += $@"<p style='font-size: 20pt;'>Bonjour <strong>{nom}</strong>,</p>";
            htmlTexte += $@"<p style='padding: 0px 0px 0px 1em; text-align: justify;font-size: 14pt;'><br/>
Vous trouverez ci-dessous le détail de votre commande référence <em style='font-size: 16pt;'>{com.FirstOrDefault().IDCommande}</em> qui sera livrée le <strong style='font-size: 14pt;'>{com.FirstOrDefault().DateLivraison:dd/MM/yyyy}</strong> à l'adresse suivante : <br /><br /></p>";
            htmlTexte += " <ul style='list-style-type:none; font-size: 18px; font-weight:bold;'>";
            htmlTexte += $"<li>{com.FirstOrDefault().Client.Adresse.Rue}</li>";
            htmlTexte += $"<li>{com.FirstOrDefault().Client.Adresse.CodePostal}&nbsp;&nbsp;&nbsp;&nbsp;{com.FirstOrDefault().Client.Adresse.Ville}</li>";
            htmlTexte += $"<li>{com.FirstOrDefault().Client.Adresse.Province}</li></ul><br/>";
            htmlTexte += $@"<p style='padding: 0px 0px 0px 1em;font-size: 14pt;'>Merci pour votre confiance !</p>";
            htmlTexte += $@"<p style='font-size: 18pt;'>L'équipe <strong>VéloMax</strong>.</p><br/><br/>";
            htmlTexte += $@"<div style='margin: 1em auto; padding: 2em  0  1em 4em; background-color: #6c3de3; border-radius: 50px;'>
<p style='font-size: 22pt; '><span style='text-decoration: underline; '><strong>Commande </strong>(ref. {com.FirstOrDefault().IDCommande}) </span>:</p>";
            htmlTexte += ListeCommande(com);
            htmlTexte += "</div>";
            htmlTexte += $@"<img src='{(mail ? $"cid:{resContact.ContentId}" : resContact.ResourceFileName)}' alt='contact'/></div></body></html>";

            return (htmlTexte, new List<MyLinkedResource>() { resVelo, resContact });
        }


        private string ListeCommande(List<Commande> a)
        {
            var client = a.FirstOrDefault().Client;
            float prixtotal = a.Aggregate(0f, (x, y) => x += ((y.Piece?.Prix ?? 0) + (y.Modele?.Prix ?? 0)) * y.Quantité);
            var modeles = a.Where(x => x.Modele != null);
            var pieces = a.Where(x => x.Piece != null);

            string textemail = "";
            textemail += "<ol style='list-style-type: upper-roman; '>";

            if (modeles.Count() > 0)
            {
                textemail += "<li style='font-size: 18pt; padding-top:1em;'>VÉLOS</li>";
                textemail += "<ol type='1'>";

                foreach (var mod in modeles)
                {
                    textemail += $@"<li style='font-size: 14pt;'><p>{mod.Modele.Ligne.ToString().Replace('_', ' ')}&nbsp;&nbsp;<i>{mod.Modele.Nom.ToString().Replace('_', ' ')}</i>&nbsp;&nbsp;({assemblages.Where(y => y.ID.Equals(mod.Modele.ID)).FirstOrDefault().Grandeurs})&nbsp;&nbsp;&nbsp;ref. {mod.Modele.ID}</li> <ul  style='font-size: 12pt;'>   <li type = 'disc' ><b>Prix à l'unité : {mod.Modele.Prix}</b>€</li> <li type = 'disc'><b>Quantité : {mod.Quantité}</b></li>  </ul>";
                }

                textemail += "</ol>";
            }
            if (pieces.Count() > 0)
            {
                textemail += "<li style='font-size: 18pt; padding-top:2em;'>PIÈCES DÉTACHÉES</li>";
                textemail += "<ol type='1'>";

                foreach (var piece in pieces)
                {
                    textemail += $@"<li style='font-size: 14pt;'><p>{piece.Piece.Nom}&nbsp;&nbsp;&nbsp;ref. <i>{piece.Piece.ID}</i></li>  <ul style='font-size: 12pt;'>   <li type = 'disc'><b>Prix à l'unité : {piece.Piece.Prix}</b>€</li> <li type = 'disc'><b>Quantité : {piece.Quantité}</b></li>   </ul>";
                }

                textemail += "</ol>";
            }

            textemail += $@"</ol><p style='font-size: 18pt; '>&nbsp;</p><p style='font-size:18pt'>Total TTC : <b>{prixtotal.ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " })}</b>€</p>";

            if (client is ClientIndividuel ind && ind.ProgrammeFidélité != null)
            {
                textemail += $@"<p style='font-size:24pt'>Total avec votre remise de {ind.ProgrammeFidélité.Rabais}% : <b>{(prixtotal * (100 - ind.ProgrammeFidélité.Rabais) / 100).ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " })}</b>€</p>";
            }
            else if (client is ClientBoutique bout && bout.Remise != 0)
            {
                textemail += $@"<p style='font-size:24pt'>Total avec votre remise de {bout.Remise}% : <b>{(prixtotal * (100 - bout.Remise) / 100).ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " })}</b>€</p>";
            }

            return textemail;
        }
    }


    public class MyLinkedResource : LinkedResource
    {
        /// <summary>
        /// Pas absolu, c'est le chemin depuis le .exe
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// A utiliser dans le code html en tant que source pour faire ref à une resource
        /// </summary>
        public string ResourceFileName => $"{ResourceSchemeHandlerFactory.SchemeName}://{FileName}";

        public MyLinkedResource(string filename, string mediaType) : base(System.IO.Path.GetFullPath(filename), mediaType)
        {
            this.FileName = filename;
        }
    }


}
