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
    /// Interaction logic for ConnectionPage.xaml
    /// </summary>
    public partial class ConnectionPage : UserControl
    {
        public ConnectionPage()
        {
            InitializeComponent();
        }

        private void Butt_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (App.MySQLServerConnected)
            {
                if (!string.IsNullOrWhiteSpace(this.TB_UserName.Text))
                {
                    if (!string.IsNullOrWhiteSpace(this.TB_UserPW.Password))
                    {
                        var compte = BDDReader.Get<Compte>(this.TB_UserName.Text, "pseudo");

                        if (compte != null && MyHelper.ComparePassword(this.TB_UserPW.Password, compte))
                        {
                            App.IsConnected = true;

                            App.Compte = compte;

                            Properties.Settings.Default.StayConnected = (bool)CB_StayConnect.IsChecked;

                            if (Properties.Settings.Default.StayConnected)
                            {
                                Properties.Settings.Default.UserConnectedAdmin = App.Admin;
                                Properties.Settings.Default.UserConnectedName = compte.Nom;
                            }

                            Properties.Settings.Default.Save();

                            App.MainWindow_.ChangePage(MyPages.Connecté);
                            App.MainWindow_.JustConnected();
                        }
                        else
                        {
                            MessageBox.Show("Nom d'utilisateur inexistant ou mot de passe incorrect", "Impossible de se connecter", MessageBoxButton.OK);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Impossible de se connecter au serveur pour le moment, veuillez réessayer quand la barre titre indiquera le contraire", "Impossible de se connecter", MessageBoxButton.OK);
            }
        }
    }
}
