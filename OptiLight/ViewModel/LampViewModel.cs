using System.Windows;
using System.Windows.Media;
using OptiLight.Model;
//using LampLibrary; // LampLibrary DLL


namespace OptiLight.ViewModel {
    public abstract class LampViewModel : BaseViewModel {
    //TODO, consider not to use "RaisePropertyChanged(); but the Notify command instead
    //Figure out where we would do this (This seems like the right place)
        public Lamp Lamp { get; set; }

        public double X { get { return Lamp.X; } set { Lamp.X = value; RaisePropertyChanged();} }
        public double Y { get { return Lamp.Y; } set { Lamp.Y = value; RaisePropertyChanged();} }
        public double Width { get { return Lamp.Width; } set { Lamp.Width = value; RaisePropertyChanged(); } }
        public double Height { get { return Lamp.Height; } set { Lamp.Height = value; RaisePropertyChanged(); } }
        public string Name { get { return Lamp.name; } }


        public Thickness Radius { get { return new Thickness(Lamp.Vertical, Lamp.Horizontal, Lamp.Vertical, Lamp.Horizontal); }
                                  set { Lamp.Vertical = value.Top; Lamp.Horizontal = value.Left; Lamp.Vertical = value.Bottom; Lamp.Horizontal = value.Right;
                                        RaisePropertyChanged(); } }

        //Determines whether a lamp is selected or not
        private bool isSelected;
        public bool IsSelected{
            get { return this.isSelected; }
            set { this.isSelected = value; RaisePropertyChanged(); RaisePropertyChanged(() => SelectedColor); }
        }

        private bool isTurnedOn = false;
        public bool IsTurnedOn {
            get { return this.isTurnedOn; }
            set { this.isTurnedOn = value; RaisePropertyChanged();RaisePropertyChanged(() => TurnedOnColor); }
        }

        //Colors
        public Brush SelectedColor => IsSelected ? Brushes.Blue : Brushes.Transparent;
        public Color TurnedOnColor => IsTurnedOn ? Colors.Transparent : Colors.Yellow;
   

        //The base means that it inherits 
        public LampViewModel(Lamp lamp){
            Lamp = lamp;
        }
    }
}
