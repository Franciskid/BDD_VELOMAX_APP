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

        public ClientBoutique(object ID, string nomEntr, int idAdresse, string tel, string mail,  string nomContact) : base(ID, idAdresse, tel, mail)
        {
            this.NomEntreprise = nomEntr;
            this.NomContact = nomContact;
        }

        public override string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'boutique', '{NomEntreprise}',null," +
            $"{(base.Adresse.ID != null ? $"'{base.Adresse.ID}'" : "null")}, " +
            $"'{base.Telephone}','{base.AdresseMail}','{NomContact}',null, null, null, null";
    }
}
