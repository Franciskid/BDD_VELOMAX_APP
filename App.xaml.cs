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

        public static bool IsConnected { get; set; } = true;

        public static bool Admin { get; set; } = true;

        private static Compte compte = new Compte(0, "TEST", "");
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

            TestFunction();
            TestFunction2();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }




        /// <summary>
        /// On teste ici
        /// </summary>
        private void TestFunction()
        {
            var b = BDDReader.Read<Adresse>();
            var a = BDDReader.Read<Assemblage>();
            var c = BDDReader.Read<Client>();
            var cdd = BDDReader.Read<ClientBoutique>();
            var cded = BDDReader.Read<ClientIndividuel>();
            var d = BDDReader.Read<Commande>();
            var cazd = BDDReader.Read<Compte>();
            var daz = BDDReader.Read<Fidelio>();
            var eef = BDDReader.Read<Fournisseurs>();
            var f = BDDReader.Read<Modele>();
            var fe = BDDReader.Read<Piece>();
        }

        /// <summary>
        /// On teste ici
        /// </summary>
        private void TestFunction2()
        {

            Adresse ad1 = new Adresse(null, "TEST", "iuh", "ibkb", "iugaud");
            Adresse ad2 = new Adresse(null, "iazgh", "iuazh", "ibkb", "iugaud");
            Adresse ad3 = new Adresse(null, "igazdh", "idazuh", "azdibkb", "iugaud");


            int id1 = (int)BDDWriter.Insert(ad1);
            int id2 = (int)BDDWriter.Insert(ad2);
            int id3 = (int)BDDWriter.Insert(ad3);

            var o1 = BDDReader.GetObject<Adresse>(id1);
            var o2 = BDDReader.GetObject<Adresse>(id2);


            o1.CodePostal = "UPDATE";

            bool b = BDDWriter.Update(o1);

            int bbb = BDDWriter.Remove<Adresse>(id2);
        }

    }
}
