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
        public static object Insert(IMySQL obj)
        {
            string table = MyConstants.TypeToTable(obj.GetType());

            if (InternalUpdate($"INSERT INTO {table}({string.Join(",", MyConstants.DICOVALUES[table].Skip(obj.ID == null ? 1 : 0))}) VALUES({obj.SaveStr()})"))
            {
                object o = DataReader.ReadQuery("SELECT LAST_INSERT_ID();").FirstOrDefault().FirstOrDefault();

                if (o != null && int.TryParse(o.ToString(), out int res))
                {
                    return res;// + 1; //+1 car la fct renvoie l'id -1 pour une raison inconnue EDIT : ne renvoie plus de +1 pour encore une raison inconnue
                }

                return o;
            }

            return null;
        }



        public static bool Update(IMySQL obj) => InternalUpdate($"UPDATE {MyConstants.TypeToTable(obj.GetType())} SET {MyConstants.UpdateRowSet(obj)} WHERE {MyConstants.TypeToID(obj.GetType())} = '{obj.ID}';");

        public static bool Remove(IMySQL obj) => InternalUpdate($"DELETE FROM {MyConstants.TypeToTable(obj.GetType())} WHERE {MyConstants.TypeToID(obj.GetType())} = '{obj.ID}';");

        public static bool Remove<T>(object id, string nomPropriété = null) => InternalUpdate($"DELETE FROM {MyConstants.TypeToID(typeof(T))} where {nomPropriété ?? MyConstants.TypeToID(typeof(T))} = '{id}';");


        private static bool InternalUpdate(string query)
        {
            try
            {
                using (MySqlConnection c = DataReader.OpenConnexion())
                {
                    if (c == null)
                    {
                        return false;
                    }

                    using (MySqlCommand command = new MySqlCommand(query, c))
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
