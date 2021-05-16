using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class Pagestastique : UserControl
    {
            public Pagestastique()
            {
            InitializeComponent();


               
            List<Squantite> statsquantites = new List<Squantite>();
            foreach (Pieces a in DataReader.Read<Pieces>())
            {
            statsquantites.Add(new Squantite(a.Nom, a.Prix, a.DelaiApprovisionnement) );
            }

            statsquantite.ItemsSource = statsquantites;
                
            List<Sfidel> Sfidels = new List<Sfidel>();
           
            foreach (ClientIndividuel a in DataReader.Read<ClientIndividuel>())
            {
                if (a.ProgrammeFidélité != null)
                {
                    Sfidels.Add(new Sfidel("individuel", a.Nom, a.Telephone, a.AdresseMail,a.DateAdhésionProgramme)) ;
                }
            }
            statsfidelite.ItemsSource = Sfidels;


        }
        public class Squantite
        {
            public string Nom { get; set; }

            public float Prix { get; set; }

            public DateTime DelaiApprovisionnement { get; set; }

            public string Details
            {
                get
                {
                    return String.Format("{0} vaut {1} il faut attendre le {2} avant de se faire livrer.", this.Nom, this.Prix,this.DelaiApprovisionnement);
                }
            }
            public Squantite(string nom,float prix,DateTime delaiApprovisionnement)
            {
                this.Nom = nom;
                this.Prix = prix;
                this.DelaiApprovisionnement = delaiApprovisionnement;
            }
        }
        public class Sfidel
        {
            public string TypeClient { get; set; }

            public DateTime Datedebut { get; set; }
            public string Nom { get; set; }

            public string Telephone { get; set; }

            public string Courriel { get; set; }

            public string Details
            {
                get
                {
                    return String.Format("{0} est la date d'adhesion au programme {}", this.Datedebut,this.Datedebut.AddDays(1));
                }
            }
            public Sfidel(string typeClient, string nom, string telephone, string courriel,DateTime datadebut)
            {
                this.TypeClient = typeClient;
                this.Nom = nom;
                this.Telephone = telephone;
                this.Courriel = courriel;


            }
        }
    }
}