namespace OptiLight.Model {
    public class SquareLamp : Lamp {

        public SquareLamp(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string name { get; } = "Square Lamp";
        public override string img { get; } = "/Resources/lamp_on.png";
        public override string viewModel { get; } = "SquareLampViewModel";
    }
}
