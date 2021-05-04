using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class DataReader
    {
        public static bool ServerIsUp()
        {
            try
            {
                var mySQLCon = new MySqlConnection(MyConstants.CONNEXION_STRING);
                mySQLCon.Open();
                return mySQLCon.State == System.Data.ConnectionState.Open;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
