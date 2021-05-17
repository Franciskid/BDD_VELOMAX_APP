﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public abstract class Client : IMySQL
    {
        public Adresse Adresse { get; protected set; }

        public string Telephone { get; protected set; }

        public string AdresseMail { get; protected set; }


        public object ID { get; set; }


        protected Client(object ID, int idAdresse, string tel, string mail)
        {
            this.ID = ID;
            this.Adresse = BDDReader.GetObject<Adresse>(idAdresse);
            this.Telephone = tel;
            this.AdresseMail = mail;
        }

        public virtual string SaveStr() => "idClient";
    }
}
