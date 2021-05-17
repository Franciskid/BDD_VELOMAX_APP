﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class ModeleCommandeViewModel
    {
        public bool IsSelected { get; set; }

        public string Nom { get; set; }

        public LigneProduit Ligne { get; set; }

        public int Prix { get; set; }

        public DateTime Discontinuation { get; set; }

        public ModeleCommandeViewModel(string nom, int prix)
        {
            this.Nom = nom;
            this.Prix = prix;
        }
        public ModeleCommandeViewModel(Modele modele)
        {
            this.Nom = modele.Nom.ToString();
            this.Ligne = modele.Ligne;
            this.Prix = modele.Prix;
            this.Discontinuation = modele.Discontinuation;
        }

        public ModeleCommandeViewModel() { }
    }
}
