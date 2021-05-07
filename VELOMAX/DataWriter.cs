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
                var maConnexion = DataReader.OpenConnexion(false);
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

    }
}
