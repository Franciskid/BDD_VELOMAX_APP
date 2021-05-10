using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class ClientBoutique : Client
    {
        public string NomEntreprise { get; private set; }

        public string NomContact { get; private set; }

        public ClientBoutique(Adresse adresse, string tel, string mail, string nomEntr, string nomContact) : base(adresse, tel, mail)
        {
            this.NomEntreprise = nomEntr;
            this.NomContact = nomContact;
        }
        public virtual string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'{typeClient}', '{nom}', '{prenom}', '{telephone}','{courriel}',{nomContact}" +
            $" {'remise'},{'fidelio'},{'idFidelio'},{'dateAdhesionFidelio'}";
    }
}
