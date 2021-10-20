using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
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
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private BackgroundWorker connexionWorker;
        private BackgroundWorker imageWorker;


        private MyPages currentPage;
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

                    case MyPages.Stock:
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

                    case MyPages.Autre:
                        //this.ButtOther.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#293f99"));
                        this.LogoOther.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        this.TB_Other.Foreground = (SolidColorBrush)FindResource("MenuFontColorFocus");
                        break;
                }
            }
        }


        #region Menu valeurs

        private const int MENUCLOSECONSTWIDTH = 92, MENUOPENCONSTWIDTH = 300, MENUCLOSEOPENTIMECONSTWIDTH = 500;

        private int menuCloseWidth = MENUCLOSECONSTWIDTH, menuOpenWidth = MENUOPENCONSTWIDTH, menuOpenCloseTimeMS = MENUCLOSEOPENTIMECONSTWIDTH;

        public int MenuCloseWidth
        {
            get => menuCloseWidth;
            set
            {
                this.menuCloseWidth = value;
                this.OnPropertyChanged("MenuCloseWidth");
            }
        }
        public int MenuOpenWidth
        {
            get => menuOpenWidth;
            set
            {
                this.menuOpenWidth = value;
                this.OnPropertyChanged("MenuOpenWidth");
            }
        }
        public string MenuOpenCloseTimeMS
        {
            get
            {
                return $"0:0:{((float)menuOpenCloseTimeMS / 1000).ToString(CultureInfo.GetCultureInfo("en-US"))}"; //Pour avoir un "." au lieu d'une ","
            }
            set
            {
                this.OnPropertyChanged("MenuOpenCloseTimeMS");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            this.GridMenu.Width = 0;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LaunchWorkerCheckConnexion();

            if (Properties.Settings.Default.RafraichissementImage)
            {
                LaunchWorkerChangeImage();
            }

            if (Properties.Settings.Default.StayConnected && BDDReader.ServerIsUp())
            {
                App.IsConnected = true;
                App.Compte = new Compte(null, Properties.Settings.Default.UserConnectedName, null);
                App.Admin = Properties.Settings.Default.UserConnectedAdmin;
                CurrentPage = MyPages.Connecté;

                this.menuOpenCloseTimeMS = 1000;
                this.MenuOpenCloseTimeMS = "";

                JustConnected(true);
            }
            else
            {
                CurrentPage = MyPages.Connexion;
                this.MenuOpenWidth = 0;
            }



            this.menuOpenCloseTimeMS = 500;
            this.MenuOpenCloseTimeMS = "";
        }


        #region Taches en arrière plan

        private void LaunchWorkerCheckConnexion()
        {
            this.connexionWorker = new BackgroundWorker();
            this.connexionWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork_Connection);
            this.connexionWorker.RunWorkerAsync();
        }

        private void LaunchWorkerChangeImage()
        {
            this.imageWorker = new BackgroundWorker();
            this.imageWorker.WorkerSupportsCancellation = true;
            this.imageWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork_ChangeImage);
            this.imageWorker.RunWorkerAsync();
        }


        private async void backgroundWorker_DoWork_Connection(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                CheckConnexion();

                System.Threading.Thread.Sleep(100);
            }
        }

        private void CheckConnexion()
        {
            if (BDDReader.ServerIsUp())
            {
                if (!App.MySQLServerConnected)
                {
                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                    {
                        App.MySQLServerConnected = true;

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
                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                    {
                        App.MySQLServerConnected = false;

                        this.TB_Connected.Text = (string)FindResource("CantConnect");
                        this.logoConnecté.Kind = PackIconKind.DatabaseRemove;
                        this.logoConnecté.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#c71c33"));
                    }));
                }
            }
        }


        private async void backgroundWorker_DoWork_ChangeImage(object sender, DoWorkEventArgs e)
        {
            int a = 2;
            Random r = new Random();
            long timestartImage = DateTime.Now.Ticks;

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                        ChangeImage((ImageBrush)MainBackgroundGrid.Background, (ImageBrush)MainBackgroundGrid2.Background, (ImageSource)FindResource($"BackImage{a}"), new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 2)))
                        );

            while (imageWorker?.CancellationPending == false)
            {
                if (timestartImage + (Properties.Settings.Default.RafraichissementImageSec * 10000000) < DateTime.Now.Ticks)
                {
                    int temp = r.Next(1, 11);

                    while (a == temp)
                        temp = r.Next(1, 11);
                    a = temp;

                    var source = this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                        ChangeImage((ImageBrush)MainBackgroundGrid.Background, (ImageBrush)MainBackgroundGrid2.Background, (ImageSource)FindResource($"BackImage{a}"), new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 2)))
                        );

                    timestartImage = DateTime.Now.Ticks;
                }

                System.Threading.Thread.Sleep(10); //Juste pour calmer le cpu
            }
        }

        private bool firstGrid = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private void ChangeImage(ImageBrush image1, ImageBrush image2, ImageSource newIm, TimeSpan fadeInTime, TimeSpan fadeOutTime)
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


        #endregion


        #region Boutons menu

        private bool buttonClickManual = false;

        private void ButtMenuOpen_Click(object sender, RoutedEventArgs e)
        {
            ButtMenuOpen.Visibility = Visibility.Hidden;
            ButtMenuClose.Visibility = Visibility.Visible;

            this.MenuCloseWidth = (int)this.GridMenu.ActualWidth;
            if (!buttonClickManual)
            {
                this.MenuOpenWidth = MENUOPENCONSTWIDTH;
            }
        }

        private void ButtMenuClose_Click(object sender, RoutedEventArgs e)
        {
            ButtMenuClose.Visibility = Visibility.Hidden;
            ButtMenuOpen.Visibility = Visibility.Visible;


            this.MenuOpenWidth = (int)this.GridMenu.ActualWidth;

            if (!buttonClickManual)
            {
                this.MenuCloseWidth = MENUCLOSECONSTWIDTH;
            }
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
            ChangePage(MyPages.Stock);
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
            ChangePage(MyPages.Autre);
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

        #endregion


        /// <summary>
        /// Permet de naviguer entre les pages
        /// </summary>
        /// <param name="page"></param>
        public void ChangePage(MyPages page)
        {
            if (App.IsConnected)
            {
                if (CurrentPage != page)
                {
                    CurrentPage = page;
                }
            }
            else if (page != MyPages.Connexion)
            {
                MessageBox.Show("Impossible de changer de page, vous devez d'abord vous authentifier pour y accéder !", "Erreur");
            }
        }

        /// <summary>
        /// Appeler quand on vient de se connecter
        /// </summary>
        public void JustConnected(bool animation = true)
        {
            this.TB_Connected.Text += $" ({App.Compte.Nom})";

            this.Butt_User.Visibility = Visibility.Visible;
            TB_UserCompte.Text = App.Compte.Nom;

            if (animation)
            {
                this.buttonClickManual = true;
                if (ButtMenuClose.Visibility == Visibility.Visible)
                {
                    this.MenuCloseWidth = MENUCLOSECONSTWIDTH;

                    ButtMenuClose.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
                else
                {
                    this.MenuOpenWidth = MENUOPENCONSTWIDTH;

                    ButtMenuOpen.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }

                this.buttonClickManual = false;
            }
        }

        /// <summary>
        /// Appeler quand on vient de se deconnecter
        /// </summary>
        public void JustDisconnected()
        {
            this.TB_Connected.Text = (string)FindResource("Connected");

            this.Butt_User.Visibility = Visibility.Collapsed;

            this.buttonClickManual = true;
            if (ButtMenuClose.Visibility == Visibility.Visible)
            {
                this.MenuOpenWidth = MENUOPENCONSTWIDTH;
                this.MenuCloseWidth = 0;
                ButtMenuClose.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
            else
            {
                this.MenuOpenWidth = 0;
                this.MenuCloseWidth = MENUCLOSECONSTWIDTH;
                ButtMenuOpen.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }

            this.GridMenu.Width = 0;
            this.buttonClickManual = false;
        }

        /// <summary>
        /// Met à jouer l'arret ou le démarrage  du rafraichissement de l'image d'arrière plan
        /// </summary>
        public void RefreshBackgroundState()
        {
            if (Properties.Settings.Default.RafraichissementImage && this.imageWorker == null)
            {
                LaunchWorkerChangeImage();
            }
            else if (!Properties.Settings.Default.RafraichissementImage && this.imageWorker != null)
            {
                this.imageWorker.CancelAsync();
                this.imageWorker.Dispose();
                this.imageWorker = null;
            }
        }


        #region Bar titre

        private void Butt_Paramètres_Click(object sender, RoutedEventArgs e)
        {
            Window win = new BDD_VELOMAX_APP.Views.ParamètresWindow();
            win.ShowDialog();

            RefreshBackgroundState();
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

        #endregion


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
        Stock,
        Commandes,
        Stats,
        Autre
    }

    public static partial class MyHelper
    {
        public static UserControl ToView(this MyPages m)
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
                    return new StatistiquesPage();

                case MyPages.Stock:
                    return new StockPage();

                case MyPages.Commandes:
                    return new CommandePage();

                case MyPages.Autre:
                    return new FidelioPage();

                default:
                    return null;
            }
        }
    }
}

