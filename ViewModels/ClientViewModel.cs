using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        public string Type { get; set; }

        public string Nom { get; set; }

        public string Prénom { get; set; }

        private string mail;

        public string Mail
        {
            get => mail;
            set
            {
                this.mail = value;
                this.OnPropertyChanged("Mail");
            }
        }

        private string phone;

        public string Téléphone
        {
            get => phone;
            set
            {
                this.phone = value;
                this.OnPropertyChanged("Téléphone");
            }
        }

        public string Adresse { get; set; }

        public int CodePostal { get; set; }

        public string Ville { get; set; }

        public string Province { get; set; }

        public int Remise { get; set; }

        public string ProgrammeFidélité { get; set; }

        public DateTime DateAdhésion { get; set; }

        public DateTime DateFin { get; set; }

        public string NomContact { get; set; }

        public ClientViewModel() { }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
