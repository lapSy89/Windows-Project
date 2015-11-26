using System.Windows;
using OptiLight.Model;
using System.Windows.Media;

namespace OptiLight.ViewModel {
    public class CanvasViewModel : BaseViewModel {
        //TODO, consider not to use "RaisePropertyChanged(); but the Notify command instead
        public Model.Canvas Canvas { get; set; }

        public int cellSize { get { return Canvas.cellSize; } set { Canvas.cellSize = value; Canvas.height = Canvas.cellsY * value; Canvas.width = cellsX * value; RaisePropertyChanged(); } }
        public int cellsX { get { return Canvas.cellsX; } set { Canvas.cellsX = value; Canvas.width = value * Canvas.cellSize; RaisePropertyChanged(); } }
        public int cellsY { get { return Canvas.cellsY; } set { Canvas.cellsY = value; Canvas.height = value * Canvas.cellSize; RaisePropertyChanged(); } }
        public int width  { get { return Canvas.width; }  set { Canvas.width  = value; RaisePropertyChanged(); } }
        public int height { get { return Canvas.height; } set { Canvas.height = value; RaisePropertyChanged(); } }

        //public static CanvasViewModel Instance { get; } = new CanvasViewModel();
        public CanvasViewModel() {
            Canvas = new Model.Canvas();
        }
    }
}
