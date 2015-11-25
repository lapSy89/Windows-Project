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
        public double Width { get; set; } = 50;
        public double Height { get; set; } = 50;
        public double Radius { get; set; } = -100;
        public double Vertical { get; set; } = -100;
        public double Horizontal { get; set; } = -100;
        //The name of the lamp type
        public abstract string name { get; }

        // The path of the image of the lamp
        public abstract string img { get; }

        //The list with the different lamp types 
        public static List<Lamp> lampTypes { get; } = new List<Lamp>() {
            new RectangleLamp(),
            new RoundLamp(),
            new SquareLamp()
        };
 
    }
}
