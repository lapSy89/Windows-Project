using OptiLight.ViewModel;

namespace OptiLight.Command {
    class ChangeHeight : IUndoRedo {

        //Variables for the command
        private LampViewModel lamp;
        private double heightDifference;

        //Constructor
        public ChangeHeight(LampViewModel lamp, double heightDifference) {
            this.lamp = lamp;
            this.heightDifference = heightDifference;
        }

        //Method for changing the height
        public void Execute() {
            lamp.LampHeight = lamp.LampHeight + heightDifference;
        }

        //Method for unchanging the height
        public void UnExecute() {
            lamp.LampHeight = lamp.LampHeight - heightDifference;
        }
    }
}
