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
using System.Windows.Threading;



namespace SIMULATOR.Controls
{
    /// <summary>
    /// Lógica de interacción para limitSwitch.xaml
    /// </summary>
    public partial class limitSwitch : UserControl
    {
        private const double AngleOn = 25;
        private const double AngleOff = -25;

        public dynamic? ObjectToTrack;

        public double? PositionToCheckOn;
        public double? PositionToCheckOff;
        private void TurnOnorOff(bool value)
        {
            if (value)
            {
                colorIndicator.Stroke = Brushes.Green;
                this.rt.Angle = AngleOn;
            }
            else
            {
                colorIndicator.Stroke = Brushes.Red;
                this.rt.Angle = AngleOff;
            }
        }
        public bool Status
        {
            set
            {
                if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        TurnOnorOff(value);
                    }));
                }
                else
                {
                    TurnOnorOff(value);
                }
                _status = value;
            }
            get
            {
                return _status;
            }
        }
        private bool _status;

        public void PositionCheckingGreaterOrEqual()
        {
            if (ObjectToTrack != null)
            {
                if (ObjectToTrack.CurrentPosition <= PositionToCheckOn)
                {
                    Status = true;
                }
                else if (ObjectToTrack.CurrentPosition >= PositionToCheckOff)
                {
                    Status = false;
                }
            }
            else
            {
                throw new Exception("No ha configurado el objeto adecuadamente");
            }
        }
        public void PositionCheckingEqual()
        {
            if (ObjectToTrack != null)
            {
                if (ObjectToTrack.CurrentPosition == PositionToCheckOn)
                {
                    Status = true;
                }
                else Status = false;
            }
            else
            {
                throw new Exception("No ha configurado el objeto adecuadamente");
            }
        }
        public limitSwitch()
        {
            InitializeComponent();
        }
    }
}
