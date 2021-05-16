﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Fournisseurs : IMySQL
    {
        public string Nom { get; private set; }

        public string Contact { get; private set; }

        public Adresse Adresse { get; private set; }

        public Score Score { get; private set; }

        public Fournisseurs(int Siret, string Nom, string Contact, int iDadresse, int Scores)
        {
            this.ID = Siret;
            this.Nom = Nom;
            this.Contact = Contact;
            this.Adresse = BDDReader.GetObject<Adresse>(iDadresse);
            this.Score = (Score)Scores;
        }
        public object ID { get; }
        public string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{Nom}','{Contact}' , '{Adresse.ID}', '{Score}'";
    }
}
