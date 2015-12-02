using OptiLight.ViewModel;

namespace OptiLight.Command {
    class ChangeBrightness : IUndoRedo {

        //Variables for the command
        private LampViewModel lamp;
        private double brightnessDifference;

        //Constructor
        public ChangeBrightness(LampViewModel lamp, double brightnessDifference) {
            this.lamp = lamp;
            this.brightnessDifference = brightnessDifference;
        }

        //Method for changing the brightness
        public void Execute() {
            lamp.Brightness = lamp.Brightness + brightnessDifference;
        }

        //Method for unchanging the brightness
        public void UnExecute() {
            lamp.Brightness = lamp.Brightness - brightnessDifference;
        }
    }
}
