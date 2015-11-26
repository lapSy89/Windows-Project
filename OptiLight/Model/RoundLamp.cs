namespace OptiLight.Model {
    public class RoundLamp : Lamp {

        public RoundLamp(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string name { get; } = "Round Lamp";
        public override string img { get; } = "/Resources/lamp_off.png";
        public override string viewModel { get; } = "RoundLampViewModel";
    }
}
