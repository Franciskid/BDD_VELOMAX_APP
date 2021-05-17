using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Classe static pour écrire des données dans la BDD (accessible à root uniquement).
    /// </summary>
    static class BDDWriter
    {
        /// <summary>
        /// Execute une déclaration sur la base de donnée et renvoie (nombre de lignes affectées, dernier id ajouté)
        /// </summary>
        /// <param name="commandes"></param>
        /// <param name="dbCreated"></param>
        public static (int, long) ExecuteNonQuery(string commandes, bool dbCreated = true)
        {
            try
            {
                using (var con = BDDReader.OpenConnexion(dbCreated))
                using (var com = new MySqlCommand(commandes, con))
                {
                    var a = (com.ExecuteNonQuery(), com.LastInsertedId);
                    return a;
                }
            }
            catch (Exception)
            {
                return (-1, -1);
            }
        }



        /// <summary>
        /// Ajoute un objet <see cref="IMySQL"/> à la base de donnée et renvoie l'ID inséré.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long Insert(IMySQL obj)
        {
            string table = MyConstants.TypeToTable(obj.GetType());
            return ExecuteNonQuery($"INSERT INTO {table}({string.Join(",", MyConstants.DICOVALUES[table].Skip(obj.ID == null ? 1 : 0))}) VALUES({obj.SaveStr()})").Item2;
        }

        /// <summary>
        /// Met à jour un objet dans la base de donnée
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Update(IMySQL obj) => ExecuteNonQuery($"UPDATE {MyConstants.TypeToTable(obj.GetType())} SET {MyConstants.UpdateRowSet(obj)} WHERE {MyConstants.TypeToID(obj.GetType())} = '{obj.ID}';").Item1 > 0;

        /// <summary>
        /// Supprime un objet dans la base de donnée
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove(IMySQL obj) => ExecuteNonQuery($"DELETE FROM {MyConstants.TypeToTable(obj.GetType())} WHERE {MyConstants.TypeToID(obj.GetType())} = '{obj.ID}';").Item1 > 0;

        /// <summary>
        /// Supprime un ou des objets dans la base de donnée à partir d'une valeur d'une propriété spécifiée (sinon alors on compare à l'id de <typeparamref name="T"/>)
        /// </summary>
        /// <param name="id">Valeur à supprimer</param>
        /// <param name="nomPropriété">Propriété à laquelle comparer l'objet (si null alors on utilise l'id)</param>
        /// <returns></returns>
        public static int Remove<T>(object id, string nomPropriété = null) => ExecuteNonQuery($"DELETE FROM {MyConstants.TypeToTable(typeof(T))} WHERE {nomPropriété ?? MyConstants.TypeToID(typeof(T))} = '{id}';").Item1;

    }
}
