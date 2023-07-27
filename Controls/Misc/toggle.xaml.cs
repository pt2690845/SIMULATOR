using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    /// Lógica de interacción para toggle.xaml
    /// </summary>
    public partial class toggle : UserControl
    {
        private readonly SolidColorBrush backgroundColorBlue = new SolidColorBrush(Color.FromRgb(0x00, 0x58, 0xb0));
        private readonly SolidColorBrush backgroundColorRed = new SolidColorBrush(Color.FromRgb(0xB0, 0x00, 0x20));
        private readonly SolidColorBrush backgroundColorOnEllipse;
        public toggle()
        {
            InitializeComponent();
            // Default Settings
            IsBlue = true;
            backgroundColorOnEllipse = this.Resources["fontColor"] as SolidColorBrush;
        }
        private bool _toggled;
        public bool Toggled
        {
            get
            {
                return _toggled;
            }
            set
            {
                _toggled = value;
                ToggleAnimation(value);
            }
        }
        public bool MouseIn { get; set; }
        public string TextOn { get;  set; }
        public string TextOff { get;  set; }
        private bool _isBlue;
        public bool IsBlue
        {
            get { return _isBlue; }
            set 
            {
                if (value)
                {
                    _isBlue = true;
                    IsRed = false;
                }
                else _isBlue = false;
            }
        }
        private bool _isRed;
        public bool IsRed
        {
            get { return _isRed; }
            set
            {
                if (value)
                {
                    _isRed = true;
                    IsBlue = false;
                }
                else _isRed = false;
            }
        }
        private void Viewbox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Toggled = !Toggled;
        }
        private void MouseEnter_Container(object sender, MouseEventArgs e)
        {
            MouseIn = true;
        }
        private void MouseLeave_Container(object sender, MouseEventArgs e)
        {
            MouseIn = false;
        }
        private void ToggleAnimation(bool toggled)
        {
            if (toggled && IsBlue && !IsRed)
            {
                ToggleBackground.Background = backgroundColorBlue;
                ToggleTitle.Text = TextOff;
                //ToggleDial.Fill = backgroundColorOnEllipse;
                Canvas.SetRight(ToggleDial, 5);
                Canvas.SetLeft(ToggleDial, 55);
            }
            else if (toggled && IsRed && !IsBlue)
            {
                ToggleBackground.Background = backgroundColorRed;
                ToggleTitle.Text = TextOff;
                //ToggleDial.Fill = backgroundColorOnEllipse;
                Canvas.SetRight(ToggleDial, 5);
                Canvas.SetLeft(ToggleDial, 55);
            }
            else if (!toggled && (IsRed || IsBlue))
            {
                ToggleBackground.Background = Brushes.Transparent;
                ToggleTitle.Text = TextOn;
                ToggleDial.Fill = Brushes.Transparent;
                Canvas.SetLeft(ToggleDial, 2.5);
                Canvas.SetRight(ToggleDial, 55);
            }
            else throw new Exception("No se ha indicado color para el Toggle");
        }
    }
}
