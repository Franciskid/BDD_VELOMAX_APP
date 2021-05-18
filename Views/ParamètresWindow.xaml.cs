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
using System.Windows.Shapes;

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Interaction logic for ParamètresWindow.xaml
    /// </summary>
    public partial class ParamètresWindow : Window
    {
        public ParamètresWindow()
        {
            InitializeComponent();
        }

        private void Butt_Abort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Butt_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
