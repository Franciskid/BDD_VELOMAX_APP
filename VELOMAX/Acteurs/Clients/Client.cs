using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    abstract class Client : IMySQL
    {
        public Adresse Adresse { get; private set; }

        public string Telephone { get; private set; }

        public string AdresseMail { get; private set; }


        public object ID { get; private set; }


        protected Client(Adresse adresse, string tel, string mail)
        {
            this.Adresse = adresse;
            this.Telephone = tel;
            this.AdresseMail = mail;
        }

        public virtual string SaveStr() => "idClient";
    }
}
