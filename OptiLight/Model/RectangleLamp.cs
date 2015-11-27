namespace OptiLight.Model {
    public class RectangleLamp : Lamp {
        public override string name { get; } = "Rectangle Lamp";
        public override string img { get; } = "/Resources/lamp_on.png";
        public override string viewModel { get; } = "RectangleLampViewModel";
    }
}
