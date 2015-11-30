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
        public double VerticalUp { get { return Lamp.VerticalUp; } set { Lamp.VerticalUp = value; RaisePropertyChanged(); } }
        public double VerticalDown { get { return Lamp.VerticalDown; } set { Lamp.VerticalDown = value; RaisePropertyChanged(); } }
        public double HorizontalLeft { get { return Lamp.HorizontalLeft; } set { Lamp.HorizontalLeft = value; RaisePropertyChanged(); } }
        public double HorizontalRight { get { return Lamp.HorizontalRight; } set { Lamp.HorizontalRight = value; RaisePropertyChanged(); } }
        public double VerticalUpConstant { get { return Lamp.VerticalUpConstant; } }
        public double VerticalDownConstant { get { return Lamp.VerticalDownConstant; } }
        public double HorizontalLeftConstant { get { return Lamp.HorizontalLeftConstant; } }
        public double HorizontalRightConstant { get { return Lamp.HorizontalRightConstant; } }


        public Thickness Radius { get { return new Thickness(HorizontalLeft, VerticalUp, HorizontalRight, VerticalDown); }
                                  set {
                HorizontalLeft = value.Left; VerticalUp = value.Top; HorizontalRight = value.Right; VerticalDown = value.Bottom;
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
            VerticalUp = Brightness * LampHeight * VerticalUpConstant;
            VerticalDown = Brightness * LampHeight * VerticalDownConstant;
            HorizontalLeft = Brightness * LampHeight * HorizontalLeftConstant;
            HorizontalRight = Brightness * LampHeight * HorizontalRightConstant;
            YellowOffset = (Brightness / LampHeight) * 0.02;
            TransparentOffset = (Brightness / LampHeight) * 0.1133;
        }

        //Offsets
        private double yellowOffset = 0.3;
        public double YellowOffset {
            get { return yellowOffset; }
            set { yellowOffset = value; RaisePropertyChanged(); }
        }

        private double transparentOffset = 1.7;
        public double TransparentOffset {
            get { return transparentOffset; }
            set { transparentOffset = value; RaisePropertyChanged(); }
        }

        //The base means that it inherits Lamp
        public LampViewModel(Lamp lamp){
            Lamp = lamp;
        }
    }
}
