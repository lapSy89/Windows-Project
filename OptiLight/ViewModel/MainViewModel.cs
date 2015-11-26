using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using OptiLight.Model;
using OptiLight.Command;
using System;
//using LampLibrary; // LampLibrary DLL

namespace OptiLight.ViewModel {

    public class MainViewModel : BaseViewModel {

        // Global points created for having initials positions of Lamp and Mouse when a lamp is moved.
        private Point initialLampPosition;
        private Point initialMousePosition;

        // Variables for showing and editing lamps (Brightness, Height and Radius)
        private double currentLampBrightness;
        private double currentLampHeight;
        private double currentLampRadius;

        // Grid control variables
        private bool snapActive = false;
        public string gridVisibility { get; set; } = "Transparent";
        public int cellSet { get { return cellSize; } set { cellSize = value; } }
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

            // Initialize Current values for editing selected lamp
            CurrentLampBrightness = 0;
            CurrentLampHeight = 0;
            CurrentLampRadius = 0;

            // Lamps created from the start
            //It generates a collection of LampViewModels
            Lamps = new ObservableCollection<LampViewModel>() {
                 new RoundLampViewModel(new Model.RoundLamp(50,50))
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

            // Sending Lamp values for editing in sidebar
            // TODO .X skal ændres!!!!!!!!!!!!!!!!
            CurrentLampBrightness = Lamp.X;
            CurrentLampHeight = Lamp.Y;
            CurrentLampRadius = Lamp.X;

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

        // Method for moving the lamp. This is created as an on-the-go method, so that each "pixel" 
        // move of the lamp isn't saved in the undo-redo command. 
        // So when undo is pressed, the lamp is moved to it's original position before even moving.
        private void LampMoved(MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {

                var Lamp = TargetLamp(e);
                var MousePosition = RelativeMousePosition(e);

                var offsetX = MousePosition.X - initialMousePosition.X;
                var offsetY = MousePosition.Y - initialMousePosition.Y;
                var newX = initialLampPosition.X + offsetX;
                var newY = initialLampPosition.Y + offsetY;

                if (newX > 0 && newY > 0 && newX < width - cellSize && newY < height - cellSize)
                {
                    if (snapActive)
                    {
                        var extraX = newX % cellSize;
                        var extraY = newY % cellSize;

                        if (extraX > cellSize / 2)
                        {
                            newX = newX - extraX + 0.5 * cellSize;
                        }
                        else
                        {
                            newX = newX - extraX + 0.5 * cellSize;
                        }
                        if (extraY > cellSize / 2)
                        {
                            newY = newY - extraY + 0.5 * cellSize;
                        }
                        else
                        {
                            newY = newY - extraY + 0.5 * cellSize;
                        }
                    }

                    Lamp.X = newX;
                    Lamp.Y = newY;
                }
            }
        }

        // Method for clicking on the canvas - deselects all lamps
        private void CanvasDown(MouseButtonEventArgs e) {

            // We retrieve the mouse position
            var MousePosition = Mouse.GetPosition((FrameworkElement)e.MouseDevice.Target);
            double mouseX = MousePosition.X;
            double mouseY = MousePosition.Y;

            // We unselect all lamps
            if (LampsAreSelected())
            {
                UnSelectAllLamps();
            }

            // Resetting sidebar values after canvas has been pressed and a lamp has been unselected
            CurrentLampBrightness = 0;
            CurrentLampRadius = 0;
            CurrentLampHeight = 0;

            // We add a lamp at the mouse position if add lamp is on
            if (addingLampSelected != null && mouseX > 0 && mouseY > 0
                && mouseX < width - cellSize && mouseY < height - cellSize) {

                // We get the right type of Lamp;
                Type lampType = addingLampSelected.GetType();

                // We get the right type of viewModel of the lamp
                string path = "OptiLight.ViewModel." + addingLampSelected.viewModel;
                Type lampTypeVM = Type.GetType(path, true);

                // We create an instance of the lamp and create a lampViewModel
                object[] args = { mouseX, mouseY };
                object[] argsVM = { (Lamp) Activator.CreateInstance(lampType, args) };
                LampViewModel lampVM = (LampViewModel) Activator.CreateInstance(lampTypeVM, argsVM);

                // We add the lamp
                undoRedoController.AddAndExecute(new AddLamp(Lamps, lampVM));
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

        // Helper method going up the three
        private static T FindParentOfType<T>(DependencyObject o) {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }

        // Method for getting/setting currentLampBrightness
        public double CurrentLampBrightness {
            get { return currentLampBrightness; }
            set { currentLampBrightness = value;
                // TODO .X skal ændres!!
                if (Lamps != null && getCurrentLamp() != null) getCurrentLamp().X = value;
                RaisePropertyChanged();
            }
        }

        // Method for getting/setting currentLampHeight
        public double CurrentLampHeight {
            get { return currentLampHeight; }
            set { currentLampHeight = value;
                // TODO .X skal ændres!!
                if (Lamps != null && getCurrentLamp() != null) getCurrentLamp().Y = value;
                RaisePropertyChanged();
            }
        }

        // Method for getting/setting currentLampRadius
        public double CurrentLampRadius {
            get { return currentLampRadius; }
            set {
                currentLampRadius = value;
                // TODO .X skal ændres!!
                if (Lamps != null && getCurrentLamp() != null) getCurrentLamp().X = value;
                RaisePropertyChanged();
            }
        }

        // Method for getting current selected lamp.
        private LampViewModel getCurrentLamp() {
            foreach (LampViewModel lamp in Lamps) {
                if(lamp.IsSelected) {
                    return lamp;
                }
            }
            return null;
        }
    }
}
