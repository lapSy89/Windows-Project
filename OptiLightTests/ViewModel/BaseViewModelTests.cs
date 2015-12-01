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
        }
    }
}