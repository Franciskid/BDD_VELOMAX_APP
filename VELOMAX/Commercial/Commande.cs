using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Commande: IMySQL
    {
        public int IdCommande { get; private set; }

        public DateTime DateCommande { get; private set; }

        public DateTime DateLivraison { get; private set; }

        public Commande(int IdCommande, DateTime DateCommande, DateTime DateLivraison)
        {
            this.IdCommande = IdCommande;
            this.DateCommande = DateCommande;
            this.DateLivraison = DateLivraison;
        }
        public object ID { get; private set; }
        public string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{DateCommande}','{DateLivraison}'";
    }
}
