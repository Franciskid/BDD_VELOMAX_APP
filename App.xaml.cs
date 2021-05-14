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

        public static bool MySQLServerConnected { get; set; } = false;

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
            var b = DataReader.Read<Adresse>();
            var a = DataReader.Read<Assemblage>();
            var c = DataReader.Read<Client>();
            var d = DataReader.Read<Commande>();
            var cazd = DataReader.Read<Compte>();
            var daz = DataReader.Read<Fidelio>();
            var eef = DataReader.Read<Fournisseurs>();
            var f = DataReader.Read<Modele>();
            var fe = DataReader.Read<Pieces>();

        }

        /// <summary>
        /// On teste ici
        /// </summary>
        private void TestFunction2()
        {

            Adresse ad1 = new Adresse(null, "igrgfeh", "iuh", "ibkb", "iugaud");
            Adresse ad2 = new Adresse(null, "iazgh", "iuazh", "ibkb", "iugaud");
            Adresse ad3 = new Adresse(null, "igazdh", "idazuh", "azdibkb", "iugaud");


            int id1 = (int)DataWriter.Insert(ad1);
            int id2 = (int)DataWriter.Insert(ad2);
            int id3 = (int)DataWriter.Insert(ad3);

            var o1 = DataReader.GetObject<Adresse>(id1);
            var o2 = DataReader.GetObject<Adresse>(id2);


            o1.CodePostal = "UPDATE";

            bool b = DataWriter.Update(o1);

            bool bb = DataWriter.Remove(o2);
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
