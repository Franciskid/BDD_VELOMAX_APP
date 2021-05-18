using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class PrixCheckoutViewModel
    {
        public string Nom { get; set; }

        public int Quantité { get; set; }

        private float prixIndiv;

        public float Prix
        {
            get => prixIndiv * Quantité;
            set => prixIndiv = value;
        }

        public PrixCheckoutViewModel()
        {

        }
        public PrixCheckoutViewModel(PieceCommandeViewModel p)
        {
            this.Nom = $"{p.Nom} : {p.Id}";
            this.Quantité = 1;
            this.prixIndiv = p.Prix;
        }

        public PrixCheckoutViewModel(ModeleCommandeViewModel m)
        {
            this.Nom = $"{m.Ligne} : {m.Nom}";
            this.Quantité = 1;
            this.prixIndiv = m.Prix;
        }
    }
}
