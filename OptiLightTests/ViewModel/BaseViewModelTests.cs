using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptiLight.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.ViewModel.Tests {
    [TestClass()]
    public class BaseViewModelTests {
        [TestMethod()]

        //Test that we can add a lamp

        // TODO: Divide commands into different test methods
        public void BaseViewModelTest() {

            //Initialize the Mainmodel
            MainViewModel main = new MainViewModel();

            //Create a lamp that imitates a "Selected Lamp"
            Model.Lamp modelLamp = new Model.RectangleLamp();
            //AddLampCommand takes an IList of type Lamp as an argument
            IList newList = new List<Model.Lamp>();
            //Add the created lamp
            newList.Add(modelLamp);


            //Execute the command with the list containing a selected rectangle Lamp
            
            main.AddLampCommand.Execute(newList);
            Assert.IsTrue(main.AddLampCommand.CanExecute(newList));

            //TODO
            //Check that we have added a lamp to the list
            //In general we should test that the command has had the desired effect


            /*
            Test that all commands are executeable, and that when they execute they do not cause an exception
            Only tests commands that do not take external arguments
            */
           
            main.LightSwitchCommand.Execute(null);
            Assert.IsTrue(main.LightSwitchCommand.CanExecute(null));

            //Works, because it prompts the user to load a drawing (Not feasable in an automatic test)
            //main.LoadDrawingCommand.Execute(null);
            Assert.IsTrue(main.LoadDrawingCommand.CanExecute(null));

            main.NewDrawingCommand.Execute(null);
            Assert.IsTrue(main.NewDrawingCommand.CanExecute(null));

            /*
            Copy Paste
            */

            // Copy does not work without something to copy (TODO)           
            //       main.CopyCommand.Execute(null);
            //       Assert.IsTrue(main.CopyCommand.CanExecute(null));

            // Paste will fail if the clipboard is empty
            //       main.PasteCommand.Execute(null);
            //       Assert.IsTrue(main.PasteCommand.CanExecute(null));

            /*
            Undo Redo Can not work with the Undo Redo stack empty (TODO)
            */

            // main.UndoCommand.Execute(null);
            // Assert.IsTrue(main.UndoCommand.CanExecute(null));

            /*
            Grid Snapping
            */

            main.toggleSnappingCommand.Execute(null);
            Assert.IsTrue(main.toggleSnappingCommand.CanExecute(null));
            main.toggleGridVisibilityCommand.Execute(null);
            Assert.IsTrue(main.toggleGridVisibilityCommand.CanExecute(null));

            /*
            Light
            */
        }
    }
}