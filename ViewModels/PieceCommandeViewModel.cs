using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class PieceCommandeViewModel
    {
        public bool IsSelected { get; set; }

        public string Nom { get; set; }

        public string Id { get; set; }

        public float Prix { get; set; }

        public int QuantitéStock { get; set; }

        public string Fournisseur { get; set; }

        public int DelaiApprovisionnementJour { get; set; }


        public PieceCommandeViewModel(string nom, int prix)
        {
            this.Nom = nom;
            this.Prix = prix;
        }

        public PieceCommandeViewModel(Piece piece)
        {
            this.Nom = piece.Nom;
            this.Prix = piece.Prix;
            this.Id = (string)piece.ID;
            this.QuantitéStock = piece.Quantité;
            this.Fournisseur = BDDReader.GetObject<Fournisseurs>(piece.SiretFournisseur).Nom;
            this.DelaiApprovisionnementJour = piece.DelaiApprovisionnementJour;
        }

        public PieceCommandeViewModel() { }
    }
}
