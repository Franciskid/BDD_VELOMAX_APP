using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class ClientViewModel
    {
        public string Type { get; }

        public string Nom { get; }

        public string Prénom { get; }

        public string Mail { get; }

        public string Téléphone { get; }

        public string Adresse { get; }

        public int CodePostal { get; }

        public string Ville { get; }

        public string Province { get; }

        public int Remise { get; }

        public string ProgrammeFidélité { get; }

        public DateTime DateAdhésion { get; }

        public DateTime DateFin { get; }

        public string NomContact { get; }

        public ClientViewModel(Client cli)
        {
            this.Adresse = cli.Adresse.Rue;
            this.Ville = cli.Adresse.Ville;
            this.CodePostal = int.Parse(cli.Adresse.CodePostal);
            this.Province = cli.Adresse.Province;
            this.Mail = cli.AdresseMail;
            this.Téléphone = cli.Telephone;

            if (cli is ClientIndividuel ind)
            {
                this.Type = "Individuel";
                this.Nom = ind.Nom;
                this.Prénom = ind.Prénom;
                this.Remise = ind.Remise;
                this.ProgrammeFidélité = ind.ProgrammeFidélité.Description;
                this.DateAdhésion = ind.DateAdhésionProgramme;
                this.DateFin = ind.DateAdhésionProgramme + new TimeSpan((int)ind.ProgrammeFidélité.Duree_annee * 365, (int)(ind.ProgrammeFidélité.Duree_annee % 1 * 12) * 8760, 0, 0);
            }
            else
            {
                this.Type = "Boutique";
                this.NomContact = ((ClientBoutique)cli).NomContact;
            }
        }
    }
}
