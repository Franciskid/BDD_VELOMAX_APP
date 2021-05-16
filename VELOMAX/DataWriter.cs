﻿using MySql.Data.MySqlClient;
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
        /// <param name="dbCreated"></param>
        public static int ExecuteNonQuery(string commandes, bool dbCreated = true)
        {
            try
            {
                using (var c = DataReader.OpenConnexion(dbCreated))
                {
                    using (MySqlCommand command = new MySqlCommand(commandes, c))
                    {
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                return -1;
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

            if (ExecuteNonQuery($"INSERT INTO {table}({string.Join(",", MyConstants.DICOVALUES[table].Skip(obj.ID == null ? 1 : 0))}) VALUES({obj.SaveStr()})") > 0)
            {
                var a = DataReader.ReadQuery("SELECT LAST_INSERT_ID();");
                object o = DataReader.ReadQuery("SELECT LAST_INSERT_ID();").FirstOrDefault().FirstOrDefault();

                if (o != null && int.TryParse(o.ToString(), out int res))
                {
                    return res;// + 1; //+1 car la fct renvoie l'id -1 pour une raison inconnue EDIT : ne renvoie plus de +1 pour encore une raison inconnue
                }

                return o;
            }

            return null;
        }

        /// <summary>
        /// Met à jour un objet dans la base de donnée
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
       // public static bool Update(IMySQL obj) => ExecuteNonQuery($"UPDATE {MyConstants.TypeToTable(obj.GetType())} SET {MyConstants.UpdateRowSet(obj)} WHERE {MyConstants.TypeToID(obj.GetType())} = '{obj.ID}';") > 0;

        /// <summary>
        /// Supprime un objet dans la base de donnée
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove(IMySQL obj) => ExecuteNonQuery($"DELETE FROM {MyConstants.TypeToTable(obj.GetType())} WHERE {MyConstants.TypeToID(obj.GetType())} = '{obj.ID}';") > 0;

        /// <summary>
        /// Supprime un ou des objets dans la base de donnée à partir d'une valeur d'une propriété spécifiée (sinon alors on compare à l'id de <typeparamref name="T"/>)
        /// </summary>
        /// <param name="id">Valeur à supprimer</param>
        /// <param name="nomPropriété">Propriété à laquelle comparer l'objet (si null alors on utilise l'id)</param>
        /// <returns></returns>
        public static int Remove<T>(object id, string nomPropriété = null) => ExecuteNonQuery($"DELETE FROM {MyConstants.TypeToTable(typeof(T))} WHERE {nomPropriété ?? MyConstants.TypeToID(typeof(T))} = '{id}';");

    }
}
