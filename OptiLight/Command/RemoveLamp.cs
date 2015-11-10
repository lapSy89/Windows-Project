using OptiLight.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OptiLight.Command {

    class RemoveLamp : IUndoRedo {
        //The collection of lamps
        private ObservableCollection<RoundLamp> lamps;

        //Holds the removed lamps, so they can be undone
        private List<RoundLamp> removedLamps;

        //Constructor
        public RemoveLamp(ObservableCollection<RoundLamp> lamps, List<RoundLamp> removedLamps) {
            this.lamps = lamps;
            this.removedLamps = removedLamps;
        }

        //For removing the lamp
        public void Execute() {
            removedLamps.ForEach(x => lamps.Remove(x));
        }

        //For undoing the command
        public void UnExecute() {
            removedLamps.ForEach(x => lamps.Add(x));
        }
    }
}
