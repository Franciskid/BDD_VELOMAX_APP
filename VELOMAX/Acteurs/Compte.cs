using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class Compte : IMySQL
    {
        public object ID { get; private set; }

        public string Nom { get; private set; }

        public string MDP_SHA1 { get; private set; }


        public Compte(int id, string nom, string motdepasse)
        {
            this.ID = id;
            this.Nom = nom;
            this.MDP_SHA1 = motdepasse;
        }

        public string SaveStr() => "idCompte";
    }

    public static partial class MyHelper
    {
        /// <summary>
        /// Hash le mot de passe selon l'algorithme de chiffrement SHA1.
        /// Requiert un ID unique associé au compte de l'utilisateur pour assurer l'unicité du hash.
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string HashPasswordSHA1(string uniqueID, string pass)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(uniqueID + pass);
                byte[] hashedBytes = sha1.ComputeHash(sourceBytes);

                return BitConverter.ToString(hashedBytes).Replace("-", "");
            }
        }

        /// <summary>
        /// Compare 2 mot de passe
        /// </summary>
        /// <param name="pass">Non haché</param>
        /// <param name="util"></param>
        /// <returns></returns>
        public static bool ComparePassword(string pass, Compte util) => HashPasswordSHA1(util.Nom, pass) == util.MDP_SHA1;


        public virtual string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{pseudo}','{motdepasse}','{unique}'";
    }
}
