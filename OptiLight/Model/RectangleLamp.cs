namespace OptiLight.Model {
    public class RectangleLamp : Lamp {

        public RectangleLamp(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string name { get; } = "Rectangle Lamp";
        public override string img { get; } = "/Resources/lamp_on.png";
        public override string viewModel { get; } = "RectangleLampViewModel";
    }
}
