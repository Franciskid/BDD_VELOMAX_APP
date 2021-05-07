using BDD_VELOMAX_APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Assemblage: ISQL
    {
        public int idAssemblage { get; private set; }
        public string Nom { get; private set; }

        public Grandeurs Grandeurs{ get; private set; }

        public Cadre Cadre { get; private set; }

        public Guidon Guidon { get; private set; }

        public Freins Freins { get; private set; }

        public Selles Selles { get; private set; }

        public Derailleur_avant derailleur_Avant { get; private set; }

        public Derailleur_arriere derailleur_Arriere { get; private set; }

        public Roue_avant roue_Avant { get; private set; }

        public Roue_arriere roue_Arriere { get; private set; }

        public Reflecteurs Reflecteurs { get; private set; }

        public Pedalier Pedalier { get; private set; }

        public Ordinateur Ordinateur { get; private set; }

        public Panier Panier { get; private set; }


        public Assemblage(int idAssemblage, string Nom, Grandeurs Grandeurs, Cadre Cadre, Guidon Guidon, Freins Freins, Selles Selles, Derailleur_avant derailleur_Avant,
            Derailleur_arriere derailleur_Arriere, Roue_avant roue_Avant, Roue_arriere roue_Arriere, Reflecteurs Reflecteurs, Pedalier Pedalier, Ordinateur Ordinateur, Panier Panier)
        {
            this.idAssemblage = idAssemblage;
            this.Nom = Nom;
            this.Grandeurs = Grandeurs;
            this.Cadre = Cadre;
            this.Guidon = Guidon;
            this.Freins = Freins;
            this.Selles = Selles;
            this.derailleur_Avant = derailleur_Avant;
            this.derailleur_Arriere = derailleur_Arriere;
            this.roue_Avant = roue_Avant;
            this.roue_Arriere = roue_Arriere;
            this.Reflecteurs = Reflecteurs;
            this.Pedalier = Pedalier;
            this.Ordinateur = Ordinateur;
            this.Panier = Panier;
        }

        public object ID { get; private set; }
        public virtual string IdToString() => "idAssemblage";
    }
    
}
