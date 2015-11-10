using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Model
{
    public class Lamp : Notify
    {
        // Standard x value for placing lamp in Grid with method for setting and getting
        private double x = 200;
        public double X { get { return x; } set { x = value; NotifyPropertyChanged(); } }

        // Standard y value for placing lamp in Grid with method for setting and getting
        private double y = 200;
        public double Y { get { return y; } set { y = value; NotifyPropertyChanged(); } }

        // Standard width value for size of Round Lamp with method for setting and getting
        private double roundWidth = 50;
        public double RoundWidth { get { return roundWidth; } set { roundWidth = value; NotifyPropertyChanged(); } }

        // Standard height value for size of Round Lamp with method for setting and getting
        private double roundHeight = 50;
        public double RoundHeight { get { return roundHeight; } set { roundHeight = value; NotifyPropertyChanged(); } }

        // Standard width value for size of Square Lamp with method for setting and getting
        private double squareWidth = 60;
        public double SquareWidth { get { return squareWidth; } set { squareWidth = value; NotifyPropertyChanged(); } }

        // Standard height value for size of Square Lamp with method for setting and getting
        private double squareHeight = 60;
        public double SquareHeight { get { return squareHeight; } set { squareHeight = value; NotifyPropertyChanged(); } }
    }
}
