using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Command
{

    class MoveLamp
    {
        // Global variables used for changing lamps position
        private Model.RoundLamp RoundLamp;
        private double changeX;
        private double changeY;

        // Constructer method for setting global variables to here-and-now data
        public MoveLamp(Model.RoundLamp RoundLamp, double changeX, double changeY)
        {
            this.RoundLamp = RoundLamp;
            this.changeX = changeX;
            this.changeY = changeY;
        }

        // Method for changing the position of the lamp
        public void Execute()
        {
            RoundLamp.X += changeX;
            RoundLamp.Y += changeY;
        }

        // Method for undoing change in position
        public void UnExecute()
        {
            RoundLamp.X -= changeX;
            RoundLamp.Y -= changeY;
        }
    }
}
