using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Command {

    class AddLamp {

        // Global variables for adding lamp to the collection of lamps
        private ObservableCollection<Model.Lamp> Lamps;
        private Model.Lamp Lamp;

        // Constructor for setting the global variables
        public AddLamp(ObservableCollection<Model.Lamp> Lamps, Model.Lamp Lamp) {
            this.Lamps = Lamps;
            this.Lamp = Lamp;
        }

        // Method for adding lamp to the collection
        public void Execute() {
            Lamps.Add(Lamp);
        }

        // Method for removing the lamp from the collection
        public void UnExecute() {
            Lamps.Remove(Lamp);
        }
    }
}
