using OptiLight.Model;
using OptiLight.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OptiLight.Command {

    class RemoveLamp : IUndoRedo {
        //The collection of lamps
        private ObservableCollection<LampViewModel> lamps;

        //Holds the removed lamps, so they can be undone
        private List<LampViewModel> removedLamps;

        //Constructor
        public RemoveLamp(ObservableCollection<LampViewModel> lamps, List<LampViewModel> removedLamps) {
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
