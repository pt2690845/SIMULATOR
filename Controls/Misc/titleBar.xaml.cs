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

namespace SIMULATOR.Controls
{
    /// <summary>
    /// Lógica de interacción para closeAndMinimizeBar.xaml
    /// </summary>
    public partial class titleBar : UserControl
    {
        public titleBar()
        {
            InitializeComponent();
        }
        public string Title
        {
            set
            {
                titleLabel.Content = value;
            }
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            Window myWindow = Window.GetWindow(this);
            myWindow.WindowState = WindowState.Minimized;
        }
        private void MouseDown_Container(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Window myWindow = Window.GetWindow(this);
                myWindow.DragMove();
            }  
        }
    }
}
