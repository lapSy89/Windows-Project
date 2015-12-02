using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using OptiLight.Model;
using OptiLight.Command;
using System;
using System.Collections;
//using LampLibrary; // LampLibrary DLL

namespace OptiLight.ViewModel {

    public class MainViewModel : BaseViewModel {

        // The number of pixels a lamp moves with a WASD key press
        private const int ATOMICLENGTH = 10;

        // Points created for having initials positions of Lamp and Mouse when a lamp is moved.
        private Point initialLampPosition;
        private Point initialMousePosition;

        // Is true if a lamp is pressed by the mouse - this way lamp released is only called if this is true
        private bool lampIsPressed { get; set; } = false;
        private bool lampIsKeyMoved { get; set; } = false;

        // The possible Mouse commands
        public ICommand LampPressedCommand { get; }
        public ICommand LampReleasedCommand { get; }
        public ICommand LampMovedCommand { get; }
        public ICommand MouseDownCanvasCommand { get; }

        // The possible arrowkey commands
        public ICommand WASDKeyPressedCommand { get; }
        public ICommand WASDKeyReleasedCommand { get; }

        // Constructor - creates the initial lamps and initializes the commands
        public MainViewModel() : base() {
            // Lamps created from the start
            //It generates a collection of LampViewModels
            Lamps = new ObservableCollection<LampViewModel>() {
                 new RectangleLampViewModel(new RectangleLamp())
            };

            // Commands are defined as relay commands
            LampPressedCommand = new RelayCommand<MouseButtonEventArgs>(LampPressed);
            LampReleasedCommand = new RelayCommand<MouseButtonEventArgs>(LampReleased);
            LampMovedCommand = new RelayCommand<MouseEventArgs>(LampMoved);
            MouseDownCanvasCommand = new RelayCommand<MouseButtonEventArgs>(CanvasDown);

            WASDKeyPressedCommand = new RelayCommand<KeyEventArgs>(WASDKeyPressed);
            WASDKeyReleasedCommand = new RelayCommand<KeyEventArgs>(WASDKeyReleased);
        }
    
        #region Lamp Pressed / Released / Moved

        // Method for capturing the mouse on a lamp
        private void LampPressed(MouseButtonEventArgs e) {
            var Lamp = TargetLamp(e);
            var MousePosition = RelativeMousePosition(e);
            initialLampPosition = new Point(Lamp.X, Lamp.Y);
            initialMousePosition = MousePosition;

            //We unselect already selected lamps
            if (LampsAreSelected()) {
                UnSelectAllLamps();
            }

            // The lamp now knows that it is selected and the sidepanel is showed
            Lamp.IsSelected = true;
            sidePanel.ShowSidePanelBox = Visibility.Visible;

            // Sending Lamp values for editing in sidebar
            sidePanel.CurrentLampBrightness = Lamp.Brightness;
            sidePanel.CurrentLampHeight = Lamp.LampHeight;
            sidePanel.CurrentLampVertRadius = (Lamp.VerticalUp + Lamp.VerticalDown) / 2;
            sidePanel.CurrentLampHoriRadius = (Lamp.HorizontalLeft + Lamp.HorizontalRight) / 2;

            lampIsPressed = true;

            e.MouseDevice.Target.CaptureMouse();
        }

        // Method for releasing the capturing of the mouse on a lamp. After the mouse is released, 
        // the lamps new position is definedand saved to the lamp.
        // The method is only allowed if the lamp was pressed before releasing.
        private void LampReleased(MouseButtonEventArgs e) {
            if (lampIsPressed) {
                var Lamp = TargetLamp(e);
                var MousePosition = RelativeMousePosition(e);

                //Set lamp coordinates to the position it was at when originally pressed
                Lamp.X = initialLampPosition.X;
                Lamp.Y = initialLampPosition.Y;

                //Calculate the movement from the relative mouse position
                var offsetX = MousePosition.X - initialMousePosition.X;
                var offsetY = MousePosition.Y - initialMousePosition.Y;

                // The move command is only added to the undo/redo stack if the lamp is moved and not when
                // it is just selected
                if (offsetX != 0 || offsetY != 0) {
                    //Calculate the new lamp coordinates, based on initial position and the offset
                    var newX = movement(initialLampPosition.X, offsetX, Lamp.Width, canvas.width);
                    var newY = movement(initialLampPosition.Y, offsetY, Lamp.Height, canvas.height);

                    this.undoRedoController.AddAndExecute(new Command.MoveLamp(Lamp, (newX - initialLampPosition.X), (newY - initialLampPosition.Y)));
                }
                e.MouseDevice.Target.ReleaseMouseCapture();
                lampIsPressed = false;
            }
        }

