using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class ClientIndividuel : Client
    {
        public string Nom { get; private set; }

        public string Prénom { get; private set; }


        /// <summary>
        /// Programme fidélité du client. Si ce paramètre est null alors le client n'a pas souscris à un programme de fidélité
        /// </summary>
        public Fidelio ProgrammeFidélité { get; private set; }


        public DateTime DateAdhésionProgramme { get; private set; }


        public ClientIndividuel(Adresse adresse, string tel, string mail) : base(adresse, tel, mail)
        {
        }

        public virtual string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'boutique', '{NomEntreprise}', '{prenom}', '{base.Adresse.ID}', '{base.Telephone}',{base.AdresseMail}','{}','{remise}', '{fidelio}', '{idFidelio}', '{dateAdhesionFidelio}'";
    }
}