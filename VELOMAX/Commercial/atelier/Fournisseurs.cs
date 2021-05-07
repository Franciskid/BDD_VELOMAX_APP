using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class Fournisseurs:ISQL
    {
        public int Siret { get; private set; }
        
        public string Nom { get; private set; }

        public string Contact { get; private set; }

        public Adresse Iadresse { get; private set; }

        public Score Score { get; private set; }
        
        public Fournisseurs (int Siret, string Nom, string Contact,String Iadresse,int Scores)
        {
            this.Siret = Siret;
            this.Nom = Nom;
            this.Contact = Contact;
            this.Iadresse = DataReader.GetObject<Adresse>(Iadresse);
            this.Score= (Score)Scores;
        }
        public object ID { get; private set; }
        public virtual string IdToString() => "Siret";
    }
}
