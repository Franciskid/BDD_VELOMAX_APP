using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    static class DataWriter
    {
        public static void ExecuteNonQuery(string commandes)
        {
            try
            {
                var maConnexion = OpenConnexion();
                var cmd = maConnexion.CreateCommand();
                cmd.CommandText = commandes;
                cmd.ExecuteNonQuery();
                maConnexion.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
            }

        }

        public static MySqlConnection OpenConnexion()
        {
            MySqlConnection maConnexion = null;
            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=velomax;" +
                                         "UID=root;PASSWORD='root'";

                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                return null;
            }

            return maConnexion;
        }
    }
}
