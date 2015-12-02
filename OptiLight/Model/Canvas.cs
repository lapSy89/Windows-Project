using System.Xml.Serialization;
using System.Windows.Media;
using System.Windows;

namespace OptiLight.Model {

    public class Canvas {

        // Default values for grid size
        public string visibility ="Transparent";
        public int cellSize { get; set; } = 50;
        public int cellsX { get; set; } = 12;
        public int cellsY { get; set; } = 10;
        public int height { get; set; } = 500;
        public int width { get; set; } = 600;
    }
}
