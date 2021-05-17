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

        public static MainWindow FenetrePrincipale { get; private set; }

        private BackgroundWorker connexionWorker = new BackgroundWorker();
        private BackgroundWorker imageWorker = new BackgroundWorker();

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
            LaunchWorkerChangeImage();

            FenetrePrincipale = this;
        }


        private void LaunchWorkerCheckConnexion()
        {
            this.connexionWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork_Connection);
            this.connexionWorker.RunWorkerAsync();
        }

        private void LaunchWorkerChangeImage()
        {
            this.imageWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork_ChangeImage);
            this.imageWorker.RunWorkerAsync();
        }

        private async void backgroundWorker_DoWork_Connection(object sender, DoWorkEventArgs e)
        {
            CheckConnexion();
        }
        private void CheckConnexion()
        {
            while (true)
            {
                if (BDDReader.ServerIsUp())
                {
                    if (!App.MySQLServerConnected)
                    {
                        App.MySQLServerConnected = true;

                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                        {
                            this.TB_Connected.Text = (string)FindResource("Connecting");
                            this.Spinner.Visibility = Visibility.Visible;
                            this.logoConnecté.Visibility = Visibility.Hidden;
                        }));

                        System.Threading.Thread.Sleep(1500);

                    }

                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                    {
                        this.TB_Connected.Text = (string)FindResource("Connected") + (App.IsConnected && App.Compte?.Nom != null ? $" ({App.Compte?.Nom})" : "");
                        this.logoConnecté.Kind = PackIconKind.DatabaseCheck;
                        this.logoConnecté.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#086e1e"));
                        this.Spinner.Visibility = Visibility.Hidden;
                        this.logoConnecté.Visibility = Visibility.Visible;
                    }));
                }
                else
                {
                    if (App.MySQLServerConnected)
                    {
                        App.MySQLServerConnected = false;

                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                        {
                            this.TB_Connected.Text = (string)FindResource("CantConnect");
                            this.logoConnecté.Kind = PackIconKind.DatabaseRemove;
                            this.logoConnecté.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#c71c33"));
                        }));
                    }
                }

                System.Threading.Thread.Sleep(100);
            }

        }

        private bool firstGrid = true;
        private Random r = new Random();
        private async void backgroundWorker_DoWork_ChangeImage(object sender, DoWorkEventArgs e)
        {
            int a = 2;

            while (true)
            {
                var source = this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                    ChangeImage2((ImageBrush)MainBackgroundGrid.Background, (ImageBrush)MainBackgroundGrid2.Background, (ImageSource)FindResource($"BackImage{a}"), new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 2)))
                    );
                System.Threading.Thread.Sleep((Int32)FindResource($"TempsMSChangementBackgroundImage"));

                int temp = r.Next(1, 11);

                while (a == temp)
                    temp = r.Next(1, 11);
                a = temp;

                //var source = this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                //    ChangeImage((ImageBrush)MainBackgroundGrid.Background, (ImageSource)FindResource($"BackImage{a}"), new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1)))
                //    );

            }
        }


        private void ChangeImage(ImageBrush image, ImageSource newIm, TimeSpan fadeInTime, TimeSpan fadeOutTime)
        {
            var fadeInAnimation = new DoubleAnimation(0, 1, fadeInTime);

            fadeInAnimation.Completed += (o, e) =>
            {
                image.Opacity = 1;
            };

            if (image.ImageSource != null)
            {
                var fadeOutAnimation = new DoubleAnimation(1, 0, fadeOutTime);

                fadeOutAnimation.Completed += (o, e) =>
                {
                    image.ImageSource = newIm;
                    image.Opacity = 0;
                    image.BeginAnimation(Brush.OpacityProperty, fadeInAnimation, HandoffBehavior.Compose);
                };

                image.BeginAnimation(Brush.OpacityProperty, fadeOutAnimation);
            }
            else
            {
                image.Opacity = 0;
                image.ImageSource = newIm;
                image.BeginAnimation(Brush.OpacityProperty, fadeInAnimation, HandoffBehavior.Compose);
            }
        }

        private void ChangeImage2(ImageBrush image1, ImageBrush image2, ImageSource newIm, TimeSpan fadeInTime, TimeSpan fadeOutTime)
        {
            var fadeInAnimation = new DoubleAnimation(0, 1, fadeInTime);

            if (image1 != null || image2 != null)
            {
                if (firstGrid)
                {
                    image2.ImageSource = newIm;

                    fadeInAnimation.Completed += (o, e) =>
                    {
                        image2.Opacity = 1;
                    };

                    var fadeOutAnimation = new DoubleAnimation(1, 0, fadeOutTime);

                    image1.BeginAnimation(Brush.OpacityProperty, fadeOutAnimation);
                    image2.BeginAnimation(Brush.OpacityProperty, fadeInAnimation);
                }
                else
                {
                    image1.ImageSource = newIm;

                    fadeInAnimation.Completed += (o, e) =>
                    {
                        image1.Opacity = 1;
                    };

                    var fadeOutAnimation = new DoubleAnimation(1, 0, fadeOutTime);


                    image2.BeginAnimation(Brush.OpacityProperty, fadeOutAnimation);
                    image1.BeginAnimation(Brush.OpacityProperty, fadeInAnimation);
                }
                firstGrid = !firstGrid;
            }
            else
            {
                image1.Opacity = 0;
                image1.ImageSource = newIm;
                image1.BeginAnimation(Brush.OpacityProperty, fadeInAnimation);
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

        public void ChangePage(MyPages page)
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


        /// <summary>
        /// Appeler quand on vient de se connecter
        /// </summary>
        public void JustConnected()
        {
            this.TB_Connected.Text += $" ({App.Compte.Nom})";

            this.Butt_User.Visibility = Visibility.Visible;
            TB_UserCompte.Text = App.Compte.Nom;
        }

        /// <summary>
        /// Appeler quand on vient de se connecter
        /// </summary>
        public void JustDisconnected()
        {
            this.TB_Connected.Text = (string)FindResource("Connected");

            this.Butt_User.Visibility = Visibility.Collapsed;
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

                case MyPages.Connexion:
                    return new ConnectionPage();

                case MyPages.Connecté:
                    return new ConnectedPage();
                case MyPages.Stats:
                    return new Pagestastique();
                case MyPages.Pieces:
                    return new Pagestock();


                default:
                    return null;
            }
        }
    }
}

