using System.Xml.Serialization;

namespace OptiLight.Model {

    public class Canvas {
        
        // Default values for grid size
        public int cellSize { get; set; } = 50;
        public int cellsX { get; set; } = 12;
        public int cellsY { get; set; } = 10;
        public int height { get; set; } = 500;
        public int width { get; set; } = 700;
    }
}
