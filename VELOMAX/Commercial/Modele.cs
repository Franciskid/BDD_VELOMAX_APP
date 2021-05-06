using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Modele : ISQL
    {
        /// <summary>
        /// int
        /// </summary>
        public object ID { get; }

        public string Nom { get; }

        public int Prix { get; }

        public LigneProduit Ligne { get; }

        public DateTime Introduction { get; }

        public DateTime Discontinuation { get; }


        public Modele(int id, string nom, int prix, string ligne, DateTime intro, DateTime discont)
        {
            this.ID = id;
            this.Nom = nom;
            this.Prix = prix;
            this.Ligne = MyHelper.DescriptionToLigneProduit(ligne);
            this.Introduction = intro;
            this.Discontinuation = discont;
        }

        public string IdToString() => "idModele";
    }
}
