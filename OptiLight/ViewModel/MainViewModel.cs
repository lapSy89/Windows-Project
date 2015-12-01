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

        // Points created for having initials positions of Lamp and Mouse when a lamp is moved.
        private Point initialLampPosition;
        private Point initialMousePosition;

        // Is true if a lamp is pressed by the mouse - this way lamp released is only called if this is true
        private bool lampIsPressed { get; set; } = false;

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
            sidePanel.CurrentLampVertRadius = Lamp.VerticalUp + Lamp.VerticalDown;
            sidePanel.CurrentLampHoriRadius = Lamp.HorizontalLeft + Lamp.HorizontalRight;

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

                Lamp.X = initialLampPosition.X;
                Lamp.Y = initialLampPosition.Y;

                var offsetX = MousePosition.X - initialMousePosition.X;
                var offsetY = MousePosition.Y - initialMousePosition.Y;

                //Calculate the new lamp coordinates, based on initial position and the offset
                var newX = initialLampPosition.X + offsetX;
                var newY = initialLampPosition.Y + offsetY;

                if (canvas.SnapActive) {
                    //"+ Lamp.[Width/Heigh] / 2" is to adjust coordinates from top-left corner to lamp center
                    var extraX = (newX + Lamp.Width / 2) % canvas.cellSize;
                    var extraY = (newY + Lamp.Height / 2) % canvas.cellSize;

                    if (extraX < canvas.cellSize / 2) {
                        newX = newX - extraX;
                    } else {
                        newX = newX - extraX + canvas.cellSize;
                    }
                    if (extraY < canvas.cellSize / 2) {
                        newY = newY - extraY;
                    } else {
                        newY = newY - extraY + canvas.cellSize;
                    }

                    if (newX < 0) {
                        newX = canvas.cellSize - Lamp.Width / 2;
                        while (newX < 0) {
                            newX += canvas.cellSize;
                        }
                    } else if (newX + Lamp.Width > canvas.width) {
                        newX = canvas.width - canvas.cellSize - Lamp.Width / 2;
                        while (newX + Lamp.Width > canvas.width) {
                            newX -= canvas.cellSize;
                        }
                    }

                    if (newY < 0) {
                        newY = canvas.cellSize - Lamp.Height / 2;
                        while (newY < 0) {
                            newY += canvas.cellSize;
                        }
                    } else if (newY + Lamp.Height > canvas.height) {
                        newY = canvas.height - canvas.cellSize - Lamp.Height / 2;
                        while (newY + Lamp.Height > canvas.height) {
                            newY -= canvas.cellSize;
                        }
                    }
                } else {
                    //Check if new position is within the canvas, before the move is accepted
                    if (newX < 0) {
                        newX = 0;
                    } else if (newX + Lamp.Width > canvas.width) {
                        newX = canvas.width - Lamp.Width;
                    }

                    if (newY < 0) {
                        newY = 0;
                    } else if (newY + Lamp.Height >= canvas.height) {
                        newY = canvas.height - Lamp.Height;
                    }
                }

                // The move command is only added to the undo/redo stack if the lamp is moved and not when
                // it is just selected
                if (offsetX != 0 || offsetY != 0) {
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
                var newX = initialLampPosition.X + offsetX;
                var newY = initialLampPosition.Y + offsetY;

                //Calculate coordinates according to specified grid, if snapping is active
                if (canvas.SnapActive) {
                    //"+ Lamp.[Width/Heigh] / 2" is to adjust coordinates from top-left corner to lamp center
                    var extraX = (newX + Lamp.Width / 2) % canvas.cellSize;
                    var extraY = (newY + Lamp.Height / 2) % canvas.cellSize;

                    if (extraX < canvas.cellSize / 2) {
                        newX = newX - extraX;
                        }
                        else {
                        newX = newX - extraX + canvas.cellSize;
                        }
                    if (extraY < canvas.cellSize / 2) {
                        newY = newY - extraY;
                        }
                        else {
                        newY = newY - extraY + canvas.cellSize;
                        }

                    if (newX < 0) {
                        newX = canvas.cellSize - Lamp.Width / 2;
                        while (newX < 0) {
                            newX += canvas.cellSize;
                        }
                    }
                    else if (newX + Lamp.Width > canvas.width) {
                        newX = canvas.width - canvas.cellSize - Lamp.Width / 2;
                        while (newX + Lamp.Width > canvas.width) {
                            newX -= canvas.cellSize;
                        }
                    }

                    if (newY < 0) {
                        newY = canvas.cellSize - Lamp.Height / 2;
                        while (newY < 0) {
                            newY += canvas.cellSize;
                        }
                    }
                    else if (newY + Lamp.Height > canvas.height) {
                        newY = canvas.height - canvas.cellSize - Lamp.Height / 2;
                        while (newY + Lamp.Height > canvas.height) {
                            newY -= canvas.cellSize;
                        }
                    }
                }
                else {
                    //Check if new position is within the canvas, before the move is accepted
                    if (newX < 0) {
                        newX = 0;
                    }
                    else if (newX + Lamp.Width > canvas.width) {
                        newX = canvas.width - Lamp.Width;
                    }

                    if (newY < 0) {
                        newY = 0;
                    }
                    else if (newY + Lamp.Height >= canvas.height) {
                        newY = canvas.height - Lamp.Height;
                    }
                }

                    Lamp.X = newX;
                    Lamp.Y = newY;
                }
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
            if (LampsAreSelected())
            {
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
                LampViewModel lampVM = (LampViewModel) Activator.CreateInstance(lampTypeVM, argsVM);

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

                // move but not with undo redo

                if (e.Key.Equals(Key.W)) {
                    
                } else if (e.Key.Equals(Key.A)) {
                    
                } else if (e.Key.Equals(Key.S)) {
                    
                } else if (e.Key.Equals(Key.D)) {
                    
                }
            }
        }

        // Method for when a WASD key is released
        private void WASDKeyReleased(KeyEventArgs e) {
            if (e != null && LampsAreSelected()) {
                LampViewModel Lamp = getSelectedLamps()[0];

                if (e.Key.Equals(Key.W)) {
                    this.undoRedoController.AddAndExecute(new Command.MoveLamp(Lamp, 0.0, -10.0));
                } else if (e.Key.Equals(Key.A)) {
                    this.undoRedoController.AddAndExecute(new Command.MoveLamp(Lamp, -10.0, 0.0));
                } else if (e.Key.Equals(Key.S)) {
                    this.undoRedoController.AddAndExecute(new Command.MoveLamp(Lamp, 0.0, 10.0));
                } else if (e.Key.Equals(Key.D)) {
                    this.undoRedoController.AddAndExecute(new Command.MoveLamp(Lamp, 10.0, 0.0));
                }
            }
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
    }
}
