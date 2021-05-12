using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static bool IsConnected = true;

        public static bool MySQLServerConnected = false;

        private void ExecuteSQLScript()
        {
            var fileContent = File.ReadAllText("../../Database/velomax.sql");

            DataWriter.ExecuteNonQuery(fileContent);
        }

        /// <summary>
        /// On teste ici
        /// </summary>
        private void TestFunction()
        {
            var a = DataReader.Read<Assemblage>();
            var b = DataReader.Read<Adresse>();
            var c = DataReader.Read<Client>();
            var d = DataReader.Read<Commande>();
            var e = DataReader.Read<Fournisseurs>();
            var f = DataReader.Read<Pieces>();

        }

        /// <summary>
        /// On teste ici
        /// </summary>
        private void TestFunction2()
        {
            Adresse ad = new Adresse(null, "12 rue de la concierge", "Paris", "75009", "France");
            Adresse a = new Adresse(null, "13 rue de la concierge", "Paris", "75009", "France");
            ClientIndividuel jz = new ClientIndividuel(null, ad,"fred","jamie", "0622246543", "farid@gmail.com", 20, DataReader.Read<Fidelio>()[0]) ;
            ClientBoutique ja = new ClientBoutique(null,a, "0626542351","haribo@gmail.com", "Haribo", "delare");
            Compte je = new Compte(null, "durant", "regis");
            Assemblage jr = new Assemblage(null, "BlueJay", "Hommes ", "C32", "G7", "F3", "S88", "DV133", "DR56", "R48", "R46", "", "", "", "");

            DataWriter.Insert(ja);
            DataWriter.Insert(jz);
            DataWriter.Insert(ad);
            DataWriter.Insert(jr);

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ExecuteSQLScript();

            TestFunction();
            TestFunction2();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
