using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Model
{
    public abstract class Lamp
    {
        // Standard x value for placing lamp in Grid with method for setting and getting
       
        public double X { get; set; } = 100;
        public double Y { get; set; } = 100;
        public double Width { get; set; } = 50;
        public double Height { get; set; } = 50;
        public double Radius { get; set; } = -20;
    }
}
