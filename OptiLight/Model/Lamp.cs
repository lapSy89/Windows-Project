using System.Xml.Serialization;
using System.Collections.Generic;

namespace OptiLight.Model {

    [XmlInclude(typeof(RoundLamp))]
    [XmlInclude(typeof(SquareLamp))]
    [XmlInclude(typeof(RectangleLamp))]

    public abstract class Lamp {

        // Standard x value for placing lamp in Grid with method for setting and getting
        public double X { get; set; } = 100;
        public double Y { get; set; } = 100;
        public abstract double Width { get; set; }
        public abstract double Height { get; set; }
        public abstract double VerticalUp { get; set; }
        public abstract double VerticalDown { get; set; }
        public abstract double HorizontalLeft { get; set; }
        public abstract double HorizontalRight { get; set; }
        public abstract double VerticalUpConstant { get; }
        public abstract double VerticalDownConstant { get; }
        public abstract double HorizontalLeftConstant { get; }
        public abstract double HorizontalRightConstant { get; }

        // Lamp light settings
        public double Brightness { get; set; } = 30;
        public double LampHeight { get; set; } = 2;

        // The name of the lamp type - used for displaying the name in the sidemenu
        public abstract string name { get; }

        // The path of the image of the lamp in the side menu
        public abstract string img { get; }

        // Then name of the corresponding viewModel - used when created
        public abstract string viewModel { get; }

        //The list with the different lamp types 
        public static List<Lamp> lampTypes { get; } = new List<Lamp>() {
            new RectangleLamp(),
            new RoundLamp(),
            new SquareLamp()
        };
 
    }
}
