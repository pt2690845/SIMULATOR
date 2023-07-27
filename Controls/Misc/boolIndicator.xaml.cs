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
    /// Lógica de interacción para boolIndicator.xaml
    /// </summary>
    public partial class boolIndicator : UserControl
    {
        private readonly SolidColorBrush backgroundOn, backgroundOff, foregroundOn, foregroundOff;

        public string VariableName
        {
            get
            {
                return VariableNameLabel.Text;
            }
            set
            {
                VariableNameLabel.Text = value;
            }
        }
        private bool _state;
        public bool State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value) TurnOn();
                else TurnOff();
                _state = value;
            }
        }
        public boolIndicator()
        {
            InitializeComponent();
            backgroundOn = this.Resources["backgroundHighlight"] as SolidColorBrush;
            backgroundOff = this.Resources["foreground"] as SolidColorBrush;
            foregroundOn = this.Resources["foregorundHighlight"] as SolidColorBrush;
            foregroundOff = this.Resources["fontColor"] as SolidColorBrush;
        }
        private void TurnOn()
        {
            Border.BorderBrush = foregroundOn;
            VariableNameLabel.Background = backgroundOn;
            VariableNameLabel.Foreground = Brushes.White;
        }
        private void TurnOff()
        {
            Border.BorderBrush = Brushes.Transparent;
            VariableNameLabel.Background = backgroundOff;
            VariableNameLabel.Foreground = foregroundOff;
        }
    }
}
