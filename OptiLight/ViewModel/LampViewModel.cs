namespace OptiLight.ViewModel {
    public abstract class LampViewModel : BaseViewModel {
    //TODO, consider not to use "RaisePropertyChanged(); but the Notify command instead
    //Figure out where we would do this (This seems like the right place)
        public Model.Lamp Lamp { get; set; }

        
        public double X { get { return Lamp.X; } set { Lamp.X = value; RaisePropertyChanged();} }
        public double Y { get { return Lamp.Y; } set { Lamp.Y = value; RaisePropertyChanged();} }
        public double Width { get { return Lamp.Width; } set { Lamp.Width = value; RaisePropertyChanged(); } }
        public double Height { get { return Lamp.Height; } set { Lamp.Height = value; RaisePropertyChanged(); } }
        public Thickness Radius { get { return new Thickness(Lamp.Radius); } set { Lamp.Radius = value.Top; RaisePropertyChanged(); } }



        //The base means that it inherits 
        public LampViewModel(Model.Lamp lamp) : base() {
            Lamp = lamp;
        }
    }
}
