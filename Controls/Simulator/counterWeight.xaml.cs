using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para counterWeight.xaml
    /// </summary>
    public partial class CounterWeight : Cabin
    {
        public CounterWeight()
        {
            InitializeComponent();
            string newPathData = "M 1,0 L 1,150 L 12,150 L 12,0 L 1,0 M 7,0 L 7,-800";
            CabinIndicator.Data = (Geometry)TypeDescriptor.GetConverter(typeof(Geometry)).ConvertFrom(newPathData);
            CabinIndicator.Fill = new SolidColorBrush(Color.FromRgb(0xa5, 0xa5, 0xb0));
        }
    }
}
