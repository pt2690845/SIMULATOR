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
    /// Lógica de interacción para sliderWithLimit.xaml
    /// </summary>
    public partial class cabinSensors : UserControl
    {
        public cabinSensors()
        {
            InitializeComponent();
            NumberOfStepsOnScroll = 30;
        }
        private double _numberOfStepsonScroll;
        public double NumberOfStepsOnScroll
        {
            get
            {
                return _numberOfStepsonScroll;
            }
            set
            {
                _numberOfStepsonScroll = value;
            }
        }

        private void WheelMoved(object sender, MouseWheelEventArgs e)
        {
            double valueBase1 = loadSensor.CurrentValueBase1;
            if (e.Delta > 0) loadSensor.CurrentValueBase1 = MakeInRange0To1(valueBase1 += 1 / NumberOfStepsOnScroll);
            else if (e.Delta < 0) loadSensor.CurrentValueBase1 = MakeInRange0To1(valueBase1 -= 1 / NumberOfStepsOnScroll);
        }
        private void MouseDown_Container(object sender, MouseButtonEventArgs e)
        {
            if(!loadSensor.MouseIn && !doorSensor.MouseIn)
            {
                if (doorSensor.Toggled) doorSensor.Toggled = false;
                else doorSensor.Toggled = true;
            }
        }
        private static double MakeInRange0To1(double variable)
        {
            if (variable > 1)
            {
                variable = 1;
            }
            else if (variable < 0)
            {
                variable = 0;
            }
            return variable;
        }
    }
}
