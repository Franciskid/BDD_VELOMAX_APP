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
using BDD_VELOMAX_APP.Views.Windows;

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

            this.TB_rafraiche.Text = Properties.Settings.Default.RafraichissementImageSec.ToString();// ConfigurationManager.AppSettings["RafraichissementImagesSec"].ToString();
            this.TB_Stock.Text = Properties.Settings.Default.StockLimite.ToString();// ConfigurationManager.AppSettings["StockFaibleLimite"].ToString();
            this.TB_Time.Text = Properties.Settings.Default.TempsLimiteJours.ToString();
            this.CB_FreqImage.IsChecked = Properties.Settings.Default.RafraichissementImage;

            this.TB_Mail.Text = Properties.Settings.Default.MailExport;
        }

        private void Butt_Abort_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Butt_Save_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(this.TB_rafraiche.Text, out int refresh))
            {
                Properties.Settings.Default.RafraichissementImageSec = refresh;
            }
            Properties.Settings.Default.RafraichissementImage = (bool)this.CB_FreqImage.IsChecked;

            if (int.TryParse(this.TB_Stock.Text, out int stock))
            {
                Properties.Settings.Default.StockLimite = stock;
            }
            if (int.TryParse(this.TB_Time.Text, out int temps))
            {
                Properties.Settings.Default.TempsLimiteJours = temps;
            }

            Properties.Settings.Default.MailExport = this.TB_Mail.Text;

            Properties.Settings.Default.Save();

            MyMessageBox.Show("Les paramètres ont bien été enregistrés !", "Nice", 1500);

            App.MainWindow_.RefreshBackgroundState();
            //this.DialogResult = true;
            //this.Close();

        }

        private void Butt_JSON_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsConnected)
            {
                var cli = BDDReader.Read<ClientIndividuel>();
                var fidel = BDDReader.Read<Fidelio>();

                var cliSelec = from c in cli where (c.ProgrammeFidélité != null && c.DateAdhésionProgramme + TimeSpan.FromDays((from f in fidel where f.ID.ToString() == c.ProgrammeFidélité.ID.ToString() select f).FirstOrDefault().Duree_annee * 365) < DateTime.Now.AddDays(Properties.Settings.Default.TempsLimiteJours)) select c;
                var export = new ExportData<ClientIndividuel>(ExportData<ClientIndividuel>.ExportType.JSON, cliSelec.ToList());

                string str = GetFilename(true);

                if (str != null)
                {
                    if (export.Export(str))
                    {
                        if (SendMail(string.IsNullOrWhiteSpace(this.TB_Mail.Text) ? Properties.Settings.Default.MailExport : this.TB_Mail.Text, export))
                        {
                            MyMessageBox.Show("L'export a bien eu lieu !", "Wow bravo !", 3000);
                            Properties.Settings.Default.MailExport = this.TB_Mail.Text;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            MessageBox.Show("L'export n'a pas eu lieu !", "Erreur", MessageBoxButton.OK);
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("Bien tenté ! Mais il faut être connecté pour accéder à ces données !", "Dommage");
            }
        }

        private void Butt_XML_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsConnected)
            {
                var export = new ExportData<Piece>(ExportData<Piece>.ExportType.XML, BDDReader.Read<Piece>($"select * from pieces where pieces.quantité < {Properties.Settings.Default.StockLimite}"));

                string str = GetFilename(false);

                if (str != null)
                {
                    if (export.Export(str))
                    {
                        if (SendMail(string.IsNullOrWhiteSpace(this.TB_Mail.Text) ? Properties.Settings.Default.MailExport : this.TB_Mail.Text, export))
                        {
                            MyMessageBox.Show("L'export a bien eu lieu !", "Wow bravo !", 3000);
                            Properties.Settings.Default.MailExport = this.TB_Mail.Text;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            MessageBox.Show("L'export n'a pas eu lieu !", "Erreur", MessageBoxButton.OK);
                        }

                    }

                }
            }
            else
            {
                MessageBox.Show("Bien tenté ! Mais il faut être connecté pour accéder à ces données !", "Dommage");
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

        static Random rand = new Random(Guid.NewGuid().GetHashCode());
        private bool SendMail<T>(string to, ExportData<T> data)
        {
            try
            {
                MailMessage mail = new MailMessage(ConfigurationManager.AppSettings["mailVelomax"], to)
                {
                    Subject = "VELOMAX",
                    Priority = MailPriority.Normal,
                    IsBodyHtml = true
                };
                mail.AlternateViews.Add(GetEmbeddedImage($@"../../Images/mailHeader{rand.Next(1, 4)}.png", $@"<p>Bonjour,</p></p><p><p>Comme demandé via l'application, voici vos fichiers en pièce-jointe.</p><p></p><p>Merci pour votre confiance</p>"));
                Attachment attac = new Attachment(data.FileName);
                mail.Attachments.Add(attac);

                MyHelper.SendMail(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private AlternateView GetEmbeddedImage(String filePath, string body)
        {
            LinkedResource res = new LinkedResource(System.IO.Path.GetFullPath(filePath), MediaTypeNames.Image.Jpeg)
            {
                ContentId = Guid.NewGuid().ToString()
            };
            string htmlBody = $@"{body}<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }



        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SystemCommands.ShowSystemMenu(this, GetMousePosition());
        }

        #region Helper mouse pos

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(this);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + this.Left, position.Y + this.Top);
        }

        #endregion
    }
}
