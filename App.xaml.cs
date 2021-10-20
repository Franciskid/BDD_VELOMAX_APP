using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CefSharp;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using CefSharp.Wpf;
using BDD_VELOMAX_APP.VeloMaxExtensions;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsConnected { get; set; } = false;

        public static bool Admin { get; set; } = true;

        private static Compte compte;
        public static Compte Compte
        {
            get => compte;
            set
            {
                compte = value;
                Admin = value != null && compte.Nom.ToLower() == "root";
            }
        }

        private static bool mySQLServerConnected = false;

        /// <summary>
        /// Setter => seulement depuis le thread principal | getter => peut etre appelé n'importe où
        /// </summary>
        public static bool MySQLServerConnected 
        {
            get => mySQLServerConnected;
            set
            {
                if (!value)
                {
                    if (mySQLServerConnected && IsConnected)
                    {
                        MessageBox.Show("Vous allez être déconnecté car le programme n'arrive plus à communiquer avec la base de donnée !", "Alerte", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    }

                    IsConnected = false;
                    Admin = false;
                    compte = null;

                    MainWindow_.CurrentPage = MyPages.Connexion;
                    MainWindow_.JustDisconnected();

                }

                mySQLServerConnected = value;
            }
        }


        /// <summary>
        /// Main Window, renvoie App.Current.MainWindow
        /// </summary>
        public static MainWindow MainWindow_ => (MainWindow)App.Current.MainWindow;


        /// <summary>
        /// Exécute les scripts nécessaires à la création, au peuplement et au bon fonctionnement de la BDD.
        /// </summary>
        private void ExecuteSQLScript()
        {
            BDDWriter.ExecuteNonQuery(File.ReadAllText("../../Database/velomax.sql"), false);

            BDDWriter.ExecuteScript(File.ReadAllText("../../Database/triggers.sql"));
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ExecuteSQLScript();

            this.MainWindow = new MainWindow();
            this.MainWindow.Show();

            var settings = new CefSettings();
            settings.RegisterScheme(new CefCustomScheme()
            {
                SchemeName = ResourceSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new ResourceSchemeHandlerFactory()
            });

            settings.CefCommandLineArgs.Add("disable-gpu"); // Disable GPU acceleration
            settings.CefCommandLineArgs.Add("disable-gpu-vsync"); //Disable GPU vsync

            Cef.Initialize(settings, performDependencyCheck: true);

        }

    }
}
