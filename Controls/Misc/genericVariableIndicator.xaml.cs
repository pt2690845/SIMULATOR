using SIMULATOR.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SIMULATOR.Controls.Misc
{
    /// <summary>
    /// Lógica de interacción para genericVariableIndicatorxaml.xaml
    /// </summary>
    public partial class genericVariableIndicator : UserControl
    {
        public genericVariableIndicator()
        {
            InitializeComponent();
        }
        public genericVariableIndicator(Variable variable)
        {
            InitializeComponent();

            // General properties
            NameColumn = variable.Name;
            VariableTypeColumn = variable.VariableType;
            DirectionColumn = variable.Direction;
            GroupColumn = variable.Group;
            AccessColumn = variable.Access;
            Value = variable.Value;
            Description = variable.Description;

            // Extra Config for state variables
            if(variable.IsState)
            {
                GroupName = variable.StateGroupName;
                stateHeader.Visibility = Visibility.Visible;
                stateContainer.Visibility = Visibility.Visible;
            }
            else
            {
                stateHeader.Visibility = Visibility.Collapsed;
                stateContainer.Visibility = Visibility.Collapsed;
            }

            // Input properties
            if(variable.IsInput)
            {
                valueColumnBool.IsHitTestVisible = true;
                valueColumnNumeric.IsReadOnly = false;
            }
            else
            {
                valueColumnBool.IsHitTestVisible = false;
                valueColumnNumeric.IsReadOnly = true;
            }

            // Type of value indicator
            if(variable.VariableType == "1_bit")
            {
                valueColumnBool.Visibility = Visibility.Visible;
                valueColumnNumeric.Visibility = Visibility.Collapsed;
            }
            else
            {
                valueColumnBool.Visibility = Visibility.Collapsed;
                valueColumnNumeric.Visibility = Visibility.Visible;
            }
            // Default 
            ExtraContentIsVisible = false;
        }
        public string NameColumn
        {
            get
            {
                return nameColumn.Text;
            }
            set
            {
                nameColumn.Text = value;
            }
        }
        public string VariableTypeColumn
        {
            get
            {
                return variableTypeColumn.Text;
            }
            set
            {
                variableTypeColumn.Text = value;
            }
        }
        public string DirectionColumn
        {
            get
            {
                return directionColumn.Text;
            }
            set
            {
                directionColumn.Text = value;
            }
        }
        public string GroupColumn
        {
            get
            {
                return groupColumn.Text;
            }
            set
            {
                groupColumn.Text = value;
            }
        }
        public string AccessColumn
        {
            get
            {
                return accessColumm.Text;
            }
            set
            {
                accessColumm.Text = value;
            }
        }
        public dynamic Value
        {
            get
            {
                if (variableTypeColumn.Text == "1_bit") return valueColumnBool.State;
                else if (variableTypeColumn.Text == "8_bit_unsigned") return byte.Parse(valueColumnNumeric.Text);
                else if (variableTypeColumn.Text == "32_bit_signed") return int.Parse(valueColumnNumeric.Text);
                // 32_bit_float
                else return float.Parse(valueColumnNumeric.Text);
            }
            set
            {
                if (variableTypeColumn.Text == "1_bit")
                {
                    if (value) valueColumnBool.VariableName = "True";
                    else valueColumnBool.VariableName = "False";
                    valueColumnBool.State = value;
                }
                // 8_bit_unsigned, 32_bit_signed, 32_bit_float
                else
                {
                    valueColumnNumeric.Text = value.ToString();
                }
            }
        }
        public string Description
        {
            get
            {
                return descriptionContainer.Text;
            }
            set
            {
                descriptionContainer.Text = value;
            }
        }
        public string GroupName
        {
            get
            {
                return stateContainer.Text;
            }
            set
            {
                stateContainer.Text = value;
            }
        }
        private bool _isVisible;
        private bool ExtraContentIsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if (value)
                {
                    extraContentContainer.Visibility = Visibility.Visible;
                    columnContainer.Background = Brushes.Gray;
                }
                else
                {
                    extraContentContainer.Visibility = Visibility.Collapsed;
                    columnContainer.Background = Brushes.Transparent;
                }
                _isVisible = value;
            }
        }
        private void ContainerClicked(object sender, MouseButtonEventArgs e)
        {
            StackPanel myStackPanel = this.Parent as StackPanel;
            foreach (genericVariableIndicator indicator in myStackPanel.Children)
            {
                if (indicator == this) continue;
                else if(indicator.ExtraContentIsVisible == true) indicator.ExtraContentIsVisible = false;
            }
            ExtraContentIsVisible = !ExtraContentIsVisible;
        }
        private void BoolVariableClicked(object sender, MouseButtonEventArgs e)
        {
            for(int i = 0; i < PLCMemory.Inputs.Count; i++)
            {
                if (NameColumn != PLCMemory.Inputs[i].Name) continue;
                PLCMemory.Inputs[i].Value = !PLCMemory.Inputs[i].Value;
            }
            ExtraContentIsVisible = !ExtraContentIsVisible;
        }
    }
}
