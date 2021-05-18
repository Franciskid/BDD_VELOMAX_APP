using System;
using System.Collections.Generic;
using System.Configuration;
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
    }
}
