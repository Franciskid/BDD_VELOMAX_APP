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
    /// Logique d'interaction pour StockPage.xaml
    /// </summary>
    public partial class StockPage : UserControl
    {
        public StockPage()
        {
            InitializeComponent();
            List <Pieces> pieces = BDDReader.Read<Pieces>();
            List<Spieces> piecesliste = new List<Spieces>();
            //pieces en stock
            foreach (Pieces a in pieces)
            {
                if (a.Quantité != 0)
                {
                    piecesliste.Add(new Spieces(a.ID.ToString(), a.Nom, a.Prix, a.DelaiApprovisionnement, a.Quantité));
                }
            }

            Datagridpiece.ItemsSource = piecesliste;

            // pieces par foursineur
        }




        public class Spieces
        {
            public string ID { get; set; }
            public string Nom { get; set; }

            public float Prix { get; set; }
            public int Quantite{ get; set; }

            public DateTime DelaiApprovisionnement { get; set; }
            public string Details
            {
                get
                {
                    return String.Format("{0} vaut {1} il faut attendre le {2} avant de se faire livrer.", this.ID, this.Prix, DateTime.Now.AddMonths(this.DelaiApprovisionnement.Month));
                }
            }

            public Spieces(string ID, string nom, float prix, DateTime delaiApprovisionnement, int quantite)
            {
                this.ID = ID;
                this.Nom = nom;
                this.Prix = prix;
                this.DelaiApprovisionnement = delaiApprovisionnement;
                this.Quantite= quantite;

            }
        }
    }
}
