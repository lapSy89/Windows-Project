namespace OptiLight.Model {
    public class RoundLamp : Lamp {
        public override string name { get; } = "Round Lamp";
        public override string img { get; } = "/Resources/lamp_off.png";
        public override string viewModel { get; } = "RoundLampViewModel";
    }
}
