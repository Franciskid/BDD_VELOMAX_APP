using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Pieces:IMySQL
    {
        public string IdPiece { get; private set; }

        public string Nom { get; private set; }

        public int SiretFournisseur { get; private set; }

        public int NumProduit { get; private set; }

        public float Prix { get; private set; }

        public int Quantité { get; private set; }

        public DateTime DateIntroduction { get; private set; }

        public DateTime DateDiscontinuation { get; private set; }

        public DateTime DelaiApprovisionnement { get; private set; }

        public Pieces (string IdPiece, string Nom, int siretFournisseur, int NumProduit, float Prix, int qté, DateTime DateIntroduction, DateTime DateDiscontinuation, DateTime DelaiApprovisionnement)
        {
            this.IdPiece = IdPiece;
            this.Nom = Nom;
            this.SiretFournisseur = siretFournisseur;
            this.NumProduit = NumProduit;
            this.Prix = Prix;
            this.Quantité = qté;
            this.DateIntroduction = DateIntroduction;
            this.DateDiscontinuation = DateDiscontinuation;
            this.DelaiApprovisionnement = DelaiApprovisionnement;
        }




        public object ID { get; private set; }
        public string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{Nom}','{SiretFournisseur}','{NumProduit}','{Prix}','{Quantité}','{DateIntroduction}','{DateDiscontinuation}','{DelaiApprovisionnement}'";
    }
}
