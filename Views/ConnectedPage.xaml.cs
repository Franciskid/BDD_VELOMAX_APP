using System;
using System.Collections.Generic;
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

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Interaction logic for ConnectedPage.xaml
    /// </summary>
    public partial class ConnectedPage : UserControl
    {
        public ConnectedPage()
        {
            InitializeComponent();
            TB_UserName.Text = App.Compte.Nom;
        }

        private void Butt_Disconnect_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.FenetrePrincipale.ChangePage(MyPages.Connexion);
            MainWindow.FenetrePrincipale.JustDisconnected();

            App.IsConnected = false;
            App.Compte = null;

        }
    }
}
