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
            }

            private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                InitializeComponent();

                List<Pieces> statsquantites = new List<Pieces>();
                foreach (Pieces a in DataReader.Read<Pieces>())
                {
              
                }


               

            }
        public class users
        {
            public int Nom { get; set; }

            public string Prix { get; set; }

            public DateTime DelaiApprovisionnement { get; set; }

            public string Details
            {
                get
                {
                    return String.Format("{0} vaut {1} il faut attendre le {2} avant de se faire livrer.", this.Nom, this.Prix,this.DelaiApprovisionnement);
                }
            }
        }
    }
}