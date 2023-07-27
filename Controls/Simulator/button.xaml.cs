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
    /// Lógica de interacción para button.xaml
    /// </summary>
    public partial class button : UserControl
    {
        private readonly SolidColorBrush colorOff, colorOn;
        private bool _pressed;
        public bool Pressed
        {
            get { return _pressed; }
            set { _pressed = value; }
        }
        private bool _callIndicator;
        public bool CallIndicator
        {
            get { return _callIndicator; }
            set 
            {
                _callIndicator = value;
                if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        ChangeColor(value);

                    }));
                }
                else ChangeColor(value);
            }
        }
        private void ChangeColor(bool value)
        {
            if (value)
            {
                buttonColorIndicator.Stroke = colorOn;
                buttonColorIndicator.Fill = colorOn;
            }
            else
            {
                buttonColorIndicator.Stroke = colorOff;
                buttonColorIndicator.Fill = null;
            }
        }
        public string NumberIndicator
        {
            get { return (string) numberContainer.Content; }
            set { numberContainer.Content = value;  }
        }

        public button()
        {
            InitializeComponent();
            Window myWindow = Window.GetWindow(this);
            colorOff = this.Resources["fontColor"] as SolidColorBrush;
            colorOn = this.Resources["foregorundHighlight"] as SolidColorBrush;
        }
        private void mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Pressed = true;
            //CallIndicator = true;
            onClickAnimation();
        }
        private void mouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            Pressed = false;
            //CallIndicator = false;
        }
        private void mouseEnter(object sender, EventArgs e)
        {
            //buttonLineIndicator.Stroke = colorOnHover;
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void mouseLeave(object sender, EventArgs e)
        {
            //buttonLineIndicator.Stroke = colorOff;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void onClickAnimation()
        {
            DoubleAnimation pressingAnimation = new DoubleAnimation();
            pressingAnimation.BeginTime = TimeSpan.FromSeconds(0);
            pressingAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            pressingAnimation.From = 1;
            pressingAnimation.To = 0.93;
            pressingAnimation.AutoReverse = true;
            st.BeginAnimation(ScaleTransform.ScaleXProperty, pressingAnimation);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, pressingAnimation);
        }
    }
}
