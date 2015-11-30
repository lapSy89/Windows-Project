using OptiLight.ViewModel;

namespace OptiLight.Command {
    class ToggleLightOnLamp : IUndoRedo {

        // Variables for the command
        private LampViewModel lamp;
        private bool LightsOn;

        // Constructor
        public ToggleLightOnLamp(LampViewModel lamp, bool lightsOn) {
            this.lamp = lamp;
            this.LightsOn = lightsOn;
        }

        // Method for toggling the lights
        public void Execute() {
            lamp.IsTurnedOn = LightsOn;
        }

        // Method for toggling the lights back
        public void UnExecute() {
            lamp.IsTurnedOn = !LightsOn;
        }
    }
}