        // Method for moving the lamp. This is created as an on-the-go method, so that each "pixel" 
        // move of the lamp isn't saved in the undo-redo command. 
        // So when undo is pressed, the lamp is moved to it's original position before even moving.
        private void LampMoved(MouseEventArgs e) {
            if (Mouse.Captured != null) {
                var Lamp = TargetLamp(e);
                var MousePosition = RelativeMousePosition(e);

                //The lamp-movement is an offset based on the relative mouse movement
                var offsetX = MousePosition.X - initialMousePosition.X;
                var offsetY = MousePosition.Y - initialMousePosition.Y;

                //Calculate the new lamp coordinates, based on initial position and the offset
                Lamp.X = movement(initialLampPosition.X, offsetX, Lamp.Width, canvas.width);
                Lamp.Y = movement(initialLampPosition.Y, offsetY, Lamp.Height, canvas.height);
            }
        }

        //Calculate a single directional 
        private double movement(double initPos, double offset, double objectDimension, double canvasDimension) {
            //Calculate the new lamp coordinates, based on initial position and the offset
            var newPos = initPos + offset;

            if (canvas.SnapActive) {
                //Calculate default padding to the edge of the canvas 
                //This value will change if snapping is active, according to cellSize
                var padding = objectDimension / 2;

                //"newX + paddingX" is to adjust coordinates from top-left corner to lamp center
                var extra = (newPos + padding) % canvas.cellSize;

                //Calculate a new padding, according to lamp size and cell size
                padding = snapPadding(objectDimension);

                if (extra < canvas.cellSize / 2) {
                    newPos = newPos - extra;
                } else {
                    newPos = newPos - extra + canvas.cellSize;
                }

                if (newPos < 0) {
                    newPos = padding - objectDimension / 2;
                } else if (newPos + objectDimension > canvasDimension) {
                    newPos = canvasDimension - padding - objectDimension / 2;
                }
            } else {
                //Check if new position is within the canvas, before the move is accepted
                if (newPos < 0) {
                    newPos = 0;
                } else if (newPos + objectDimension > canvasDimension) {
                    newPos = canvasDimension - objectDimension;
                }
            }
            return newPos;
        }

        //Auxiliary function to calculate minimum distance to canvas edge
        //when snapping is active
        private double snapPadding(double lampDimension) {
            var padding = 0;
            
            while (padding < lampDimension / 2) {
                padding += canvas.cellSize;
                }
            return padding;
            }

        #endregion Lamp Pressed / Released / Moved

        #region Canvas Pressed

        // Method for clicking on the canvas - deselects all lamps
        private void CanvasDown(MouseButtonEventArgs e) {

            // We retrieve the mouse position
            var MousePosition = Mouse.GetPosition((FrameworkElement)e.MouseDevice.Target);
            double mouseX = MousePosition.X;
            double mouseY = MousePosition.Y;

            // We unselect all lamps and the sidepanel box is collapsed
            if (LampsAreSelected()) {
                UnSelectAllLamps();
                sidePanel.ShowSidePanelBox = Visibility.Collapsed;

                // Resetting sidebar values after canvas has been pressed and a lamp has been unselected
                sidePanel.CurrentLampBrightness = 0;
                sidePanel.CurrentLampHeight = 0;
                sidePanel.CurrentLampVertRadius = 0;
                sidePanel.CurrentLampHoriRadius = 0;
            }

            // We add a lamp at the mouse position if add lamp is on
            if (sidePanel.addingLampSelected != null && mouseX > 0 && mouseY > 0
                && mouseX < canvas.width - canvas.cellSize && mouseY < canvas.height - canvas.cellSize) {

                // We get the right type of Lamp;
                Type lampType = sidePanel.addingLampSelected.GetType();

                // We get the right type of viewModel of the lamp
                string path = "OptiLight.ViewModel." + sidePanel.addingLampSelected.viewModel;
                Type lampTypeVM = Type.GetType(path, true);

                // We create an instance of the lamp and create a lampViewModel
                Lamp lamp = (Lamp)Activator.CreateInstance(lampType);
                lamp.X = mouseX;
                lamp.Y = mouseY;

                object[] argsVM = { lamp };
                LampViewModel lampVM = (LampViewModel)Activator.CreateInstance(lampTypeVM, argsVM);

                // We add the lamp
                undoRedoController.AddAndExecute(new AddLamp(Lamps, lampVM));
            }
        }

