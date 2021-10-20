using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
                    return (com.ExecuteNonQuery(), com.LastInsertedId);
                }
            }
            catch (Exception)
            {
                return (-1, -1);
            }
        }

        /// <summary>
        /// Execute les fichiers triggers
        /// </summary>
        /// <returns></returns>
        public static int ExecuteScript(string statement)
        {
            try
            {
                using (var con = BDDReader.OpenConnexion(true))
                {
                    var com = new MySqlScript(con, statement) //Obligé d'utiliser un script pour que les delimiter soient reconnus
                    {
                        Delimiter = "$$"
                    };

                    return com.Execute();
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }


        /// <summary>
        /// Ajoute un objet <see cref="IMySQL"/> à la base de donnée et renvoie l'ID inséré.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long Insert(IMySQL obj) => ExecuteNonQuery($"INSERT INTO {BDDConstants.TypeToTable(obj.GetType())}({string.Join(",", BDDConstants.DICOTABLEVALUES[BDDConstants.TypeToTable(obj.GetType())].Skip(obj.ID == null ? 1 : 0))}) VALUES({obj.SaveStr()})").Item2;

        /// <summary>
        /// Met à jour un objet dans la base de donnée. Si l'id n'est pas spécifié on mettra à jour avec l'id de l'objet
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="oldId"></param>
        /// <returns></returns>
        public static bool Update(IMySQL obj, object oldId = null) => ExecuteNonQuery($"UPDATE {BDDConstants.TypeToTable(obj.GetType())} SET {string.Join(",", BDDConstants.DICOTABLEVALUES[BDDConstants.TypeToTable(obj.GetType())].Select((x, y) => $"{x} = {obj.SaveStr().Split(',')[y++]}"))} WHERE {BDDConstants.TypeToID(obj.GetType())} = '{oldId ?? obj.ID}';").Item1 > 0;

        /// <summary>
        /// Supprime un objet dans la base de donnée
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Delete(IMySQL obj) => (int)typeof(BDDWriter).GetMethod(nameof(BDDWriter.Delete), new Type[] { typeof(object), typeof(string) }).MakeGenericMethod(obj.GetType()).Invoke(null, new object[] { obj.ID, null }) >= 1;// ExecuteNonQuery($"DELETE FROM {BDDConstants.TypeToTable(obj.GetType())} WHERE {BDDConstants.TypeToID(obj.GetType())} = '{obj.ID}';").Item1 > 0;

        /// <summary>
        /// Supprime un ou des objets dans la base de donnée à partir d'une valeur d'une propriété spécifiée (si la propriété n'est pas spcéifiée on comparera à l'id de <typeparamref name="T"/>)
        /// </summary>
        /// <param name="id">Valeur à supprimer</param>
        /// <param name="nomPropriété">Propriété à laquelle comparer l'objet (si null alors on utilise l'id)</param>
        /// <returns></returns>
        public static int Delete<T>(object id, string nomPropriété = null) where T : IMySQL => ExecuteNonQuery($"DELETE FROM {BDDConstants.TypeToTable(typeof(T))} WHERE {nomPropriété ?? BDDConstants.TypeToID(typeof(T))} = '{id}';").Item1;

    }
}
