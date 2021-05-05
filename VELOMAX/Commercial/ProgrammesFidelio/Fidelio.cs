using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Fidelio : IFidelio
    {
        public float Prix { get; }

        public float Rabais { get; }

        public float Duree_annee { get; }

        public string Description { get; }


        public Fidelio(float prix, float rabais, float duree, string description)
        {
            this.Prix = prix;
            this.Rabais = rabais;
            this.Duree_annee = duree;
            this.Description = description;
        }
    }
}
