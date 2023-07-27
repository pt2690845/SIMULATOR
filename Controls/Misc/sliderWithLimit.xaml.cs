using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SIMULATOR.Controls
{
    /// <summary>
    /// Lógica de interacción para sliderWithLimit.xaml
    /// </summary>
    public partial class sliderWithLimit : UserControl
    {
        private readonly SolidColorBrush colorInLine = new SolidColorBrush(Color.FromRgb(0x46, 0xB9, 0xA2));
        private readonly SolidColorBrush colorOutLine = Brushes.Red;
        private readonly SolidColorBrush colorInPoint = new SolidColorBrush(Color.FromRgb(0xad, 0xd8, 0xe6));
        public sliderWithLimit()
        {
            InitializeComponent();
            CurrentValueBase1 = StartingValueBase1;
            LimitValue = double.NaN;
            IsScrollable = true;
        }
        public bool MouseIn { get; set; }
        public double CurrentValue
        {
            get
            {
                return _currentValueBase1 * (MaxValue - MinValue) + MinValue;
            }
        }
        private double _maxValue;
        public double MaxValue
        {
            set
            {
                _maxValue = value;
            }
            get
            {
                return _maxValue;
            }
        }
        private double _minValue;
        public double MinValue
        {
            set
            {
                _minValue = value;
            }
            get
            {
                return _minValue;
            }
        }
        private double _limitValue;
        public double LimitValue
        {
            set
            {
                _limitValue = value;
                drawBackground();
            }
            get
            {
                return _limitValue;
            }
        }
        public double LimitValueBase1
        {
            get
            {
                return LimitValue / (MaxValue - MinValue) + MinValue;
            }
        }
        public bool IsAboveLimit
        {
            get
            {
                if (CurrentValue > LimitValue)
                {
                    return true;
                }
                else return false;
            }
        }
        public double StartingValueBase1
        {
            get
            {
                return _startingValueBase1;
            }
            set
            {
                _startingValueBase1 = value;
            }
        }
        protected double _startingValueBase1 = 0.5;
        public double CurrentValueBase1
        {
            get
            {
                return _currentValueBase1;
            }
            set
            {
                if (1 >= value && value >= 0)
                {
                    _currentValueBase1 = value;
                    upDatePosition();
                    if (objectsToUpdate != null)
                    {
                        objectsToUpdate();
                    }
                }
                else
                {
                    throw new Exception("Valor introducido en base 1 es mayor a 1 o menor que 0");
                }
            }
        }
        protected double _currentValueBase1;
        protected double width
        {
            get
            {
                return Container.Width;
            }
        }
        public Action? objectsToUpdate;
       
        private bool _isScrollable;
        public bool IsScrollable
        {
            get
            {
                return _isScrollable;
            }
            set
            {
                _isScrollable = value;
            }
        }
        private void upDatePosition()
        {
            double xAxisMargin = width * (CurrentValueBase1);
            pointIndicator.Margin = new Thickness(xAxisMargin, 0, 0, 0);
            drawForeGround();
            if (CurrentValueBase1 > LimitValueBase1)
            {
                pointIndicator.Fill = colorInPoint;
                lineIndicator.Stroke = colorOutLine;
            }

            else
            {
                pointIndicator.Fill = colorInPoint;
                lineIndicator.Stroke = colorInLine;
            }
        }
        private void drawBackground()
        {
            string backgroundInRange = "M 0,50 L " + Convert.ToString((int)(width * LimitValueBase1)) + ",50";
            string backgroundOutOfRange = "M " + Convert.ToString((int)(width * LimitValueBase1)) + ",50 L " + Convert.ToString((int)(width)) + ",50";
            backgroundIndicatorInRange.Data = (Geometry)TypeDescriptor.GetConverter(typeof(Geometry)).ConvertFrom(backgroundInRange);
            backgroundIndicatorOutOfRange.Data = (Geometry)TypeDescriptor.GetConverter(typeof(Geometry)).ConvertFrom(backgroundOutOfRange);
        }
        private void drawForeGround()
        {
            string pathDataInRange = "M 0,50 L " + Convert.ToString((int)(width * CurrentValueBase1)) + ",50";
            lineIndicator.Data = (Geometry)TypeDescriptor.GetConverter(typeof(Geometry)).ConvertFrom(pathDataInRange);
        }
        private void MouseMove_Container(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double xAxisPosition = e.GetPosition(Container).X;
                double xAxisPositionBase1 = xAxisPosition / width;
                CurrentValueBase1 = MakeInRange0To1(xAxisPositionBase1);
            }
        }
        private void MouseEnter_Container(object sender, MouseEventArgs e)
        {
            MouseIn = true;
        }
        private void MouseLeave_Container(object sender, MouseEventArgs e)
        {
            MouseIn = false;
        }
        private static double MakeInRange0To1(double variable)
        {
            if (variable > 1)
            {
                variable = 1;
            }
            else if (variable < 0)
            {
                variable = 0;
            }
            return variable;
        }
    }
}
