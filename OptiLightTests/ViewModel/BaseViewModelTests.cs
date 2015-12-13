using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using LampLibrary;
using System.Windows.Input;

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
            Lamp modelLamp = new RectangleLamp();
            //AddLampCommand takes an IList of type Lamp as an argument
            IList newList = new List<Lamp>();
            //Add the created lamp
            newList.Add(modelLamp);


            //Execute the command with the list containing a selected rectangle Lamp
            main.AddLampCommand.Execute(newList);
            Assert.IsTrue(main.AddLampCommand.CanExecute(newList));

            //Now click the canvas
            MouseButtonEventArgs e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            main.MouseDownCanvasCommand.Execute(e);

            /*
            Is not possible to test, as we can not manipulate mouseX and mouseY from the testClass
            */
            //Assert.IsTrue(main.getLampCount == 1);
            //Check that we have added a lamp to the list
            //In general we should test that the command has had the desired effect


            /*----------------------------------------------------------------------------------------------------
            | Test that all commands are executeable, and that when they execute they do not cause an exception   |
            |                                                                                                     |
            ----------------------------------------------------------------------------------------------------*/

            /*
            Light Switch (Done)
            */
            Assert.IsTrue(main.LightSwitchCommand.CanExecute(null));
            main.LightSwitchCommand.Execute(null);
            //Lights are now off
            Assert.IsTrue(main.LightsOn == false);
            main.LightSwitchCommand.Execute(null);
            //Lights are now on again
            Assert.IsTrue(main.LightsOn == true);

            /*
            Load Save
            */
            //Works, because it prompts the user to load a drawing (Not feasable in an automatic test)
            //main.LoadDrawingCommand.Execute(null);
            Assert.IsTrue(main.LoadDrawingCommand.CanExecute(null));
            main.NewDrawingCommand.Execute(null);
            Assert.IsTrue(main.NewDrawingCommand.CanExecute(null));
            //TODO check some parameters that apply when a new drawing is loaded

            /*
            Grid Snapping (Done)
            */
            //Assert False before we activate the visibility and Snap
            Assert.IsFalse(main.canvas.SnapActive == true);
            Assert.IsFalse(main.canvas.visibility == "Black");

            main.toggleSnappingCommand.Execute(null);
            Assert.IsTrue(main.toggleSnappingCommand.CanExecute(null));
            main.toggleGridVisibilityCommand.Execute(null);
            Assert.IsTrue(main.toggleGridVisibilityCommand.CanExecute(null));
            //Assert True after we have activated the visibility and Snap
            Assert.IsTrue(main.canvas.SnapActive == true);
            Assert.IsTrue(main.canvas.visibility == "Black");

            /*
            Cut Copy Paste
            */

            // Copy does not work without something to copy (TODO)           
            //       main.CopyCommand.Execute(null);
            //       Assert.IsTrue(main.CopyCommand.CanExecute(null));

            // Paste will fail if the clipboard is empty
            //       main.PasteCommand.Execute(null);
            //       Assert.IsTrue(main.PasteCommand.CanExecute(null));

            // Cut does not work without something to cut (TODO)
            //       main.CutCommand.Execute(null);
            //       Assert.IsTrue(main.CutCommand.CanExecute(null));

            /*
            Undo Redo Can not work with the Undo Redo stack empty (TODO)
            */

            //  main.UndoCommand.Execute(null);
            // Assert.IsTrue(main.UndoCommand.CanExecute(null));


        }
    }
}