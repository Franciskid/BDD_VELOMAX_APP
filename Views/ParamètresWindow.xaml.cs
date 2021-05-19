using MaterialDesignExtensions.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

            var cliSelec = from c in cli where c.DateAdhésionProgramme + TimeSpan.FromDays((from f in fidel where f.ID == c.ProgrammeFidélité.ID select f).FirstOrDefault().Duree_annee * 365) < DateTime.Now.AddMonths(2) select c;
            var export = new ExportData<ClientIndividuel>(ExportData<ClientIndividuel>.ExportType.JSON, cliSelec.ToList());

            export.Export(GetFilename(true));

            MessageBox.Show("L'export a bien eu lieu !", "!", MessageBoxButton.OK);
        }

        private void Butt_XML_Click(object sender, RoutedEventArgs e)
        {
            var export = new ExportData<Piece>(ExportData<Piece>.ExportType.XML, BDDReader.Read<Piece>($"select * from pieces where pieces.quantité < {ConfigurationManager.AppSettings["StockFaibleLimite"]}"));

            export.Export(GetFilename(false));

            MessageBox.Show("L'export a bien eu lieu !", "!", MessageBoxButton.OK);
        }

        private string GetFilename(bool json)
        {
            Microsoft.Win32.SaveFileDialog openFileDlg = new Microsoft.Win32.SaveFileDialog();
            openFileDlg.DefaultExt = json ? "json" : "xml";
            if (openFileDlg.ShowDialog() == true)
                return openFileDlg.FileName;

            return null;
        }
    }
}
