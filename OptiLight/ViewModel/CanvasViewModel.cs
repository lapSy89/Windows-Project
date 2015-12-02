using System.Windows;
using LampLibrary; // LampLibrary DLL

namespace OptiLight.ViewModel {
    public class CanvasViewModel : BaseViewModel {

        // The canvas from the the Model
        public Canvas Canvas { get; set; }

        // We use the Singleton design pattern for our constructor
        public static CanvasViewModel Instance { get; } = new CanvasViewModel();
        private CanvasViewModel() {
            Canvas = new Canvas();
        }

        // Variable for whether the snapping is active or not
        private bool snapActive = false;
        public bool SnapActive {
            get { return snapActive; }
            set { snapActive = value; RaisePropertyChanged(); }
        }
        
        //Boolean variable for whether the grid is visible or not
        private bool gridVisible = false;
        public bool GridVisible {
            get { return gridVisible; }
            set { gridVisible = value; RaisePropertyChanged(); }
        }

        // The color of the grid. OFF = transparent, ON = black
        public string visibility {
            get { return Canvas.visibility; }
            set { Canvas.visibility = value; RaisePropertyChanged(); }
        }

        // Method for turning the visibility of the grid on and off
        // The new keyword makes sure we override the BaseViewModel method of same name
        new public void toggleVisibility() {
            if (!GridVisible) {
                visibility = "Black";
                GridVisible = true;
            }
            else {
                visibility = "Transparent";
                GridVisible = false;
            }
        }

        //Set the cell size of the canvas grid.
        public int cellSize {
            get { return Canvas.cellSize; }
            set {
                //Only values above 0 and below or equal to half of the shortest grid dimension
                if (value > 0 && value <= Canvas.width / 2 && value <= Canvas.height / 2) {

                    //Calculate amount of cells in height/width, so the canvas never shrinks at resizing
                    //This is to avoid lamps escaping the grid at resizing.
                    Canvas.cellsX = (Canvas.width % value > 0) ? (Canvas.width / value + 1) : Canvas.width / value;
                    Canvas.cellsY = (Canvas.height % value > 0) ? (Canvas.height / value + 1) : Canvas.height / value;

                    Canvas.cellSize = value;
                    Canvas.height = Canvas.cellsY * value;
                    Canvas.width = Canvas.cellsX * value;

                    RaisePropertyChanged();
                    RaisePropertyChanged(() => corner1);
                    RaisePropertyChanged(() => corner2);
                    RaisePropertyChanged(() => Canvas.height);
                    RaisePropertyChanged(() => Canvas.width);
                    RaisePropertyChanged(() => Canvas.cellsX);
                    RaisePropertyChanged(() => Canvas.cellsY);
                    RaisePropertyChanged(() => viewport);
                } else {
                    MessageBox.Show("Only positive values Between 1 and half of the shortest grid dimension", "OptiLight - Invalid Input", MessageBoxButton.OK);
                }
            }
        }

        public int cellsX {
            get { return Canvas.cellsX; }
            set {
                if(value > 0) {
                    Canvas.cellsX = value;
                    Canvas.width = value * Canvas.cellSize;
                    RaisePropertyChanged();
                    RaisePropertyChanged(() => Canvas.width);
                } else {
                    MessageBox.Show("Positive Values Only", "OptiLight - Invalid Input", MessageBoxButton.OK);
                }
            }
        }

        public int cellsY {
            get { return Canvas.cellsY; }
            set {
                if (value > 0) {
                    Canvas.cellsY = value;
                    Canvas.height = value * Canvas.cellSize;
                    RaisePropertyChanged();
                    RaisePropertyChanged(() => Canvas.height);
                } else {
                    MessageBox.Show("Positive Values Only", "OptiLight - Invalid Input", MessageBoxButton.OK);
                }
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
    }
}
