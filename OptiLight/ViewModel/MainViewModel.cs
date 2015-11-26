using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace OptiLight.ViewModel {

    public class MainViewModel : BaseViewModel {
        // Global points created for having initials positions of Lamp and Mouse when a lamp is moved.
        private Point initialLampPosition;
        private Point initialMousePosition;

        // The collection of the lamps

        // Grid control variables
        private bool snapActive = false;
        public string gridVisibility { get; set; } = "Transparent";
        public int cellSet { get { return cellSize; } set { System.Console.WriteLine(value); cellSize = value; } }
        public int cellSize { get; set; }
        public int cellsX { get; set; }
        public int cellsY { get; set; }
        public int width { get; set; }
        public int height { get; set; }



        // The possible commands
        public ICommand LampPressedCommand { get; }
        public ICommand MouseDownCanvasCommand { get; }
        public ICommand LampReleasedCommand { get; }
        public ICommand LampMovedCommand { get; }
        public ICommand toggleSnappingCommand { get; }
        public ICommand toggleGridVisibilityCommand { get; }

        // Constructor - creates the initial lamps and initializes the commands
        public MainViewModel() : base() {
            cellSet = 50; //Default cell size
            cellsX = 14;
            cellsY = 10;
            width = cellSize * cellsX;
            height = cellSize * cellsY;

            // Lamps created from the start
            //It generates a collection of LampViewModels
            Lamps = new ObservableCollection<LampViewModel>() {
                 new RoundLampViewModel(new Model.RoundLamp())
            };
            HighlightedLamps = new ObservableCollection<LampViewModel> { };

            // Commands are defined as relay commands
            LampPressedCommand = new RelayCommand<MouseButtonEventArgs>(LampPressed);
            LampReleasedCommand = new RelayCommand<MouseButtonEventArgs>(LampReleased);
            LampMovedCommand = new RelayCommand<MouseEventArgs>(LampMoved);
            toggleSnappingCommand = new RelayCommand(toggleSnapping);
            toggleGridVisibilityCommand = new RelayCommand(toggleVisibility);
            MouseDownCanvasCommand = new RelayCommand<MouseButtonEventArgs>(CanvasDown);
        }

        public void toggleSnapping()
        {
            snapActive = !snapActive;
        }
    
        public void toggleVisibility() {
            if (gridVisibility.Equals("Black")) {
                gridVisibility = "Transparent";
            } else if (gridVisibility.Equals("Transparent")) {
                gridVisibility = "Black";
            }
            RaisePropertyChanged(() => gridVisibility);
        }

        // Method for capturing the mouse on a lamp
        private void LampPressed(MouseButtonEventArgs e) {
            var Lamp = TargetLamp(e);
            var MousePosition = RelativeMousePosition(e);

            if (LampsAreSelected()) {
                UnSelectAllLamps();
            }

            initialLampPosition = new Point(Lamp.X, Lamp.Y);
            initialMousePosition = MousePosition;

            HighlightedLamps.Add(Lamp);
            Lamp.IsSelected = true;
            e.MouseDevice.Target.CaptureMouse();
        }

        // Method for releasing the capturing of the mouse on a lamp. After the mouse is released, 
        // the lamps new position is definedand saved to the lamp.
        private void LampReleased(MouseButtonEventArgs e) {
            var Lamp = TargetLamp(e);
            var MousePosition = RelativeMousePosition(e);

            if (MousePosition.X > 0 && MousePosition.Y > 0
                && MousePosition.X < width - cellSize
                && MousePosition.Y < height - cellSize) {

                Lamp.X = initialLampPosition.X;
                Lamp.Y = initialLampPosition.Y;

                var offsetX = MousePosition.X - initialMousePosition.X;
                var offsetY = MousePosition.Y - initialMousePosition.Y;
            
                if (snapActive) {
                    var extraX = (Lamp.X + offsetX) % cellSize; 
                    var extraY = (Lamp.Y + offsetY) % cellSize;

                    if (extraX > cellSize/2) {
                        offsetX = offsetX - extraX + 0.5 * cellSize;
                    }
                    else {
                        offsetX = offsetX - extraX + 0.5 * cellSize;
                    }
                    if (extraY > cellSize/2) {
                        offsetY = offsetY - extraY + 0.5 * cellSize;
                    }
                    else {
                        offsetY = offsetY - extraY + 0.5 * cellSize;
                    }
                }

                // The move command is only added to the undo/redo stack if the lamp is moved and not when
                // it is just selected
                if (offsetX != 0 || offsetY != 0) {
                    this.undoRedoController.AddAndExecute(new Command.MoveLamp(Lamp, offsetX, offsetY));
                }
            }
            e.MouseDevice.Target.ReleaseMouseCapture();
        }



        private void CanvasDown(MouseButtonEventArgs e) {
            if (LampsAreSelected()) {
                UnSelectAllLamps();
            }
        }
        // Method for moving the lamp. This is created as an on-the-go method, so that each "pixel" 
        // move of the lamp isn't saved in the undo-redo command. 
        // So when undo is pressed, the lamp is moved to it's original position before even moving.

        private void LampMoved(MouseEventArgs e) {
            if (Mouse.Captured != null){

                var Lamp = TargetLamp(e);
                var MousePosition = RelativeMousePosition(e);

                var offsetX = MousePosition.X - initialMousePosition.X;
                var offsetY = MousePosition.Y - initialMousePosition.Y;
                var newX = initialLampPosition.X + offsetX;
                var newY = initialLampPosition.Y + offsetY;

                if (newX > 0 && newY > 0 && newX < width-cellSize && newY < height-cellSize){
                    if (snapActive){
                        var extraX = newX % cellSize;
                        var extraY = newY % cellSize;

                        if (extraX > cellSize / 2) {
                            newX = newX - extraX + 0.5 * cellSize;
                        } else {
                            newX = newX - extraX + 0.5 * cellSize;
                        }
                        if (extraY > cellSize / 2) {
                            newY = newY - extraY + 0.5 * cellSize;
                        } else {
                            newY = newY - extraY + 0.5 * cellSize;
                        }
                    }

                    Lamp.X = newX;
                    Lamp.Y = newY;
                }
            }
        }

        // Helping method for attaching the mouse to a lamp
        private LampViewModel TargetLamp(MouseEventArgs e) {
            var targetedElement = (FrameworkElement)e.MouseDevice.Target;
            return (LampViewModel)targetedElement.DataContext;
        }
        // Helper method for registration of the position of the mouse.
        private Point RelativeMousePosition(MouseEventArgs e) {
            var targetedElement = (FrameworkElement)e.MouseDevice.Target;
            var canvas = FindParentOfType<Canvas>(targetedElement);
            return Mouse.GetPosition(canvas);
        }

        // Ved ikke rigtig hvad denne gør!?
        private static T FindParentOfType<T>(DependencyObject o) {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
    }
}
