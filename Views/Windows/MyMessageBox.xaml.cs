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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BDD_VELOMAX_APP.Views.Windows
{
    /// <summary>
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : Window
    {
        private double tempsMS;

        private MyMessageBox(string text, string entete, int temps)
        {
            InitializeComponent();

            this.DataContext = this;

            this.TB_Text.Text = text;

            this.TB_Entete.Text = entete;


            this.tempsMS = temps;

            StartCloseTimer();
        }

        /// <summary>
        /// Affiche une box qui fadeIn puis fadeOut au bout du <paramref name="tempsMS"/> indiqué
        /// </summary>
        /// <param name="text">Texte principa à afficher</param>
        /// <param name="tempsMS">Temps en milisec avant le fermement automatique</param>
        public static void Show(string text, int tempsMS)
        {
            Show(text, "", tempsMS);
        }

        /// <summary>
        /// Affiche une box qui fadeIn puis fadeOut au bout du <paramref name="tempsMS"/> indiqué
        /// </summary>
        /// <param name="text">Texte principa à afficher</param>
        /// <param name="entete">Entete à coté du logo</param>
        /// <param name="tempsMS">Temps en milisec avant le fermement automatique</param>
        public static void Show(string text, string entete, int tempsMS)
        {
            var mmb = new MyMessageBox(text, entete, tempsMS);
            mmb.Show();
        }

        private void StartCloseTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(this.tempsMS);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Window_Loaded;
            e.Handled = true;
            var anim = new DoubleAnimation(0, 1, (Duration)TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => this.Show();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(1, 0, (Duration)TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
    }
}