        #endregion Canvas Pressed

        #region WASDkey Presses / Released

        // Method for when a WASD key is pressed
        private void WASDKeyPressed(KeyEventArgs e) {
            if (e != null && LampsAreSelected()) {
                LampViewModel Lamp = getSelectedLamps()[0];

                if (!lampIsKeyMoved) {
                    initialLampPosition = new Point(Lamp.X, Lamp.Y);
                    lampIsKeyMoved = true;
                }

                if (e.Key.Equals(Key.W)) {
                    Lamp.Y = keyMovement(Lamp.Y, Lamp.Height, canvas.height, -1.0);
                } else if (e.Key.Equals(Key.A)) {
                    Lamp.X = keyMovement(Lamp.X, Lamp.Width, canvas.width, -1.0);
                } else if (e.Key.Equals(Key.S)) {
                    Lamp.Y = keyMovement(Lamp.Y, Lamp.Height, canvas.height, 1.0);
                } else if (e.Key.Equals(Key.D)) {
                    Lamp.X = keyMovement(Lamp.X, Lamp.Width, canvas.width, 1.0);
                }
            }
        }

        // Method for when a WASD key is released
        private void WASDKeyReleased(KeyEventArgs e) {
            if (e != null && LampsAreSelected()) {
                LampViewModel Lamp = getSelectedLamps()[0];

                double offsetX = Lamp.X - initialLampPosition.X;
                double offsetY = Lamp.Y - initialLampPosition.Y;


                

                if (offsetX != 0 || offsetY != 0) {
                    Lamp.X = initialLampPosition.X;
                    Lamp.Y = initialLampPosition.Y;
                    this.undoRedoController.AddAndExecute(new Command.MoveLamp(Lamp, offsetX, offsetY));
                    lampIsKeyMoved = false; ;
                }
            }
        }

        private double keyMovement(double initPos, double objectDimension, double canvasDimension, double direction) {
            double stepSize;
            if (canvas.SnapActive) {
                stepSize = canvas.cellSize;
            } else {
                stepSize = ATOMICLENGTH;
            }

            var newPos = initPos + stepSize * direction;

            if (canvas.SnapActive) {
                //Calculate default padding to the edge of the canvas 
                //This value will change if snapping is active, according to cellSize
                var padding = objectDimension / 2;

                //"newX + paddingX" is to adjust coordinates from top-left corner to lamp center
                var extra = (newPos + padding) % canvas.cellSize;

                //Calculate a new padding, according to lamp size and cell size
                padding = snapPadding(objectDimension);

                if (extra < canvas.cellSize / 2) {
                    newPos = newPos - extra;
                } else {
                    newPos = newPos - extra + canvas.cellSize;
                }

                if (newPos < 0) {
                    newPos = padding - objectDimension / 2;
                } else if (newPos + objectDimension > canvasDimension) {
                    newPos = canvasDimension - padding - objectDimension / 2;
                }
            } else {
                //Check if new position is within the canvas, before the move is accepted
                if (newPos < 0) {
                    newPos = 0;
                } else if (newPos + objectDimension > canvasDimension) {
                    newPos = canvasDimension - objectDimension;
                }
            }
            return newPos;
        }

        #endregion WASDkey Pressesd / Released

        // Helping method for attaching the mouse to a lamp
        private LampViewModel TargetLamp(MouseEventArgs e) {
            var targetedElement = (FrameworkElement)e.MouseDevice.Target;
            return (LampViewModel)targetedElement.DataContext;
        }
        // Helper method for registration of the position of the mouse.
        private Point RelativeMousePosition(MouseEventArgs e) {
            var targetedElement = (FrameworkElement)e.MouseDevice.Target;
            var canvas = FindParentOfType<System.Windows.Controls.Canvas>(targetedElement);
            return Mouse.GetPosition(canvas);
        }

        // Helper method going up the three
        private static T FindParentOfType<T>(DependencyObject o) {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
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
