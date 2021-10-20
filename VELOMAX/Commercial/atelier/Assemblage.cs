using BDD_VELOMAX_APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class Assemblage : IMySQL
    {
        public object ID { get; private set; }

        public NomModeles Nom { get; private set; }

        public Grandeurs Grandeurs { get; private set; }

        public Cadre Cadre { get; private set; }

        public Guidon Guidon { get; private set; }

        public Freins Freins { get; private set; }

        public Selles Selles { get; private set; }

        public Derailleur_avant Derailleur_Avant { get; private set; }

        public Derailleur_arriere Derailleur_Arriere { get; private set; }

        public Roue_avant Roue_Avant { get; private set; }

        public Roue_arriere Roue_Arriere { get; private set; }

        public Reflecteurs Reflecteurs { get; private set; }

        public Pedalier Pedalier { get; private set; }

        public Ordinateur Ordinateur { get; private set; }

        public Panier Panier { get; private set; }


        public Assemblage(object idAssemblage, string Nom, string Grandeurs, string Cadre, string Guidon, string Freins, string Selles, string derailleur_Avant,
            string derailleur_Arriere, string roue_Avant, string roue_Arriere, string Reflecteurs, string Pedalier, string Ordinateur, string Panier)
        {
            this.ID = idAssemblage;
            this.Nom = MyHelper.StringToEnum<NomModeles>(Nom?.Replace(' ', '_'));
            this.Grandeurs = MyHelper.StringToEnum<Grandeurs>(Grandeurs?.Replace(' ', '_'));
            this.Cadre = MyHelper.StringToEnum<Cadre>(Cadre);
            this.Guidon = MyHelper.StringToEnum<Guidon>(Guidon);
            this.Freins = MyHelper.StringToEnum<Freins>(Freins);
            this.Selles = MyHelper.StringToEnum<Selles>(Selles);
            this.Derailleur_Avant = MyHelper.StringToEnum<Derailleur_avant>(derailleur_Avant?.Replace(' ', '_'));
            this.Derailleur_Arriere = MyHelper.StringToEnum<Derailleur_arriere>(derailleur_Arriere?.Replace(' ', '_'));
            this.Roue_Avant = MyHelper.StringToEnum<Roue_avant>(roue_Avant?.Replace(' ', '_'));
            this.Roue_Arriere = MyHelper.StringToEnum<Roue_arriere>(roue_Arriere?.Replace(' ', '_'));
            this.Reflecteurs = MyHelper.StringToEnum<Reflecteurs>(Reflecteurs);
            this.Pedalier = MyHelper.StringToEnum<Pedalier>(Pedalier);
            this.Ordinateur = MyHelper.StringToEnum<Ordinateur>(Ordinateur);
            this.Panier = MyHelper.StringToEnum<Panier>(Panier);
        }

        public virtual string SaveStr() => (ID != null ? $"'{ID}', " : "") +$"'{Nom}', '{Grandeurs}', '{Cadre}', '{Guidon}','{Freins}', '{Selles}', '{Derailleur_Avant}', '{Derailleur_Arriere}','{Roue_Avant}', '{Roue_Arriere}', '{Reflecteurs}', '{Pedalier}','{Ordinateur}', '{Panier}'";
    }

}
