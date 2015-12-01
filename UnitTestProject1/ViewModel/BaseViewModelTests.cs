using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptiLight.Model;
using OptiLight.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.ViewModel.Tests {
    [TestClass()]
    public class BaseViewModelTests {
        public SidePanelViewModel sidePanel { get; } = SidePanelViewModel.Instance;

        [TestMethod()]
        public void AddNewLampTest() {


            //We either choose a type of lamp to add or stop adding
            if (sidePanel.addingLampSelected == null) {
               
            //    sidePanel.addingLampSelected = selectedLamp;
            } else if (sidePanel.addingLampSelected.name.Equals(selectedLamp.name)) {
            //    sidePanel.addingLampSelected = null;
            //    sidePanel.AddingColor = Colors.Transparent;
            } else {
            //    sidePanel.addingLampSelected = selectedLamp;
            //    sidePanel.AddingColor = Colors.DarkGray;
            }


            Assert.Fail();
        }
    }
}