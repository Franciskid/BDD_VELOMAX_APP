using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class FournisseurViewModel : INotifyPropertyChanged
    {
        private int siret;
        public int Siret
        {
            get => siret;
            set
            {
                this.siret = value;
                this.OnPropertyChanged("Siret");
            }
        }

        private string nom;
        public string Nom
        {
            get => nom;
            set
            {
                this.nom = value;
                this.OnPropertyChanged("Nom");
            }
        }

        private string contact;

        public string Contact
        {
            get => contact;
            set
            {
                this.contact = value;
                this.OnPropertyChanged("Contact");
            }
        }

        private string score;
        public string Score
        {
            get => score;
            set
            {
                this.score = value;
                this.OnPropertyChanged("Score");
            }
        }


        private string adresse;
        public string Adresse
        {
            get => adresse;
            set
            {
                this.adresse = value;
                this.OnPropertyChanged("Adresse");
            }
        }

        private string codepostal;
        public string CodePostal
        {
            get => codepostal;
            set
            {
                this.codepostal = value;
                this.OnPropertyChanged("CodePostal");
            }
        }

        private string ville;

        public string Ville
        {
            get => ville;
            set
            {
                this.ville = value;
                this.OnPropertyChanged("Ville");
            }
        }

        private string province;

        public string Province
        {
            get => province;
            set
            {
                this.province = value;
                this.OnPropertyChanged("Province");
            }
        }


        public FournisseurViewModel() { }


        public FournisseurViewModel(Fournisseurs f)
        {
            this.Siret = (int)f.ID;
            this.Nom = f.Nom;
            this.Contact = f.Contact;
            this.score = f.Score.ToString();
            this.Adresse = f.Adresse.Rue;
            this.CodePostal = f.Adresse.CodePostal;
            this.Ville = f.Adresse.Ville;
            this.Province = f.Adresse.Province;
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
