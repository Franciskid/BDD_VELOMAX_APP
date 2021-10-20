﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Classe static pour lire des informations depuis la BDD (accessible à bozo/root).
    /// </summary>
    static class BDDReader
    {
        /// <summary>
        /// Vérifie si le serveur MySQL est lancé et si on arrive à s'y connecter
        /// </summary>
        /// <returns></returns>
        public static bool ServerIsUp()
        {
            try
            {
                using (var c = OpenConnexion()) return c?.State == System.Data.ConnectionState.Open;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Ouvre une connexion avec le serveur MySQL
        /// </summary>
        /// <param name="db">Se connecte à la base de donnée 'velomax' ou juste au serveur</param>
        /// <returns></returns>
        public static MySqlConnection OpenConnexion(bool db = true)
        {
            try //Pas de using ici évidemment
            {
                var mySQLCon = new MySqlConnection(ConfigurationManager.AppSettings.Get(db ? App.Admin ? "ServeurConnexionDBRoot" : "ServeurConnexionDBBozo" : App.Admin ? "ServeurConnexionRoot" : "ServeurConnexionBozo"));
                mySQLCon.Open();

                if (mySQLCon.State != System.Data.ConnectionState.Open)
                {
                    mySQLCon.Dispose();
                    return null;
                }
                return mySQLCon;
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        /// Renvoie le premier objet qui satisfait la valeur liée à la propriété et à l'objet indiqués. Si aucune propriété n'est indiquée, on comparera l'objet à l'id de la table <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="nomPropriété"></param>
        /// <returns></returns>
        public static T Get<T>(object val, string nomPropriété = null) where T : IMySQL => Read<T>($"SELECT * FROM {BDDConstants.TypeToTable(typeof(T))} WHERE {nomPropriété ?? BDDConstants.TypeToID(typeof(T))} = '{val}';").FirstOrDefault();


        /// <summary>
        /// Lit la table <typeparamref name="T"/> dans la BDD velomax.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Read<T>() where T : IMySQL => Read<T>($"SELECT * FROM {BDDConstants.TypeToTable(typeof(T))};");

        /// <summary>
        /// Lit la table <typeparamref name="T"/> dans la BDD velomax.
        /// Les objets sélectionnés doivent forcément correspondre en type et en ordre à la classe <typeparamref name="T"/> indiquée.
        /// </summary>
        /// <typeparam name="T">Type <see cref="IMySQL"/></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<T> Read<T>(string query) where T : IMySQL
        {
            try
            {
                using (MySqlConnection c = OpenConnexion())
                using (MySqlCommand command = new MySqlCommand(query, c))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<T> l = new List<T>();

                    var val = BDDConstants.DICOTABLEVALUES[BDDConstants.TypeToTable(typeof(T))];
                    while (reader.Read())
                    {
                        if (typeof(T) == typeof(Adresse))
                        {
                            l.Add((T)(IMySQL)new Adresse((int)reader[val[0]], (string)reader[val[1]], (string)reader[val[2]], (string)reader[val[3]], (string)reader[val[4]]));
                        }
                        else if (typeof(T) == typeof(Fidelio))
                        {
                            l.Add((T)(IMySQL)new Fidelio((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (Single)reader[val[4]], (Single)reader[val[3]]));
                        }
                        else if ((typeof(T) == typeof(ClientIndividuel) || typeof(T) == typeof(Client)) && (string)reader[val[1]] == "individuel")
                        {
                            l.Add((T)(IMySQL)new ClientIndividuel((int)reader[val[0]], reader.GetStringSafe(2), reader.GetStringSafe(3), (int)reader[val[4]], reader.GetStringSafe(5), reader.GetStringSafe(6), (bool)reader[val[9]] ? (int?)reader[val[10]] : null, reader.GetDateTimeSafe(11)));
                        }
                        else if ((typeof(T) == typeof(ClientBoutique) || typeof(T) == typeof(Client)) && (string)reader[val[1]] == "boutique")
                        {
                            l.Add((T)(IMySQL)new ClientBoutique((int)reader[val[0]], reader.GetStringSafe(2), (int)reader[val[4]], reader.GetStringSafe(5), reader.GetStringSafe(6), reader.GetStringSafe(7), (int)reader.GetIntSafe(8)));
                        }
                        else if (typeof(T) == typeof(Modele))
                        {
                            l.Add((T)(IMySQL)new Modele((int)reader[val[0]], (string)reader[val[1]], (int)reader[val[2]], (string)reader[val[3]], reader.GetDateTime(val[4]), reader.GetDateTime(val[5])));
                        }
                        else if (typeof(T) == typeof(Compte))
                        {
                            l.Add((T)(IMySQL)new Compte((int)reader[val[0]], (string)reader[val[1]], (string)reader[val[2]]));
                        }
                        else if (typeof(T) == typeof(Assemblage))
                        {
                            l.Add((T)(IMySQL)new Assemblage((int)reader[val[0]], reader.GetStringSafe(1), reader.GetStringSafe(2), reader.GetStringSafe(3), reader.GetStringSafe(4), reader.GetStringSafe(5), reader.GetStringSafe(6), reader.GetStringSafe(7), reader.GetStringSafe(8), reader.GetStringSafe(9), reader.GetStringSafe(10), reader.GetStringSafe(11), reader.GetStringSafe(12), reader.GetStringSafe(12), reader.GetStringSafe(13)));
                        }
                        else if (typeof(T) == typeof(Commande))
                        {
                            l.Add((T)(IMySQL)new Commande((int)reader[val[0]], (int)reader[val[1]], (int)reader[val[2]], reader.GetStringSafe(3), reader.GetIntSafe(4), (int)reader.GetIntSafe(5), reader.GetDateTimeSafe(6), reader.GetDateTimeSafe(7)));
                        }
                        else if (typeof(T) == typeof(Fournisseurs))
                        {
                            l.Add((T)(IMySQL)new Fournisseurs((int)reader[val[0]], (string)reader[val[1]], (string)reader[val[2]], (int)reader[val[3]], int.Parse((string)reader[val[4]]), int.Parse((string)reader[val[5]])));
                        }
                        else if (typeof(T) == typeof(Piece))
                        {
                            l.Add((T)(IMySQL)new Piece(reader.GetStringSafe(0), reader.GetStringSafe(1), (int)reader[val[2]], (int)reader[val[3]], (Single)reader[val[4]], (int)reader[val[5]], reader.GetDateTimeSafe(6), reader.GetDateTimeSafe(7), (int)reader.GetIntSafe(8)));
                        }
                    }

                    return l;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Execute une recherche et met les objets recherchés dans une liste
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<List<object>> ReadQuery(string query)
        {
            try
            {
                using (MySqlConnection c = OpenConnexion())
                using (MySqlCommand command = new MySqlCommand(query, c))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<List<object>> l = new List<List<object>>();
                    for (int x = 0; reader.Read(); x++)
                    {
                        l.Add(new List<object>(reader.FieldCount));

                        for (int i = 0; i < reader.FieldCount; i++)
                            l[x].Add(reader.GetValue(i));
                    }

                    return l;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

    public static partial class MyHelper
    {
        /// <summary>
        /// Vérifie si le string recherché est bien non null et en renvoie la valeur de manière sécurisé.
        /// </summary>
        public static string GetStringSafe(this MySqlDataReader @this, int column) => !@this.IsDBNull(column) ? @this.GetString(column) : null;
        
        /// <summary>
        /// Vérifie si le int recherché est bien non null et en renvoie la valeur de manière sécurisé.
        /// </summary>
        public static int? GetIntSafe(this MySqlDataReader @this, int column) => !@this.IsDBNull(column) ? (int?)@this.GetInt32(column) : null;

        /// <summary>
        /// Vérifie si le datetime recherché est bien non null et en renvoie la valeur de manière sécurisé.
        /// </summary>
        public static DateTime GetDateTimeSafe(this MySqlDataReader @this, int column) => !@this.IsDBNull(column) ? @this.GetDateTime(column) : default;
    }
}
