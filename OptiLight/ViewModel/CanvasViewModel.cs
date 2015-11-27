using System.Windows;
using System.Windows.Media;
using OptiLight.Model;

namespace OptiLight.ViewModel {
    public class CanvasViewModel : BaseViewModel {
        //TODO, consider not to use "RaisePropertyChanged(); but the Notify command instead
        public Model.Canvas Canvas { get; set; }

        public int cellSize {
            get { return Canvas.cellSize; }
            set {
                Canvas.cellSize = value;
                Canvas.height = Canvas.cellsY * value;
                Canvas.width = cellsX * value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => corner1);
                RaisePropertyChanged(() => corner2);
                RaisePropertyChanged(() => Canvas.height);
                RaisePropertyChanged(() => Canvas.width);
                RaisePropertyChanged(() => viewport);
            }
        }

        public int cellsX {
            get { return Canvas.cellsX; }
            set {
                Canvas.cellsX = value;
                Canvas.width = value * Canvas.cellSize;
                RaisePropertyChanged();
                RaisePropertyChanged(() => Canvas.width);
            }
        }

        public int cellsY {
            get { return Canvas.cellsY; }
            set {
                Canvas.cellsY = value;
                Canvas.height = value * Canvas.cellSize;
                RaisePropertyChanged();
                RaisePropertyChanged(() => Canvas.height);
            }
        }

        public int width {
            get { return Canvas.width; }
            set {
                Canvas.width = value;
                RaisePropertyChanged();
            }
        }

        public int height {
            get { return Canvas.height; }
            set {
                Canvas.height = value;
                RaisePropertyChanged();
            }
        }

        public Point corner1 { get { return (new Point(0, Canvas.cellSize)); } }
        public Point corner2 { get { return (new Point(Canvas.cellSize, Canvas.cellSize)); } }

        //Creates a viewpoint, the size of a cell, as defined by the Canvas.
        //The Viewpoint is a rectangle, constructed from two Points (0, 0) and (cellSize, cellSize).
        public Rect viewport { get {
                return new Rect(new Point(0, 0), new Point(Canvas.cellSize, Canvas.cellSize)); } }

        //public static CanvasViewModel Instance { get; } = new CanvasViewModel();
        public CanvasViewModel() {
            Canvas = new Model.Canvas();
        }
    }
}
