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

        public int Remise { get; private set; }


        /// <summary>
        /// Programme fidélité du client. Si ce paramètre est null alors le client n'a pas souscris à un programme de fidélité
        /// </summary>
        public Fidelio ProgrammeFidélité { get; private set; }


        public DateTime DateAdhésionProgramme { get; private set; }


        public ClientIndividuel(Object Id, string nom, string prenom, int idAdresse, string tel, string mail,int remise, int fidel, DateTime adhesion) : base(Id, idAdresse, tel, mail)
        {
            this.Prénom = prenom;
            this.Nom = nom;
            this.Remise = remise;
            this.ProgrammeFidélité = DataReader.GetObject<Fidelio>(fidel);
            this.DateAdhésionProgramme = adhesion;
        }

        public override string SaveStr() => (ID != null ? $"'{ID}', " : "") + $"'individuel', '{Nom}','{Prénom}', " +
            $"{(base.Adresse.ID != null ? $"'{base.Adresse.ID}'" : "null")}, " +
            $"'{base.Telephone}','{base.AdresseMail}',''," +
            $"'{Remise}'," +
            $"{(this.ProgrammeFidélité != null ? $"True" : "False")}," +
            $"'{(this.ProgrammeFidélité.ID != null ? $"{this.ProgrammeFidélité.ID}" : "null")}'," +
            $"'{(this.ProgrammeFidélité != null ? $"{DateAdhésionProgramme:dd/MM:yyyy}" : "")}'";
    }
}