using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using BDD_VELOMAX_APP.Views;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BackgroundWorker connexionWorker = new BackgroundWorker();

        MyPages currentPage;

        public MyPages CurrentPage
        {
            get => currentPage;
            set
            {
                ContentItem.Content = value.ToView();
                currentPage = value;

                ModifyColorButton();
                switch (value)
                {
                    case MyPages.Connexion:
                    case MyPages.Connecté:
                        //this.ButtMain.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoMain.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");//(new BrushConverter().ConvertFrom("#cc7523"));
                        this.TB_Main.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        break;

                    case MyPages.Clients:
                        //this.ButtClients.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoClients.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        this.TB_Clients.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        break;

                    case MyPages.Pieces:
                        //this.ButtClients.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoPieces.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        this.TB_Pieces.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        break;

                    case MyPages.Commandes:
                        //this.ButtCommande.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoCommandes.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        this.TB_Commandes.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        break;

                    case MyPages.Stats:
                        //this.ButtStats.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoStats.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        this.TB_Stats.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        break;

                    case MyPages.Other:
                        //this.ButtOther.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoOther.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        this.TB_Other.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
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
                ButtPieces,
                ButtStats,
                ButtOther
            };
            List<TextBlock> tb = new List<TextBlock>()
            {
                this.TB_Clients,
                TB_Commandes,
                TB_Pieces,
                TB_Main,
                TB_Other,
                TB_Stats
            };
            List<PackIcon> logos = new List<PackIcon>()
            {
                LogoMain,
                LogoClients,
                LogoCommandes,
                LogoPieces,
                LogoStats,
                LogoOther,
            };

            //buttons.ForEach(x => x.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#00000000"));
            logos.ForEach(x => x.Foreground = (SolidColorBrush)FindResource("MenuFontColor"));//BrushConverter().ConvertFrom("#d7d7f7"));
            tb.ForEach(x => x.Foreground = (SolidColorBrush)FindResource("MenuFontColor")); //new BrushConverter().ConvertFrom("#d7d7f7"));
        }


        public MainWindow()
        {
            InitializeComponent();

            CurrentPage = MyPages.Connexion;

            LaunchWorkerCheckConnexion();

        }


        private void LaunchWorkerCheckConnexion()
        {
            this.connexionWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.connexionWorker.RunWorkerAsync();
        }

        private async void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckConnexion();
        }

        public void CheckConnexion()
        {
            while (true)
            {
                if (DataReader.ServerIsUp())
                {
                    if (!App.MySQLServerConnected)
                    {
                        App.MySQLServerConnected = true;

                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                        {
                            this.TB_Connected.Text = "Connexion au serveur";
                            this.Spinner.Visibility = Visibility.Visible;
                            this.logoConnecté.Visibility = Visibility.Hidden;
                        }));

                        System.Threading.Thread.Sleep(1000);

                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                        {
                            this.TB_Connected.Text = "Connecté au serveur";
                            this.logoConnecté.Kind = PackIconKind.DatabaseCheck;
                            this.logoConnecté.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#086e1e"));
                            this.Spinner.Visibility = Visibility.Hidden;
                            this.logoConnecté.Visibility = Visibility.Visible;
                        }));
                    }
                }
                else
                {
                    if (App.MySQLServerConnected)
                    {
                        App.MySQLServerConnected = false;

                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                        {
                            this.TB_Connected.Text = "Impossible de se connecter au serveur";
                            this.logoConnecté.Kind = PackIconKind.DatabaseRemove;
                            this.logoConnecté.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#c71c33"));
                        }));
                    }
                }

                System.Threading.Thread.Sleep(50);
            }

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

        private void ButtPieces_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(MyPages.Pieces);
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
        Pieces,
        Commandes,
        Stats,
        Other
    }

    public static partial class MyHelper
    {
        public static object ToView(this MyPages m)
        {
            switch (m)
            {
                case MyPages.Clients:
                    return new ClientPage();

                default:
                    return null;
            }
            return null;
        }
    }
}

