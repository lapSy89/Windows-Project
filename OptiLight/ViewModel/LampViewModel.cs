using System.Windows;
using System.Windows.Media;
using OptiLight.Model;
//using LampLibrary; // LampLibrary DLL


namespace OptiLight.ViewModel {
    public abstract class LampViewModel : BaseViewModel {

        public Lamp Lamp { get; set; }

        public double X { get { return Lamp.X; } set { Lamp.X = value; RaisePropertyChanged();} }
        public double Y { get { return Lamp.Y; } set { Lamp.Y = value; RaisePropertyChanged();} }
        public double Width { get { return Lamp.Width; } set { Lamp.Width = value; RaisePropertyChanged(); } }
        public double Height { get { return Lamp.Height; } set { Lamp.Height = value; RaisePropertyChanged(); } }
        public string Name { get { return Lamp.name; } }
        public double Vertical { get { return Lamp.Vertical; } set { Lamp.Vertical = value; RaisePropertyChanged(); } }
        public double Horizontal { get { return Lamp.Horizontal; } set { Lamp.Horizontal = value; RaisePropertyChanged(); } }


        public Thickness Radius { get { return new Thickness(Vertical, Horizontal, Vertical, Horizontal); }
                                  set { Vertical = value.Top; Horizontal = value.Left; Vertical = value.Bottom; Horizontal = value.Right;
                                        RaisePropertyChanged(); } }

        // Values for displaying in sidebar
        public double Brightness { get { return Lamp.Brightness; }
                                   set { Lamp.Brightness = value;
                                         changeLightRadius();
                                         RaisePropertyChanged();
                                         RaisePropertyChanged(() => Radius); }
        }
        public double LampHeight { get { return Lamp.LampHeight; }
                                   set { Lamp.LampHeight = value;
                                         changeLightRadius();
                                         RaisePropertyChanged();
                                         RaisePropertyChanged(() => Radius);
            }
        }

        //Determines whether a lamp is selected or not
        private bool isSelected;
        public bool IsSelected{
            get { return this.isSelected; }
            set { this.isSelected = value; RaisePropertyChanged(); RaisePropertyChanged(() => SelectedColor); }
        }

        // Determines whether a lamp is turned on or not
        private bool isTurnedOn = false;
        public bool IsTurnedOn {
            get { return this.isTurnedOn; }
            set { this.isTurnedOn = value; RaisePropertyChanged(); RaisePropertyChanged(() => TurnedOnColor); }
        }

        //Colors
        public Brush SelectedColor => IsSelected ? Brushes.Blue : Brushes.Transparent;
        public Color TurnedOnColor => IsTurnedOn ? Colors.Transparent : Colors.Yellow;
   
        //Change lightradius
        public void changeLightRadius() {
            Vertical = LampHeight * (-50);
            Horizontal = LampHeight * (-50);
        }

        //The base means that it inherits Lamp
        public LampViewModel(Lamp lamp){
            Lamp = lamp;
        }
    }
}
