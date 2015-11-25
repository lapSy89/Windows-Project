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
        public Thickness Radius { get { return new Thickness(Lamp.Radius); } set { Lamp.Radius = value.Top; RaisePropertyChanged(); } }

        //Determines whether a lamp is selected or not
        private bool isSelected;
        public bool IsSelected{
            get { return this.isSelected; }
            set { this.isSelected = value; RaisePropertyChanged(); RaisePropertyChanged(() => SelectedColor); }
        }

        //Colors
        public Brush SelectedColor => IsSelected ? Brushes.Blue : Brushes.Transparent;
        //The base means that it inherits 
        public LampViewModel(Lamp lamp) : base() {
            Lamp = lamp;
        }
    }
}
