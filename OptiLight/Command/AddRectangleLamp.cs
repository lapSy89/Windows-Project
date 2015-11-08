using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Command
{

    class AddRectangleLamp
    {

        // Global variables for adding lamp to the collection of lamps
        private ObservableCollection<Model.RectangleLamp> RectangleLamps;
        private Model.RectangleLamp RectangleLamp;

        // Constructor for setting the global variables
        public AddRectangleLamp(ObservableCollection<Model.RectangleLamp> RectangleLamps, Model.RectangleLamp RectangleLamp)
        {
            this.RectangleLamps = RectangleLamps;
            this.RectangleLamp = RectangleLamp;
        }

        // Method for adding lamp to the collection
        public void Execute()
        {
            RectangleLamps.Add(RectangleLamp);
        }

        // Method for removing the lamp from the collection
        public void UnExecute()
        {
            RectangleLamps.Remove(RectangleLamp);
        }
    }
}
