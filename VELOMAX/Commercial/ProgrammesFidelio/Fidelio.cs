using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Fidelio : IFidelio, ISQL
    {
        public float Prix { get; }

        public float Rabais { get; }

        public float Duree_annee { get; }

        public string Description { get; }

        public object ID { get; private set; }

        public Fidelio(int id, string description, float prix, float rabais, float duree)
        {
            this.ID = id;
            this.Prix = prix;
            this.Rabais = rabais;
            this.Duree_annee = duree;
            this.Description = description;
        }
    }
}
