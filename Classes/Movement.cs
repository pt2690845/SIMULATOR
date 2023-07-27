using SIMULATOR.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIMULATOR
{
    public class movement
    {
        public readonly double Distance;
        public double Time
        {
            get
            {
                return Math.Abs(Distance) / _speed;
            }
        }
        private double _speed;
        public double Speed
        {
            get
            {
                return _speed;
            }
        }
        public movement(double distance, double speed)
        {
            Distance = distance;
            _speed = speed;
        }
    }
}
