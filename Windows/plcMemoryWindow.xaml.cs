using SIMULATOR.Classes;
using SIMULATOR.Controls.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SIMULATOR
{
    /// <summary>
    /// Lógica de interacción para plcMemoryWindow.xaml
    /// </summary>
    public partial class plcMemoryWindow : Window
    {
        Timer myTimer;
        public plcMemoryWindow()
        {
            InitializeComponent();

            CreateGenericVariableIndicators(PLCMemory.Inputs);
            CreateGenericVariableIndicators(PLCMemory.InternalVariables);
            CreateGenericVariableIndicators(PLCMemory.Outputs);
            CreateGenericVariableIndicators(PLCMemory.States);

            myTimer = new Timer(UpdateVariableIndicators, null, 0, 50);
            MultiThreadOperation.doTask = false;
        }
        private void CreateGenericVariableIndicators(List<Variable> Group)
        {
            for(int i = 0; i < Group.Count; i++)
            {
                genericVariableIndicator myVariable = new genericVariableIndicator(Group[i]);
                genericVariableContainer.Children.Add(myVariable);
            }
        }
        private int UpdateVariableIndicatorGrop(List<Variable> Group, int startingIndex)
        {
            genericVariableIndicator myIndicator;
            for (int i = 0; i < Group.Count; i++)
            {
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    myIndicator = genericVariableContainer.Children[i + startingIndex] as genericVariableIndicator;
                    myIndicator.Value = Group[i].Value;
                }));
            }
            return startingIndex + Group.Count;
        }
        private void UpdateVariableIndicators(object? state)
        {
            int index = 0;
            index = UpdateVariableIndicatorGrop(PLCMemory.Inputs, index);
            index = UpdateVariableIndicatorGrop(PLCMemory.InternalVariables, index);
            index = UpdateVariableIndicatorGrop(PLCMemory.Outputs, index);
            UpdateVariableIndicatorGrop(PLCMemory.States, index);
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            MultiThreadOperation.doTask = true;
        }
    }
}
