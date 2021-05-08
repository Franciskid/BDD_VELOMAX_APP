using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Pieces:ISQL
    {
        public string IdPiece { get; private set; }

        public string Nom { get; private set; }

        public string NomFournisseur { get; private set; }

        public int NumProduit { get; private set; }

        public float Prix { get; private set; }

        public DateTime DateIntroduction { get; private set; }

        public DateTime DateDiscontinuation { get; private set; }

        public DateTime DelaiApprovisionnement { get; private set; }

        public Pieces (string IdPiece, string Nom, string NomFournisseur, int NumProduit, float Prix, DateTime DateIntroduction, DateTime DateDiscontinuation, DateTime DelaiApprovisionnement)
        {
            this.IdPiece = IdPiece;
            this.Nom = Nom;
            this.NomFournisseur = NomFournisseur;
            this.NumProduit = NumProduit;
            this.Prix = Prix;
            this.DateIntroduction = DateIntroduction;
            this.DateDiscontinuation = DateDiscontinuation;
            this.DelaiApprovisionnement = DelaiApprovisionnement;
        }




        public object ID { get; private set; }
        public virtual string IdToString() => "IdPiece";
    }
}
