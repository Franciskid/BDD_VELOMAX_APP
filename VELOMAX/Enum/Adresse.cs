using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Représente une adresse physique d'une personne.
    /// </summary>
    public class Adresse : IMySQL
    {
        /// <summary>
        /// Numéro
        /// </summary>
        public int Numéro { get; }

        /// <summary>
        /// Rue
        /// </summary>
        public string Rue { get; }

        /// <summary>
        /// Code postal
        /// </summary>
        public int CodePostal { get; }

        /// <summary>
        /// Ville
        /// </summary>
        public string Ville { get; }

        /// <summary>
        /// Province
        /// </summary>
        public string Province { get; }


        /// <summary>
        /// Initialisation d'une adresse à partir des paramètres
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="rue"></param>
        /// <param name="codepostal"></param>
        /// <param name="ville"></param>
        /// <param name="province"></param>
        public Adresse(int? id, string rue, string ville, string codepostal, string province)
        {
            this.ID = id;
            this.Rue = rue;
            this.CodePostal = int.Parse(codepostal);
            this.Ville = ville;
            this.Province = province;
        }


        public override string ToString()
        {
            return this.Numéro + " " + this.Rue + " " + CodePostal + " " + Ville;
        }

        public object ID { get; private set; }

        public virtual string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{Rue}', '{CodePostal}', '{Ville}', '{Province}'";
    }
}
