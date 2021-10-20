using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for CommandeConfirmationWindow.xaml
    /// </summary>
    public partial class CommandeConfirmationWindow : Window
    {
        private string HtmlText = "";
        private List<MyLinkedResource> resource;

        public CommandeConfirmationWindow(string htmlCode, List<MyLinkedResource> resource, string mail)
        {
            InitializeComponent();

            this.HtmlText = htmlCode;
            this.resource = resource;

            TB_Mail.Text += $" ({mail})";

        }

        private void Butt_Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.webBrowserCommande.Dispose();
            this.Close();
        }

        private void Butt_Commander_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            this.webBrowserCommande.Dispose();
            this.Close();
        }


        private void webBrowserCommande_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try //IsInitialized indique true au moment de quitter la fenêtre alors qu'en réalité le browser ne l'est déjà plus, peut dont crash à ce moment là
            {
                if (webBrowserCommande.IsInitialized)
                {
                    webBrowserCommande.LoadHtml(HtmlText);

                    resource.ForEach(x => webBrowserCommande.RegisterResourceHandler(x.ResourceFileName, x.ContentStream, x.ContentType.MediaType, false));

                    this.webBrowserCommande.ZoomLevelIncrement = 0.5;

                }
            }
            catch { }
        }
        private void webBrowserCommande_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            try
            {
                if (webBrowserCommande.IsInitialized)
                {
                    this.webBrowserCommande.SetZoomLevel(-3);
                    this.webBrowserCommande.ZoomLevel = -3;
                }
            }
            catch { }
        }


        #region mouse stuff

        private bool isControlKeyPressed = false;
        private const double maxZoomLevel = 10, minZoomLevel = -10;

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
            {
                isControlKeyPressed = false;
            }
        }

        private void OnKPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
            {
                isControlKeyPressed = true;
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (isControlKeyPressed)
            {
                if (e.Delta > 0 && webBrowserCommande.ZoomLevel <= maxZoomLevel)
                {
                    webBrowserCommande.ZoomInCommand.Execute(null);
                }
                else if (e.Delta < 0 && webBrowserCommande.ZoomLevel >= minZoomLevel)
                {
                    webBrowserCommande.ZoomOutCommand.Execute(null);
                }
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SystemCommands.ShowSystemMenu(this, GetMousePosition());
        }


        #region Helper mouse pos

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(this);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + this.Left, position.Y + this.Top);
        }

        #endregion

        #endregion

    }

    public class RectConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new System.Windows.Rect(0, 0, (double)values[0], (double)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
