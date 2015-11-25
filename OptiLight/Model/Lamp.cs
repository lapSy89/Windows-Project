using System.Xml.Serialization;

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
        public double Radius { get; set; } = -20;
    }
}
