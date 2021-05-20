using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class Piece : IMySQL
    {
        public string Nom { get; set; }

        public int SiretFournisseur { get; set; }

        public int NumProduit { get; set; }

        public float Prix { get; set; }

        public int Quantité { get; set; }

        public DateTime DateIntroduction { get; set; }

        public DateTime DateDiscontinuation { get; set; }

        public int DelaiApprovisionnementJour { get; set; }

        public Piece (string IdPiece, string Nom, int siretFournisseur, int NumProduit, float Prix, int qté, DateTime DateIntroduction, DateTime DateDiscontinuation, int DelaiApprovisionnementJour)
        {
            this.ID = IdPiece;
            this.Nom = Nom;
            this.SiretFournisseur = siretFournisseur;
            this.NumProduit = NumProduit;
            this.Prix = Prix;
            this.Quantité = qté;
            this.DateIntroduction = DateIntroduction;
            this.DateDiscontinuation = DateDiscontinuation;
            this.DelaiApprovisionnementJour = DelaiApprovisionnementJour;
        }
        public Piece() { }


        public object ID { get; set; }
        public string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{Nom}','{SiretFournisseur}','{NumProduit}','{Prix}','{Quantité}','{DateIntroduction}','{DateDiscontinuation}','{DelaiApprovisionnementJour}'";
    }
}
