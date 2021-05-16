using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Commande: IMySQL
    {
        public object ID { get; private set; }

        public int IDCommande { get; private set; }

        public Client Client { get; private set; }

        public Pieces Piece { get; private set; }

        public Assemblage Assemblage { get; private set; }

        public DateTime DateCommande { get; private set; }

        public DateTime DateLivraison { get; private set; }

        public Commande(int? idCommande, int numCommande, int idClient, string idPiece, int idAssemblage, DateTime DateCommande, DateTime DateLivraison)
        {
            this.ID = idCommande;
            this.IDCommande = numCommande;
            this.Client = DataReader.GetObject<Client>(idClient);
            this.Piece = DataReader.GetObject<Pieces>(idPiece);
            this.Assemblage = DataReader.GetObject<Assemblage>(idAssemblage);
            this.DateCommande = DateCommande;
            this.DateLivraison = DateLivraison;
        }

        public string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{DateCommande}','{DateLivraison}'";
    }
}
