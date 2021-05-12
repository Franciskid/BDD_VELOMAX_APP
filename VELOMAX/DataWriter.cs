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
        /// <summary>
        /// Execute une déclaration sur la base de donnée
        /// </summary>
        /// <param name="commandes"></param>
        public static void ExecuteNonQuery(string commandes)
        {
            try
            {
                using (var maConnexion = DataReader.OpenConnexion(false))
                {
                    var cmd = maConnexion.CreateCommand();
                    cmd.CommandText = commandes;
                    cmd.ExecuteNonQuery();
                    maConnexion.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
            }

        }


        /// <summary>
        /// Ajoute un objet <see cref="IMySQL"/> à la base de donnée
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Insert(IMySQL obj)
        {
            //try
            {
                using (MySqlConnection c = DataReader.OpenConnexion())
                {
                    if (c == null)
                    {
                        return false;
                    }

                    string table = MyConstants.TypeToTable(obj.GetType());

                    MySqlCommand command = new MySqlCommand($"INSERT INTO {table}({string.Join(",", MyConstants.DICOVALUES[table].Skip(obj.ID == null ? 1 : 0))}) VALUES({obj.SaveStr()})", c);

                    return command.ExecuteNonQuery() > 0;
                }

            }
            //catch (Exception e)
            //{
            //    return false;
            //}
        }
    }
}
