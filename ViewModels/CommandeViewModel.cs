using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class CommandeViewModel
    {
        public int IDCommande { get; set; }

        public string Client { get; set; }

        public string PieceCommandée { get; set; }

        public DateTime DateCommande { get; set; }

        public DateTime DateLivraison { get; set; }


        public CommandeViewModel() { }

        public CommandeViewModel(Commande com) 
        {
            this.IDCommande = com.IDCommande;
            this.Client = com.Client.AdresseMail;
            this.PieceCommandée = com.Piece != null ? $"{com.Piece.Nom} : {com.Piece.ID}" : $"{com.Modele.Ligne} : {com.Modele.Nom}";
            this.DateCommande = com.DateCommande;
            this.DateLivraison = com.DateLivraison;
        }

    }
}
