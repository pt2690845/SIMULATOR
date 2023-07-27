using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Security.RightsManagement;
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
    /// Lógica de interacción para stateIndicator.xaml
    /// </summary>
    public class State
    {
        private string _name;
        public string Name 
        { 
            set
            {

                _name = value;
            }
            get
            {
                return _name;
            }
        }
        private bool _value;
        public bool Value
        {
            set
            {
                _value = value;
            }
            get
            {
                return _value;
            }
        }
        public State(string name, bool value)
        {
            Name = name;
            Value = value;
        }
    }
    public partial class stateIndicator : UserControl
    {
        private bool _isStateSelected;
        private bool IsStateSelected
        {
            get
            {
                return _isStateSelected;
            }
            set
            {
                if(value)
                {
                    stateName.Foreground = Brushes.White;
                    separator.Foreground = Brushes.White;
                    currentState.Foreground = Brushes.White;
                }
                else
                {
                    stateName.Foreground = this.Resources["fontColor"] as SolidColorBrush;
                    separator.Foreground = this.Resources["fontColor"] as SolidColorBrush;
                    currentState.Foreground = this.Resources["fontColor"] as SolidColorBrush;
                }
                _isStateSelected = value;
            }
        }
        public string StateGroupName
        {
            set
            { 
                stateName.Text = value;
            }
            get
            {
                return stateName.Text;
            }
        }
        public List<State> GroupStates = new List<State>();
        public stateIndicator()
        {
            InitializeComponent();
            DataContext = this;
            stateName.ToolTip = this.StateGroupName;
            currentState.ToolTip = this.currentState.Text;
        }
        public void updateStateInfo()
        {
            for (int i = 0; i < GroupStates.Count; i++)
            {
                if (GroupStates[i].Value == true)
                {
                    currentState.Text = GroupStates[i].Name;
                    stateName.ToolTip = this.StateGroupName;
                    currentState.ToolTip = this.currentState.Text;
                    IsStateSelected = true;
                    break;
                }
                if (i == GroupStates.Count - 1)
                {
                    currentState.Text = "No hay estado en ejecución";
                    IsStateSelected = false;
                }
            }
        }
    }
}
