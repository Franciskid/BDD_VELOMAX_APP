using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour CommandePage.xaml
    /// </summary>
    public partial class CommandePage : UserControl
    {
        public ObservableCollection<ModeleCommandeViewModel> modeleCollec = new ObservableCollection<ModeleCommandeViewModel>();

        public ObservableCollection<PieceCommandeViewModel> pieceCollec = new ObservableCollection<PieceCommandeViewModel>();

        public ObservableCollection<PrixCheckoutViewModel> checkout = new ObservableCollection<PrixCheckoutViewModel>();


        public CommandePage()
        {
            InitializeComponent();

            var pieces = BDDReader.Read<Piece>();
            var modeles = BDDReader.Read<Modele>();
            var clients = BDDReader.Read<Client>();

            this.dataGridModele.ItemsSource = modeleCollec;
            this.dataGridPiece.ItemsSource = pieceCollec;

            pieces.ForEach(x => pieceCollec.Add(new PieceCommandeViewModel(x)));
            modeles.ForEach(x => modeleCollec.Add(new ModeleCommandeViewModel(x)));

            this.DataGridCheckout.ItemsSource = checkout;

            cb_Client.ItemsSource = clients.Select(x => {
                if (x is ClientBoutique bout)
                    return $"{x.ID} Boutique : {bout.NomEntreprise}";
                if (x is ClientIndividuel indiv)
                    return $"{x.ID} Individuel : {indiv.Prénom} {indiv.Nom}";

                return x.ID.ToString();
            });
        }

        private void dataGridModele_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            checkout.Clear();

            foreach (var piece in pieceCollec)
            {
                if (piece.IsSelected)
                {
                    checkout.Add(new PrixCheckoutViewModel(piece));
                }
            }
            foreach (var modele in modeleCollec)
            {
                if (modele.IsSelected)
                {
                    checkout.Add(new PrixCheckoutViewModel(modele));
                }
            }
            TB_PrixTotal.Text = checkout.Aggregate(0f, (x, y) => x += y.Prix).ToString() + "€";
        }

        private void cb_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] txt = this.cb_Client.SelectedItem.ToString().Split(' ');

            if (txt[1] == "Boutique")
            {
                this.TB_Remise.Text = BDDReader.GetObject<ClientBoutique>(txt[0]).Remise + "%";
            }
            else
            {
                this.TB_Remise.Text = BDDReader.GetObject<ClientIndividuel>(txt[0]).ProgrammeFidélité.Rabais + "%";
            }

            TB_PrixTotal.Text = (checkout.Aggregate(0f, (x, y) => x += y.Prix * y.Quantité) * (100 - int.Parse(TB_Remise.Text.Substring(0, TB_Remise.Text.Length - 1))) / 100).ToString() + "€";
        }

        private void Butt_Checkout_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
