using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Compte : ISQL
    {
        public object ID { get; private set; }

        public string Nom { get; private set; }

        public string MDP_SHAH1 { get; private set; }


        public Compte(int id, string nom, string motdepasse)
        {
            this.ID = id;
            this.Nom = nom;
            this.MDP_SHAH1 = motdepasse;
        }

        public string IdToString() => "idCompte";
    }
}
