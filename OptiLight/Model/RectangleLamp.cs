namespace OptiLight.Model {
    public class RectangleLamp : Lamp {

        public override double Height { get; set; } = 50;
        public override double Width { get; set; } = 100;
        public override double VerticalUp { get; set; } = -125;
        public override double VerticalDown { get; set; } = -125;
        public override double HorizontalLeft { get; set; } = -75;
        public override double HorizontalRight { get; set; } = -75;
        public override double VerticalUpConstant { get; } = -125.0 / 60.0;
        public override double VerticalDownConstant { get; } = -125.0 / 60.0;
        public override double HorizontalLeftConstant { get; } = -75.0 / 60.0;
        public override double HorizontalRightConstant { get; } = -75.0 / 60.0;
        public override string name { get; } = "Rectangle Lamp";
        public override string img { get; } = "/Resources/lamp_on.png";
        public override string viewModel { get; } = "RectangleLampViewModel";
    }
}
