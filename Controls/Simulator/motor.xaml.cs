using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SIMULATOR.Controls
{
    /// <summary>
    /// Lógica de interacción para motor.xaml
    /// </summary>
    public partial class motor : UserControl
    {
        private bool _isSpinning;
        public Action MotorStartClockwise { get; set; }
        public Action MotorStartCounterClockwise { get; set; }
        public Action MotorStop { get; set; }
        public bool IsSpinning
        {
            get { return _isSpinning; }
            set
            {
                // The motor starts spinning
                if (!_isSpinning & value)
                {
                    // The motor starts spinning clockwise
                    if (_direction)
                    {
                        this.StartSpinningClockwise();
                        this.MotorStartClockwise();
                    }

                    // The motor starts moving counter clockwise
                    else
                    {
                        this.StartSpinningCounterClockwise();
                        this.MotorStartCounterClockwise();
                    }
                }
                // The motor stops spinning
                else if (_isSpinning & !value)
                {
                    this.StopSpinning();
                    this.MotorStop();
                }
                _isSpinning = value;
            }
        }
        private bool _direction;
        public bool Direction
        {
            get { return _direction; }
            set
            {
                // The motor is spinning and goes from counterclockwise to clockwise
                if (_isSpinning & !_direction & value)
                {
                    this.StartSpinningClockwise();
                    this.MotorStartClockwise();
                }
                // The motor is spinning and goes from clockwise to counter clockwise
                else if (_isSpinning & _direction & !value)
                {
                    this.StartSpinningCounterClockwise();
                    this.MotorStartCounterClockwise();
                }
                _direction = value;
            }
        }
        private double _rotationSpeed;  //rpm

        private bool _motorRight;
        public bool MotorRight
        {
            set
            {
                if(value)
                {
                    IsSpinning = true;
                    Direction = true;
                }
                else if (!value && !_motorLeft)
                {
                    IsSpinning = false;
                }
                _motorRight = value;
            }
        }
        private bool _motorLeft;
        public bool MotorLeft
        {
            set
            {
                if(value)
                {
                    IsSpinning = true;
                    Direction = false;
                }
                else if (!_motorRight && !value)
                {
                    IsSpinning = false;
                }
                _motorLeft = value;
                
            }
        }
        public double RotationSpeed
        {
            get
            {
                return _rotationSpeed;
            }
            set
            {
                _rotationSpeed = value;
            }
        }
        private double TimePeriod
        {
            get
            {
                return 60 / _rotationSpeed;
            }
        }

        public double Angle
        {
            get
            {
                return rt.Angle;
            }
        }        
        public double LinearSpeed
        {
            get
            {
                double rotationSpeedRad = RotationSpeed / 60 * (2 * Math.PI); //rad/s
                double radius = (MotorIndicator.Height + MotorIndicator.Width) / 2; //px
                return rotationSpeedRad * radius; //px/s
            }
        }

        public motor()
        {
            InitializeComponent();
        }

        private void StartSpinningClockwise()
        {
            configSpinAnimation(true, true);
        }
        private void StartSpinningCounterClockwise()
        {
            configSpinAnimation(true, false);
        }
        private void StopSpinning()
        {
            configSpinAnimation(false, null);
        }
        private void configSpinAnimation(bool spin, bool? direction)
        {
            DoubleAnimation da = new DoubleAnimation();
            if (spin)
            {
                da.BeginTime = TimeSpan.FromSeconds(0);
                da.Duration = new Duration(TimeSpan.FromSeconds(TimePeriod));
                da.From = rt.Angle;
                da.RepeatBehavior = RepeatBehavior.Forever;

                if (direction.HasValue && direction.Value)
                {
                    da.To = rt.Angle + 360;
                }
                else if (direction.HasValue && !direction.Value)
                {
                    da.To = rt.Angle - 360;
                }
                else
                {
                    throw new Exception("Error: no se ha proporcionado dirección de giro válida");
                }
            }
            else
            {
                da.BeginTime = null;
            }
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }
    }
}
