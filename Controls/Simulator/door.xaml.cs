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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SIMULATOR.Controls
{
    /// <summary>
    /// Lógica de interacción para door.xaml
    /// </summary>
    /// 
    public partial class door : UserControl
    {
        
        // SIZE DEPENDANT CONFIGURATION
        public readonly double ClosedPositionDoor1 = 0;       //px
        private readonly double ClosedPositionDoor2 = 22;      //px
        public readonly double OpenPosition = 47;             //px

        protected double _translationSpeed; //px/s
        public double TranslationSpeed
        {
            set { _translationSpeed = value; }
            get { return _translationSpeed; }
        }

        private double _currentPosition;
        public double CurrentPosition
        {
            get
            {
                if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        _currentPosition = this.tt1.X;
                    }));
                }
                else
                {
                    _currentPosition = this.tt1.X;
                }
                return _currentPosition;
            }
        }


        private movement ClosingMovement1   
        {
            get
            {
                return new movement( - this.tt1.X, TranslationSpeed * 2);
            }
        }
        private movement OpeningMovement1
        {
            get
            {
                return new movement(OpenPosition - this.tt1.X - ClosedPositionDoor1, TranslationSpeed * 2);
            }
        }
        private movement ClosingMovement2
        {
            get
            {
                return new movement(- this.tt2.X, TranslationSpeed);
            }
        }
        private movement OpeningMovement2
        {
            get
            {
                return new movement(OpenPosition - this.tt2.X - ClosedPositionDoor2, TranslationSpeed);
            }
        }
        public door()
        {
            InitializeComponent();
        }
        public void StartOpen()
        {
            ConfigTranslateAnimation(true, true);
        }
        public void StartClose()
        {
            ConfigTranslateAnimation(true, false);
        }
        public void Stop()
        {
            ConfigTranslateAnimation(false, null);
        }
        protected void ConfigTranslateAnimation(bool move, bool? direction)
        {
            DoubleAnimation da1 = new DoubleAnimation();
            DoubleAnimation da2 = new DoubleAnimation();
            if (move)
            {
                da1.BeginTime = TimeSpan.FromSeconds(0);
                da2.BeginTime = TimeSpan.FromSeconds(0);
                da1.From = this.tt1.X;
                da2.From = this.tt2.X;
                if (direction.HasValue && direction.Value)
                {
                    da1.Duration = new Duration(TimeSpan.FromSeconds(OpeningMovement1.Time));
                    da2.Duration = new Duration(TimeSpan.FromSeconds(OpeningMovement2.Time));
                    da1.To = this.tt1.X + OpeningMovement1.Distance;
                    da2.To = this.tt2.X + OpeningMovement2.Distance;
                }
                else if (direction.HasValue && !direction.Value)
                {
                    da1.Duration = new Duration(TimeSpan.FromSeconds(ClosingMovement1.Time));
                    da2.Duration = new Duration(TimeSpan.FromSeconds(ClosingMovement2.Time));
                    da1.To = this.tt1.X + ClosingMovement1.Distance;
                    da2.To = this.tt2.X + ClosingMovement2.Distance;
                }
                else
                {
                    throw new Exception("Error: no se ha proporcionado dirección de movimiento válida");
                }
            }
            else
            {
                da1.BeginTime = null;
                da2.BeginTime = null;
            }
            this.tt1.BeginAnimation(TranslateTransform.XProperty, da1);
            this.tt2.BeginAnimation(TranslateTransform.XProperty, da2);
        }
    }
}
