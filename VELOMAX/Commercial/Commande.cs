using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class Commande: IMySQL
    {
        public object ID { get; private set; }

        public int IDCommande { get; private set; }

        public Client Client { get; private set; }

        public Piece Piece { get; private set; }

        public Modele Modele { get; private set; }

        public int Quantité { get; private set; }

        public DateTime DateCommande { get; private set; }

        public DateTime DateLivraison { get; private set; }

        public Commande(int? idCommande, int numCommande, int idClient, string idPiece, int? idModele, int quantité, DateTime DateCommande, DateTime DateLivraison)
        {
            this.ID = idCommande;
            this.IDCommande = numCommande;
            this.Client = BDDReader.GetObject<Client>(idClient);
            this.Piece = idPiece != null ? BDDReader.GetObject<Piece>(idPiece) : null;
            this.Modele = idModele != null ? BDDReader.GetObject<Modele>(idModele) : null;
            this.Quantité = quantité;
            this.DateCommande = DateCommande;
            this.DateLivraison = DateLivraison;
        }

        public string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{IDCommande}','{Client.ID}',{(Piece != null ? $"'{Piece?.ID}'" : "null")},{(Modele != null ? $"'{Modele?.ID}'" : "null")},'{Quantité}','{DateCommande:yyyy-MM-dd}','{DateLivraison:yyyy-MM-dd}'";
    }
}
