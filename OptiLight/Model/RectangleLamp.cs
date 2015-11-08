using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Model
{
   public class RectangleLamp : Notify
    {
        // Standard x value for placing lamp in Grid with method for setting and getting
        private double x = 200;
        public double X { get { return x; } set { x = value; NotifyPropertyChanged(); } }

        // Standard y value for placing lamp in Grid with method for setting and getting
        private double y = 200;
        public double Y { get { return y; } set { y = value; NotifyPropertyChanged(); } }

        // Standard width value for size of lamp with method for setting and getting
        private double width = 120;
        public double Width { get { return width; } set { width = value; NotifyPropertyChanged(); } }

        // Standard height value for size of lamp with method for setting and getting
        private double height = 50;
        public double Height { get { return height; } set { height = value; NotifyPropertyChanged(); } }
    }
}
