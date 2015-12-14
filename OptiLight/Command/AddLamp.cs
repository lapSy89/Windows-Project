using System.Collections.ObjectModel;

namespace OptiLight.Command {

    public class AddLamp : IUndoRedo {

        // Global variables for adding lamp to the collection of lamps
        private ObservableCollection<ViewModel.LampViewModel> lampCollection;
        private ViewModel.LampViewModel lamp;

        // Constructor for setting the global variables
        public AddLamp(ObservableCollection<ViewModel.LampViewModel> lampCollection, ViewModel.LampViewModel lamp) {
            this.lampCollection = lampCollection;
            this.lamp = lamp;
        }

        // Method for adding lamp to the collection
        public void Execute() {
            lampCollection.Add(lamp);
        }

        // Method for removing the lamp from the collection
        public void UnExecute() {
            lampCollection.Remove(lamp);
        }
    }
}
