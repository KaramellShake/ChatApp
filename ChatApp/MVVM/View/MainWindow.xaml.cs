using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.MVVM.ViewModel;

namespace ChatClient.MVVM.View
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) //Diese Methode bewirkt, dass wir unsere GUI, in dem wir die linke Maustaste gedrückt halten, bewegen können!
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e) //Mit dieser Methode können wir unsere GUI durch einen Klick auf den Button minimieren!
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e) //Diese Methode dient zum Maximieren oder Wiederherstellen der GUI!
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e) //Schließt die Anwendung!
        {
            this.Close();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NavigationMenuPopup.IsOpen) //Schließt das Popup, wenn irgendwo hingeklickt wird!
            {
                NavigationMenuPopup.IsOpen = false;

                return;
            }

            //Position des Popup setzen!
            NavigationMenuPopup.PlacementTarget = sender as UIElement;
            NavigationMenuPopup.Placement = PlacementMode.Bottom;
            NavigationMenuPopup.IsOpen = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}