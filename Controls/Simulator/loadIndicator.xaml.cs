using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    /// Lógica de interacción para loadIndicator.xaml
    /// </summary>
    public partial class loadIndicator : UserControl
    {
        private readonly SolidColorBrush colorInLine = new SolidColorBrush(Color.FromRgb(0x7f, 0xff, 0xd4));
        private readonly SolidColorBrush colorOutLine = Brushes.Red;
        public sliderWithLimit sliderToTrack { get; set; }
        private readonly double startingValue = 0.5;
        public void UpdateValue()
        {
            double scale = sliderToTrack.CurrentValueBase1 / sliderToTrack.LimitValueBase1;
            if (scale > 1) scale = 1;
            st.ScaleY = scale;
            if (sliderToTrack.IsAboveLimit)
            {
                Bottom.Stroke = colorOutLine;
                Top.Visibility = Visibility.Visible;
                Lateral.Stroke = colorOutLine;
            }
            else if (!sliderToTrack.IsAboveLimit && scale > 0)
            {
                Bottom.Stroke = colorInLine;
                Top.Visibility = Visibility.Collapsed;
                Bottom.Visibility = Visibility.Visible;
                Lateral.Stroke = colorInLine;
            }
            else
            {
                Bottom.Visibility = Visibility.Collapsed;
            }
        }
        public loadIndicator()
        {
            InitializeComponent();
            st.ScaleY = startingValue;
        }
    }
}
