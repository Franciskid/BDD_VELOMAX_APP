using MaterialDesignThemes.Wpf;
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

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyPages currentPage;

        public MyPages CurrentPage
        {
            get => currentPage;
            set
            {
                ActivateItemAsync.Content = value.ToView();
                currentPage = value;

                ModifyColorButton();
                switch (value)
                {
                    case MyPages.Connexion:
                    case MyPages.Connecté:
                        this.ButtMain.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoMain.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cc7523"));
                        break;

                    case MyPages.Clients:
                        this.ButtClients.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF13480B"));
                        this.LogoClients.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cc7523"));
                        break;

                    case MyPages.Commandes:
                        this.ButtCommande.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF13480B"));
                        this.LogoCommandes.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cc7523"));
                        break;

                    case MyPages.Stats:
                        this.ButtStats.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF13480B"));
                        this.LogoStats.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cc7523"));
                        break;

                    case MyPages.Other:
                        this.ButtOther.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF13480B"));
                        this.LogoOther.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cc7523"));
                        break;
                }
            }
        }

        private void ModifyColorButton()
        {
            List<Button> buttons = new List<Button>()
            {
                this.ButtMain,
                ButtClients,
                ButtCommande,
                ButtStats,
                ButtOther
            };
            List<PackIcon> logos = new List<PackIcon>()
            {
                LogoMain,
                LogoClients,
                LogoCommandes,
                LogoStats,
                LogoOther,
            };

            buttons.ForEach(x => x.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0d1536"));
            logos.ForEach(x => x.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#d7d7f7"));
        }


        public MainWindow()
        {
            InitializeComponent();

            CurrentPage = MyPages.Connexion;
        }

        private void ButtMenuOpen_Click(object sender, RoutedEventArgs e)
        {
            ButtMenuOpen.Visibility = Visibility.Hidden;
            ButtMenuClose.Visibility = Visibility.Visible;
        }

        private void ButtMenuClose_Click(object sender, RoutedEventArgs e)
        {
            ButtMenuClose.Visibility = Visibility.Hidden;
            ButtMenuOpen.Visibility = Visibility.Visible;
        }


        private void ButtMain_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = App.IsConnected ? MyPages.Connecté : MyPages.Connexion;
        }

        private void ButtClients_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(MyPages.Clients);
        }

        private void ButtCommande_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(MyPages.Commandes);
        }

        private void ButtStats_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(MyPages.Stats);
        }

        private void ButtOther_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(MyPages.Other);
        }

        private void ChangePage(MyPages page)
        {
            if (App.IsConnected)
            {
                if (CurrentPage != page)
                {
                    CurrentPage = page;
                }
            }
            else
            {
                MessageBox.Show("Impossible de changer de page, vous devez d'abord vous authentifier pour y accéder !", "Erreur");
            }
        }

        private void GridTitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.Current.MainWindow.DragMove();
            }
            catch { }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }

    public enum MyPages
    {
        Connexion,
        Connecté,
        Clients,
        Commandes,
        Stats,
        Other
    }

    public static partial class MyHelper
    {
        public static object ToView(this MyPages m)
        {
            //switch (m)
            //{
            //    case MyPages.Clients:
            //        return new ClientsPageView();

            //    case MyPages.Commandes:
            //        return new CommandesPageView();

            //    case MyPages.Stats:
            //        return new StatsPageView();

            //    case MyPages.Connecté:
            //        return new ConnectéPageView();

            //    case MyPages.Other:
            //        return new OtherPageView();

            //    default:
            //    case MyPages.Connexion:
            //        return new ConnexionPageView();
            //}
            return null;
        }
    }
}

