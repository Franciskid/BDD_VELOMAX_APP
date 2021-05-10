using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour PageTESTT.xaml
    /// </summary>
    public partial class PageTESTT : UserControl
    {
        public ObservableCollection<object> PersonCollection { get; set; }

        public ICollectionView PersonCollectionView { get; }

        public string personFilter = "";
        public string PersonFilter
        {
            get { return personFilter; }
            set
            {
                personFilter = value;
                if (this.CB_PropriétéFiltre.SelectedIndex == 0 && this.CB_TypeFiltre.SelectedIndex == 0)
                {
                    PersonCollectionView.Filter = FilterAll;
                }
                else if (this.CB_PropriétéFiltre.SelectedIndex != 0 && this.CB_TypeFiltre.SelectedIndex != 0)
                {
                    PersonCollectionView.Filter = FilterByTypeAndProperty;
                }
                else if (this.CB_TypeFiltre.SelectedIndex > 0)
                {
                    PersonCollectionView.Filter = FilterByType;
                }
                else if (this.CB_PropriétéFiltre.SelectedIndex > 0)
                {
                    PersonCollectionView.Filter = FilterByProperty;
                }
            }
        }


        public PageTESTT()
        {
            InitializeComponent();

            PersonCollectionView = CollectionViewSource.GetDefaultView(PersonCollection);

            PersonCollectionView.Filter = FilterAll;

            DataPerson.ItemsSource = PersonCollectionView;


            this.CB_PropriétéFiltre.ItemsSource = PropriétésFiltre();
            this.CB_PropriétéFiltre.SelectedIndex = 0;

            this.CB_TypeFiltre.ItemsSource = TypesFiltre();
            this.CB_TypeFiltre.SelectedIndex = 0;

            this.CB_Etat.ItemsSource = new List<string>()
            {
            };
        }


        /// <summary>
        /// Récupère toutes les propriétés publiques de <see cref="PersonneModel"/>
        /// </summary>
        /// <returns></returns>
        private List<string> PropriétésFiltre()
        {
            return null;
        }

        /// <summary>
        /// Crée une liste contenant les types de personne disponibles
        /// </summary>
        /// <returns></returns>
        private List<string> TypesFiltre()
        {
            return null;
        }

        #region Filtres

        public bool FilterByProperty(object x)
        {
            return true;
        }

        public bool FilterByType(object x)
        {
           
            return false;
        }

        public bool FilterAll(object x)
        {
            
            return false;
        }

        public bool FilterByTypeAndProperty(object x)
        {
            return false;
        }

        #endregion


        private void Butt_Delete_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Butt_Modify_Click(object sender, RoutedEventArgs e)
        {
        }


        private void CB_Filtres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.PersonFilter = this.personFilter;
        }


        private void TextBox_PreviewTextInput_OnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DataPerson_SelectionChanged(object sender, RoutedEventArgs e)
        {
        }

        private bool nextColor = false;
        private void DataPerson_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyle = new Style(typeof(DataGridCell));
            e.Column.CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush((nextColor = !nextColor) ? (Color)ColorConverter.ConvertFromString("#FFC2DDC2") : (Color)ColorConverter.ConvertFromString("#FFb2dbb2"))));
        }
    }
}
