using SIMULATOR.Classes;
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

namespace SIMULATOR
{
    /// <summary>
    /// Lógica de interacción para simulatorTest.xaml
    /// </summary>
    public partial class simulatorTest : Window
    {
        MainWindow mainWindow = Application.Current.Windows[0] as MainWindow;
        public simulatorTest()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Motor up
            PLCMemory.Outputs[8].Value = true;
            PLCMemory.Outputs[9].Value = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Motor down
            PLCMemory.Outputs[8].Value = false;
            PLCMemory.Outputs[9].Value = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Motor stop
            PLCMemory.Outputs[8].Value = false;
            PLCMemory.Outputs[9].Value = false;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            PLCMemory.Outputs[10].Value = true;
            PLCMemory.Outputs[11].Value = false;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            PLCMemory.Outputs[10].Value = false;
            PLCMemory.Outputs[11].Value = true;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            PLCMemory.Outputs[10].Value = false;
            PLCMemory.Outputs[11].Value = false;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            PLCMemory.Outputs[0].Value = true;
            PLCMemory.Outputs[4].Value = true;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            PLCMemory.Outputs[0].Value = false;
            PLCMemory.Outputs[4].Value = false;
        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            PLCMemory.InternalVariables[0].Value = true;
            PLCMemory.InternalVariables[4].Value = true;
            PLCMemory.InternalVariables[8].Value = true;
            PLCMemory.InternalVariables[9].Value = true;
            PLCMemory.InternalVariables[12].Value = true;
            PLCMemory.InternalVariables[15].Value = true;
            PLCMemory.InternalVariables[18].Value = true;
            PLCMemory.InternalVariables[20].Value = true;
        }
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            PLCMemory.InternalVariables[0].Value = false;
            PLCMemory.InternalVariables[4].Value = false;
            PLCMemory.InternalVariables[8].Value = false;
            PLCMemory.InternalVariables[9].Value = false;
            PLCMemory.InternalVariables[12].Value = false;
            PLCMemory.InternalVariables[15].Value = false;
            PLCMemory.InternalVariables[18].Value = false;
            PLCMemory.InternalVariables[20].Value = false;
        }
        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            foreach(Variable myVariable in PLCMemory.States)
            {
                myVariable.Value = false;
            }
        }
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            PLCMemory.States[0].Value = true;
            PLCMemory.States[3].Value = true;
            PLCMemory.States[6].Value = true;
            PLCMemory.States[12].Value = true;
            PLCMemory.States[18].Value = true;
            PLCMemory.States[22].Value = true;

            PLCMemory.States[1].Value = false;
            PLCMemory.States[4].Value = false;
            PLCMemory.States[7].Value = false;
            PLCMemory.States[13].Value = false;
            PLCMemory.States[19].Value = false;
            PLCMemory.States[23].Value = false;

            PLCMemory.States[2].Value = false;
            PLCMemory.States[5].Value = false;
            PLCMemory.States[8].Value = false;
            PLCMemory.States[14].Value = false;
            PLCMemory.States[20].Value = false;
            PLCMemory.States[24].Value = false;
        }
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            PLCMemory.States[0].Value = false;
            PLCMemory.States[3].Value = false;
            PLCMemory.States[6].Value = false;
            PLCMemory.States[12].Value = false;
            PLCMemory.States[18].Value = false;
            PLCMemory.States[22].Value = false;

            PLCMemory.States[1].Value = true;
            PLCMemory.States[4].Value = true;
            PLCMemory.States[7].Value = true;
            PLCMemory.States[13].Value = true;
            PLCMemory.States[19].Value = true;
            PLCMemory.States[23].Value = true;

            PLCMemory.States[2].Value = false;
            PLCMemory.States[5].Value = false;
            PLCMemory.States[8].Value = false;
            PLCMemory.States[14].Value = false;
            PLCMemory.States[20].Value = false;
            PLCMemory.States[24].Value = false;
        }
        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            PLCMemory.States[0].Value = false;
            PLCMemory.States[3].Value = false;
            PLCMemory.States[6].Value = false;
            PLCMemory.States[12].Value = false;
            PLCMemory.States[18].Value = false;
            PLCMemory.States[22].Value = false;

            PLCMemory.States[1].Value = false;
            PLCMemory.States[4].Value = false;
            PLCMemory.States[7].Value = false;
            PLCMemory.States[13].Value = false;
            PLCMemory.States[19].Value = false;
            PLCMemory.States[23].Value = false;

            PLCMemory.States[2].Value = true;
            PLCMemory.States[5].Value = true;
            PLCMemory.States[8].Value = true;
            PLCMemory.States[14].Value = true;
            PLCMemory.States[20].Value = true;
            PLCMemory.States[24].Value = true;
        }
    }
}
