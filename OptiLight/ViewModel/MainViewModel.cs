using GalaSoft.MvvmLight.Command;
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

        // Values to keeo track of grid snapping
        private bool snapActive = false;
        public int gridSize = 50;

        public ICommand LampPressedCommand { get; }
        public ICommand LampReleasedCommand { get; }
        public ICommand MoveLampCommand { get; }
        public ICommand toggleSnappingCommand { get; }

        public MainViewModel() : base() {
            // Lamps created from the start
            //It generates a collection of LampViewModels
            Lamps = new ObservableCollection<LampViewModel>() {
                 new RoundLampViewModel(new Model.RoundLamp() { X = 50, Y = 50, Width = 50, Height = 50 }),
                 new RectangleLampViewModel(new Model.RectangleLamp() { X = 100, Y = 50, Width = 50, Height = 50 }),
                 new SquareLampViewModel(new Model.SquareLamp() { X = 200, Y = 100, Width = 50, Height = 50 })
            };

            // Commands are defined as relay commands
          
            LampPressedCommand = new RelayCommand<MouseButtonEventArgs>(MousePressed);
            LampReleasedCommand = new RelayCommand<MouseButtonEventArgs>(MouseReleased);
            MoveLampCommand = new RelayCommand<MouseEventArgs>(MoveLamp);
            toggleSnappingCommand = new RelayCommand(toggleSnapping);
        }

        public void toggleSnapping()
        {
            snapActive = !snapActive;
        }
    
        // Method for capturing the mouse on a lamp
        private void MousePressed(MouseButtonEventArgs e) {
            var Lamp = TargetLamp(e);
            var MousePosition = RelativeMousePosition(e);

            initialLampPosition = new Point(Lamp.X, Lamp.Y);
            initialMousePosition = MousePosition;

            e.MouseDevice.Target.CaptureMouse();
        }

        // Method for releasing the capturing of the mouse on a lamp. After the mouse is released, the lamps new position is defined
        // and saved to the lamp.
        private void MouseReleased(MouseButtonEventArgs e) {
            var Lamp = TargetLamp(e);
            var MousePosition = RelativeMousePosition(e);

            if (MousePosition.X > 0 && MousePosition.Y > 0) {
            Lamp.X = initialLampPosition.X;
            Lamp.Y = initialLampPosition.Y;


            var offsetX = MousePosition.X - initialMousePosition.X;
            var offsetY = MousePosition.Y - initialMousePosition.Y;

            
                if (snapActive) {
                    var extraX = offsetX % gridSize;
                    var extraY = offsetY % gridSize;

                    if (extraX > gridSize / 2) {
                        offsetX = offsetX - extraX + gridSize;
                    }
                    else {
                        offsetX = offsetX - extraX;
                    }
                    if (extraY > gridSize / 2) {
                        offsetY = offsetY - extraY + gridSize;
                    }
                    else {
                        offsetY = offsetY - extraY;
                    }
                }



                new Command.MoveLamp(Lamp, offsetX, offsetY).Execute();
            }
            e.MouseDevice.Target.ReleaseMouseCapture();
        }

        // Method for moving the lamp. This is created as an on-the-go method, so that each "pixel" move of the lamp isn't saved in the undo-redo
        // command. So when undo is pressed, the lamp is moved to it's original position before even moving.
        private void MoveLamp(MouseEventArgs e) {
            if (Mouse.Captured != null)
            {
                var Lamp = TargetLamp(e);
                var MousePosition = RelativeMousePosition(e);

                var offsetX = MousePosition.X - initialMousePosition.X;
                var offsetY = MousePosition.Y - initialMousePosition.Y;
                var newX = initialLampPosition.X + offsetX;
                var newY = initialLampPosition.Y + offsetY;

                if (newX > 0 && newY > 0){
                    if (snapActive){
                        var extraX = newX % gridSize;
                        var extraY = newY % gridSize;

                        if (extraX > gridSize / 2) {
                            newX = newX - extraX + gridSize;
                        } else {
                            newX = newX - extraX;
                        }
                        if (extraY > gridSize / 2) {
                            newY = newY - extraY + gridSize;
                        } else {
                            newY = newY - extraY;
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
