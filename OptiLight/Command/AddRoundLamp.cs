using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Command {

    class AddRoundLamp {

        // Global variables for adding lamp to the collection of lamps
        private ObservableCollection<Model.RoundLamp> RoundLamps;
        private Model.RoundLamp RoundLamp;

        // Constructor for setting the global variables
        public AddRoundLamp(ObservableCollection<Model.RoundLamp> RoundLamps, Model.RoundLamp RoundLamp) {
            this.RoundLamps = RoundLamps;
            this.RoundLamp = RoundLamp;
        }

        // Method for adding lamp to the collection
        public void Execute() {
            RoundLamps.Add(RoundLamp);
        }

        // Method for removing the lamp from the collection
        public void UnExecute() {
            RoundLamps.Remove(RoundLamp);
        }
    }
}
