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
                return OpenConnexion().State == System.Data.ConnectionState.Open;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public static MySqlConnection OpenConnexion()
        {
            try
            {
                var mySQLCon = new MySqlConnection(MyConstants.CONNEXION_STRING);
                mySQLCon.Open();
                return mySQLCon;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<ISQL> Read<T>() where T : ISQL
        {
            MySqlConnection c = null;
            try
            {
                c = OpenConnexion();

                if (c == null)
                {
                    return null;
                }

                MySqlCommand command = new MySqlCommand($"SELECT * FROM {MyConstants.TypeToTable(typeof(T))}", c);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<ISQL> l = new List<ISQL>();
                    while (reader.Read())
                    {
                        if (typeof(T) == typeof(Fidelio))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.FIDELIO];
                            l.Add(new Fidelio((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (Single)reader[val[3]], (Single)reader[val[4]]));
                        }
                        if (typeof(T) == typeof(Client))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.CLIENTS];
                            l.Add(new Fidelio((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (Single)reader[val[3]], (Single)reader[val[4]]));
                        }
                    }

                    return l;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                c?.Close();
            }
        }

        public static T GetObject<T>(object id) where T : ISQL => (T)Read<T>().FirstOrDefault(x => x.ID.Equals(id));
    }
}
