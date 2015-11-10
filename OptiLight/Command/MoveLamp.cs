using OptiLight.ViewModel;

namespace OptiLight.Command {

    class MoveLamp : IUndoRedo {
        
        // Global variables used for changing lamps position
        private LampViewModel lamp;
        private double v1;
        private double v2;

        // Constructer method for setting global variables to here-and-now data
  
        public MoveLamp(LampViewModel lamp, double v1, double v2) {
            this.lamp = lamp;
            this.v1 = v1;
            this.v2 = v2;
        }

        // Method for changing the position of the lamp
        public void Execute() {
            lamp.X += v1;
            lamp.Y += v2;
        }

        // Method for undoing change in position
        public void UnExecute() {
            lamp.X -= v1;
            lamp.Y -= v2;
        }
    }
}
