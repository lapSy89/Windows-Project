using OptiLight.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
