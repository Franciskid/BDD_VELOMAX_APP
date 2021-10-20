using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class ModeleCommandeViewModel
    {
        public bool Sélectionné { get; set; }


        public int ID { get; set; }

        public string Nom { get; set; }

        public int Prix { get; set; }

        public LigneProduit Ligne { get; set; }

        public Grandeurs Grandeur { get; set; }

        public DateTime Discontinuation { get; set; }

        public ModeleCommandeViewModel(Modele modele, Grandeurs grandeur)
        {
            this.ID = (int)modele.ID;
            this.Nom = modele.Nom.ToString();
            this.Ligne = modele.Ligne;
            this.Prix = modele.Prix;
            this.Grandeur = grandeur;
            this.Discontinuation = modele.Discontinuation;
        }

        public ModeleCommandeViewModel() { }
    }
}
