using MaterialDesignExtensions.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Interaction logic for ParamètresWindow.xaml
    /// </summary>
    public partial class ParamètresWindow : Window
    {
        public ParamètresWindow()
        {
            InitializeComponent();

            this.TB_rafraiche.Text = ConfigurationManager.AppSettings["RafraichissementImagesSec"].ToString();
            this.TB_Stock.Text = ConfigurationManager.AppSettings["StockFaibleLimite"].ToString();
        }

        private void Butt_Abort_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Butt_Save_Click(object sender, RoutedEventArgs e)
        { 
            bool isRefreshInt = int.TryParse(this.TB_rafraiche.Text, out int refresh);
            if (isRefreshInt)
            {
                ConfigurationManager.AppSettings.Set("RafraichissementImagesSec", refresh.ToString());
            }
            bool isStockInt = int.TryParse(this.TB_Stock.Text, out int stock);
            if (isStockInt)
            {
                ConfigurationManager.AppSettings.Set("StockFaibleLimite", stock.ToString());
            }
            this.DialogResult = true;

            this.Close();

        }

        private void Butt_JSON_Click(object sender, RoutedEventArgs e)
        {
            var cli = BDDReader.Read<ClientIndividuel>();
            var fidel = BDDReader.Read<Fidelio>();

            var cliSelec = from c in cli where (c.DateAdhésionProgramme + TimeSpan.FromDays((from f in fidel where f.ID.ToString() == c.ProgrammeFidélité.ID.ToString() select f).FirstOrDefault().Duree_annee * 365) < DateTime.Now.AddMonths(2)) select c;
            var export = new ExportData<ClientIndividuel>(ExportData<ClientIndividuel>.ExportType.JSON, cliSelec.ToList());

            string str = GetFilename(true);

            if (str != null)
            {
                if (export.Export(str))
                {
                    SendMail(string.IsNullOrWhiteSpace(this.TB_Mail.Text) ? "velomax.noreply@gmail.com" : this.TB_Mail.Text, export);
                    MessageBox.Show("L'export a bien eu lieu !", "!", MessageBoxButton.OK);
                }

            }
        }

        private void Butt_XML_Click(object sender, RoutedEventArgs e)
        {
            var export = new ExportData<Piece>(ExportData<Piece>.ExportType.XML, BDDReader.Read<Piece>($"select * from pieces where pieces.quantité < {ConfigurationManager.AppSettings["StockFaibleLimite"]}"));

            string str = GetFilename(false);

            if (str != null)
            {
                if (export.Export(str))
                {
                    if (SendMail(string.IsNullOrWhiteSpace(this.TB_Mail.Text) ? "velomax.noreply@gmail.com" : this.TB_Mail.Text, export))
                    {
                        MessageBox.Show("L'export a bien eu lieu !", "Wow bravo !", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("L'export n'a pas eu lieu !", "Erreur", MessageBoxButton.OK);
                    }
                    
                }

            }
        }

        private string GetFilename(bool json)
        {
            Microsoft.Win32.SaveFileDialog openFileDlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = json ? "json" : "xml",
                Filter = json ? "Fichiers JSON (*.json) | *.json" : "Fichiers XML(*.xml) | *.xml",
                AddExtension = true
            };
            if (openFileDlg.ShowDialog() == true)
                return openFileDlg.FileName;

            return null;
        }

        private bool SendMail<T>(string to, ExportData<T> data)
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

                    MailMessage mail = new MailMessage("velomax.noreply@gmail.com", to);
                    mail.Subject = "VELOMAX";
                    mail.Priority = MailPriority.High;
                    mail.Body = @"Nous vous remercions pour votre demande.\n" +
                        "Là voici en pièce jointe.";
                    mail.IsBodyHtml = true;
                    mail.AlternateViews.Add(GetEmbeddedImage(@"../../Images/mailHeader2.png", $@"<p>Bonjour,</p></p><p><p>Comme demandé via l'application, voici vos fichiers en pièce-jointe.</p><p></p><p>Merci pour votre confiance</p>"));
                    Attachment attac = new Attachment(data.FileName);
                    mail.Attachments.Add(attac);

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
    }
}
