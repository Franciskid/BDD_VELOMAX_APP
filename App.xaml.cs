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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ExecuteSQLScript();

            var a = DataReader.Read<Assemblage>();
            var b = DataReader.Read<Commande>();
            var c = DataReader.Read<Fournisseurs>();
            var d = DataReader.Read<Adresse>();
            var Q = DataReader.Read<Pieces>();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
