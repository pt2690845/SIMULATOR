using SIMULATOR.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para configWindow.xaml
    /// </summary>
    public partial class configWindow : Window
    {
        public const int extraRows = 3;
        public configWindow()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            int numberOfSlots = PLCMemory.SLOT + extraRows;
            for (int i = 0; i < numberOfSlots; i++)
            {
                comboboxSlot.Items.Add(i.ToString());
            }
            comboboxSlot.SelectedIndex = PLCMemory.SLOT;

            int numberOfRacks = PLCMemory.RACK + extraRows;
            for (int i = 0; i < numberOfRacks; i++)
            {
                comboboxRack.Items.Add(i.ToString());
            }
            comboboxRack.SelectedIndex = PLCMemory.RACK;

            textboxIP.Text = PLCMemory.IP;
        }
        private void saveData()
        {
            PLCMemory.SLOT = comboboxSlot.SelectedIndex;
            PLCMemory.RACK = comboboxRack.SelectedIndex;
            IPAddress myIp;
            if (IPAddress.TryParse(textboxIP.Text, out myIp))
            {
                PLCMemory.IP = textboxIP.Text;
            }
            else
            {
                MessageBox.Show("La IP introducida no es válida");
            }
        }
        private void ClickOnApply (object sender, RoutedEventArgs e)
        {
            saveData();
        }
        private void ClickOnCancel(object sender, RoutedEventArgs e)
        {
            saveData();
        }
    }
}
