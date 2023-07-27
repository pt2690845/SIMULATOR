using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Lógica de interacción para cabin.xaml
    /// </summary
    public partial class Cabin : UserControl
    {
        public double TopPosition = 100;      //px
        public double BottomPosition = 625;   //px
        private double _startingPosition;
        protected double StartingPosition           //px
        {
            get
            {
                if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        _startingPosition = Canvas.GetTop(this);
                    }));
                }
                else
                {
                    _startingPosition =  Canvas.GetTop(this);
                }
                return _startingPosition;
            }
        }
        private double _movement;
        public double CurrentPosition                //px
        {
            get
            {
                if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        _movement = this.tt.Y;
                    }));
                }
                else
                {
                    _movement = this.tt.Y;
                }
                return StartingPosition + _movement;
            }
        }

        public List<dynamic> ObjectsInside = new List<dynamic>();

        protected double _translationSpeed; //px/s
        public double TranslationSpeed
        {
            set { _translationSpeed = value; }
            get { return _translationSpeed; }
        }

        protected movement upMovement
        {
            get
            {
                return new movement(TopPosition - this.tt.Y - StartingPosition, TranslationSpeed);
            }
        }
        protected movement downMovement
        {
            get
            {
                return new movement(BottomPosition - this.tt.Y - StartingPosition, TranslationSpeed);
            }
        }

        public Cabin()
        {
            InitializeComponent();
        }

        public void StartTranslateUp()
        {
            ConfigTranslateAnimation(true, true);
        }
        public void StartTranslateDown()
        {
            ConfigTranslateAnimation(true, false);
        }
        public void StopTranslate()
        {
            ConfigTranslateAnimation(false, null);
        }
        protected void ConfigTranslateAnimation(bool move, bool? direction)
        {
            DoubleAnimation da = new DoubleAnimation();
            if (move)
            {
                da.BeginTime = TimeSpan.FromSeconds(0);
                da.From = this.tt.Y;
                if (direction.HasValue && direction.Value)
                {
                    da.Duration = new Duration(TimeSpan.FromSeconds(upMovement.Time));
                    da.To = this.tt.Y + upMovement.Distance;
                }
                else if (direction.HasValue && !direction.Value)
                {
                    da.Duration = new Duration(TimeSpan.FromSeconds(downMovement.Time));
                    da.To = this.tt.Y + downMovement.Distance;
                }
                else
                {
                    throw new Exception("Error: no se ha proporcionado dirección de movimiento válida");
                }
            }
            else
            {
                da.BeginTime = null;
            }
            this.tt.BeginAnimation(TranslateTransform.YProperty, da);
            for (int i = 0; i < ObjectsInside.Count(); i++)
            {
                ObjectsInside[i].tt.BeginAnimation(TranslateTransform.YProperty, da);
            }
        }
    }
}