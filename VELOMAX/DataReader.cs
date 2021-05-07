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
        /// <summary>
        /// Vérifie si le serveur MySQL est lancé et si on arrive à s'y connecter
        /// </summary>
        /// <returns></returns>
        public static bool ServerIsUp()
        {
            try
            {
                return OpenConnexion().State == System.Data.ConnectionState.Open;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Ouvre une connexion avec le serveur MySQL
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection OpenConnexion(bool db = true)
        {
            try
            {
                var mySQLCon = new MySqlConnection(db ? MyConstants.CONNEXION_STRING_DB : MyConstants.CONNEXION_STRING);
                mySQLCon.Open();
                return mySQLCon;
            }
            catch (Exception e)
            {
                return null;
            }
        }



        /// <summary>
        /// Lit une table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Read<T>() where T : ISQL => Read<T>($"SELECT * FROM {MyConstants.TypeToTable(typeof(T))}");

        /// <summary>
        /// Renvoie le premier objet qui satisfait la condition liée à la propriété et à l'objet indiqués. Si aucune propriété n'est indiquée, on comparera l'objet à l'id de la table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="nomPropriété"></param>
        /// <returns></returns>
        public static T GetObject<T>(object id, string nomPropriété = null) where T : ISQL =>
            Read<T>($"SELECT * FROM {MyConstants.TypeToTable(typeof(T))} WHERE {nomPropriété ?? MyConstants.TypeToID(typeof(T))} = '{id}'").FirstOrDefault();

        /// <summary>
        /// Lit une table de donnée en entier en fonction d'une condition. 
        /// Les objets sélectionnés doivent forcément correspondre en type et en ordre à la classe <see cref="ISQL"/> indiquée.
        /// </summary>
        /// <typeparam name="T">Type <see cref="ISQL"/></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<T> Read<T>(string query) where T : ISQL
        {
            MySqlConnection c = null;
           //try
            {
                c = OpenConnexion();

                if (c == null)
                {
                    return null;
                }

                MySqlCommand command = new MySqlCommand(query, c);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<T> l = new List<T>();

                    while (reader.Read())
                    {
                        if (typeof(T) == typeof(Adresse))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_ADRESSE];
                            l.Add((T)(ISQL)new Adresse((int)reader[val[0]], (string)reader[val[1]], (string)reader[val[2]], (string)reader[val[3]], (string)reader[val[4]]));
                        }
                        if (typeof(T) == typeof(Fidelio))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_FIDELIO];
                            l.Add((T)(ISQL)new Fidelio((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (Single)reader[val[3]], (Single)reader[val[4]]));
                        }
                        if (typeof(T) == typeof(ClientIndividuel))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_CLIENTS];
                            l.Add((T)(ISQL)new Fidelio((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (Single)reader[val[3]], (Single)reader[val[4]]));
                        }
                        if (typeof(T) == typeof(ClientBoutique))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_CLIENTS];
                            l.Add((T)(ISQL)new Fidelio((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (Single)reader[val[3]], (Single)reader[val[4]]));
                        }
                        if (typeof(T) == typeof(Modele))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_MODELES];
                            l.Add((T)(ISQL)new Modele((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (string)reader[val[3]], reader.GetDateTime(val[4]), reader.GetDateTime(val[5])));
                        }
                        if (typeof(T) == typeof(Compte))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_COMPTES];
                            l.Add((T)(ISQL)new Compte((int)reader[val[0]], (string)reader[val[1]], (string)reader[val[2]]));
                        }
                        if (typeof(T) == typeof(Assemblage))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_ASSEMBLAGES];
                            l.Add((T)(ISQL)new Assemblage((int)reader[val[0]], reader.GetStringSafe(1), reader.GetStringSafe(2), reader.GetStringSafe(3), reader.GetStringSafe(4), reader.GetStringSafe(5), reader.GetStringSafe(6), reader.GetStringSafe(7), reader.GetStringSafe(8), reader.GetStringSafe(9), reader.GetStringSafe(10), reader.GetStringSafe(11), reader.GetStringSafe(12), reader.GetStringSafe(12), reader.GetStringSafe(13)));
                        }
                        if (typeof(T) == typeof(Commande))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_COMMANDES];
                            l.Add((T)(ISQL)new Commande((int)reader[val[0]], reader.GetDateTime(val[1]), reader.GetDateTime(val[2])));
                        }
                        if (typeof(T) == typeof(Fournisseurs))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_FOURNISSEURS];
                            l.Add((T)(ISQL)new Fournisseurs((int)reader[val[0]], (string)reader[val[1]], (string)reader[val[2]], (string)reader[val[3]], (int)reader[val[4]]));
                        }
                        if (typeof(T) == typeof(Pieces))
                        {
                            var val = MyConstants.DICOVALUES[MyConstants.TABLE_PIECES];
                            l.Add((T)(ISQL)new Pieces(reader.GetStringSafe(0), reader.GetStringSafe(1), reader.GetStringSafe(2), (int)reader[val[2]], (float)reader[val[3]], reader.GetDateTime(val[4]), reader.GetDateTime(val[5]), reader.GetDateTime(val[6])));
                        }
                    }

                    return l;
                }
            }
            ///catch (Exception ex)
            {
                return null;
            }
            //finally
            {
                c?.Close();
            }
        }



        /// <summary>
        /// Execute une recherche et met les objets recherchés dans une liste
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<List<object>> ReadQuery(string query)
        {
            MySqlConnection c = null;
            try
            {
                c = OpenConnexion();

                if (c == null)
                {
                    return null;
                }

                MySqlCommand command = new MySqlCommand(query, c);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<List<object>> l = new List<List<object>>();
                    for (int x = 0; reader.Read(); x++)
                    {
                        l.Add(new List<object>());

                        for (int i = 0; i < reader.FieldCount; i++)
                            l[x].Add(reader.GetValue(i));
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

    }

    public static partial class MyHelper
    {
        public static string GetStringSafe(this MySqlDataReader @this, int column)
        {
            return !@this.IsDBNull(column) ? @this.GetString(column) : null;
        }
    }
}
