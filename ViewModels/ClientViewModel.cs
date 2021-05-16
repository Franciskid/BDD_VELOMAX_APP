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
        private string type;
        public string Type {
            get => type;
            set
            {
                this.type = value;
                this.OnPropertyChanged("Type");
            }
        }

        private string nom;
        public string Nom {
            get => nom;
            set
            {
                this.nom = value;
                this.OnPropertyChanged("Nom");
            }
        }

        private string prénom;

        public string Prénom {
            get => prénom;
            set
            {
                this.prénom = value;
                this.OnPropertyChanged("Prénom");
            }
        }

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

        private string adresse;
        public string Adresse {
            get => adresse;
            set
            {
                this.adresse = value;
                this.OnPropertyChanged("Adresse");
            }
        }

        private int codepostal;
        public int CodePostal {
            get => codepostal;
            set
            {
                this.codepostal = value;
                this.OnPropertyChanged("CodePostal");
            }
        }

        private string ville;

        public string Ville {
            get => ville;
            set
            {
                this.ville = value;
                this.OnPropertyChanged("Ville");
            }
        }

        private string province;
        public string Province {
            get => province;
            set
            {
                this.province = value;
                this.OnPropertyChanged("Province");
            }
        }

        private int remise;
        public int Remise {
            get => remise;
            set
            {
                this.remise = value;
                this.OnPropertyChanged("Remise");
            }
        }

        private string programmefidélité;
        public string ProgrammeFidélité {
            get => programmefidélité;
            set
            {
                this.programmefidélité = value;
                this.OnPropertyChanged("ProgrammeFidélité");
            }
        }

        private DateTime dateadhésion;
        public DateTime DateAdhésion {
            get => dateadhésion;
            set
            {
                this.dateadhésion = value;
                this.OnPropertyChanged("DateAdhésion");
            }
        }

        private DateTime datefin;
        public DateTime DateFin {
            get => datefin;
            set
            {
                this.datefin = value;
                this.OnPropertyChanged("DateFin");
            }
        }

        private string nomcontact;
        public string NomContact {
            get => nomcontact;
            set
            {
                this.nomcontact = value;
                this.OnPropertyChanged("NomContact");
            }
        }

       
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
                this.Nom = ((ClientBoutique)cli).NomEntreprise;
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
