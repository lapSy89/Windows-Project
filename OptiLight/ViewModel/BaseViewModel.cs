using OptiLight.Command;
using OptiLight.View;
using OptiLight.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Linq;
using System;
using LampLibrary; // LampLibrary DLL

namespace OptiLight.ViewModel {

    // Implements the Galasoft ViewModelBase
    public abstract class BaseViewModel : ViewModelBase {

        //We initialize the other viewModels as singletons
        public CanvasViewModel canvas { get; } = CanvasViewModel.Instance;
        public SidePanelViewModel sidePanel { get; } = SidePanelViewModel.Instance;

        // The undoRedoController is initialized as a singleton
        protected UndoRedoController undoRedoController = UndoRedoController.Instance;

        // All the single lamps, all the single lamps, all the single lamps, all the single lamps, throw your light up!
        public static ObservableCollection<LampViewModel> Lamps { get; set; }

        public int getLampCount { get; set; }

        // Contains a copy of all current types of lamps - used in the sidepanel
        public static List<Lamp> lampTypes { get; } = Lamp.lampTypes;

        // Dialog windows for New, Load and Save
        public DialogViews dialogWindow { get; set; }

        // Path for saving the file, if file is already saved.
        private static string savedPath = null;

        // Whether the global lights of the lamps are on of off
        private bool lightsOn = true;
        public bool LightsOn {
            get { return lightsOn; }
            set { lightsOn = value; RaisePropertyChanged(); }
        }

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public ICommand CutCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand PasteCommand { get; set; }

        public ICommand AddLampCommand { get; set; }
        public ICommand RemoveLampCommand { get; }

        public ICommand NewDrawingCommand { get; }
        public ICommand SaveDrawingCommand { get; }
        public ICommand LoadDrawingCommand { get; }
        public ICommand SaveAsDrawingCommand { get; }

        public ICommand LightSwitchCommand { get; }
        public ICommand SwitchLampLightCommand { get; }

        public ICommand toggleSnappingCommand { get; }
        public ICommand toggleGridVisibilityCommand { get; }

        //Constructor 
        public BaseViewModel() {

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            CutCommand = new RelayCommand(Cut, LampsAreSelected);
            CopyCommand = new RelayCommand(Copy, LampsAreSelected);
            PasteCommand = new RelayCommand(Paste);

            AddLampCommand = new RelayCommand<IList>(AddNewLamp);
            RemoveLampCommand = new RelayCommand(RemoveLamp, LampsAreSelected);

            dialogWindow = new DialogViews();
            NewDrawingCommand = new RelayCommand(NewDrawing);
            LoadDrawingCommand = new RelayCommand(LoadDrawing);
            SaveDrawingCommand = new RelayCommand(SaveDrawing);
            SaveAsDrawingCommand = new RelayCommand(SaveAsDrawing);
        
            LightSwitchCommand = new RelayCommand(LightSwitch);
            SwitchLampLightCommand = new RelayCommand(singleLampLightSwitch, LampsAreSelected);

            toggleSnappingCommand = new RelayCommand(toggleSnapping);
            toggleGridVisibilityCommand = new RelayCommand(toggleVisibility);
        }

        #region New / Save / Load
        // Method for making a new drawing
        private void NewDrawing() {
            // Check if changes are made to the drawing
            if (!undoRedoController.drawingIsSaved) {
                // Pop up window for confirming deleting of changes.
                if (dialogWindow.NewFile()) {
                    // Deleting lamps
                    clearWorkspace();
                    Lamps.Clear();
                    savedPath = null;
                    undoRedoController.drawingIsSaved = true;
                }
            } else {
                clearWorkspace();
                Lamps.Clear();
                savedPath = null;
                undoRedoController.drawingIsSaved = true;
            }
        }


        // Method for saving drawing
        private void SaveDrawing() {

            // Path for saving the file
            if (savedPath == null) {
                string savePath = dialogWindow.SaveFile();
                if (savePath != null) {

                    // Saving the file.
                    XML.Instance.AsyncSaveToFile(getCurrentSetup(), savePath);
                    savedPath = savePath;
                    undoRedoController.drawingIsSaved = true;
                }
            } else {
                XML.Instance.AsyncSaveToFile(getCurrentSetup(), savedPath);
                undoRedoController.drawingIsSaved = true;
            }
        }

        // Method for saving as drawing
        private void SaveAsDrawing() {
            string savePath = dialogWindow.SaveFile();
            if (savePath != null) {

                XML.Instance.AsyncSaveToFile(getCurrentSetup(), savePath);
                savedPath = savePath;
            }
        }

        // Method for loading drawing
        private async void LoadDrawing() {
            string loadPath;
            if (!undoRedoController.drawingIsSaved) {
                loadPath = dialogWindow.OpenFile(true);  
            } else {
                loadPath = dialogWindow.OpenFile(false);
            }

            if (loadPath != null) {
                // Get list of lamps
                Setup setup = await XML.Instance.AsyncOpenFromFile(loadPath);

                // If there is an error in opening the file.
                if(setup == null || setup.Lamps == null) {
                    dialogWindow.popUpError();
                } else {
                    // Clear the board for loading new lamps
                    Lamps.Clear();
                    savedPath = loadPath;
                    undoRedoController.drawingIsSaved = true;

                    // The canvas is sized properly
                    canvas.cellSize = setup.cellSize;
                    canvas.cellsX = setup.cellsX;
                    canvas.cellsY = setup.cellsX;

                    // Inserting lamps into array of lamps
                    foreach (Lamp lamp in setup.Lamps) {
                        string path = "OptiLight.ViewModel." + lamp.viewModel;
                        Type lampTypeVM = Type.GetType(path, true);
                        object[] argsVM = { lamp };
                        LampViewModel lampVM = (LampViewModel)Activator.CreateInstance(lampTypeVM, argsVM);
                        Lamps.Add(lampVM);
                    }
                    clearWorkspace();
                }
            }
        }

        // We clear the workspace for loading or new workspace
        private void clearWorkspace() {
            undoRedoController.ClearStacks();
            sidePanel.addingLampSelected = null;
            sidePanel.AddingColor = Colors.Transparent;
        }

        // We retrive the file to save
        private Setup getCurrentSetup() {
            Setup setup = new Setup();
            setup.cellSize = canvas.cellSize;
            setup.cellsX = canvas.cellsX;
            setup.cellsY = canvas.cellsY;
            setup.Lamps = Lamps.Select(x => x.Lamp).ToList();
            return setup;
        }

        #endregion New / Save / Load

        #region Cut / Copy / Paste

        // The selected lamps are removed and moved to the clipboard as xml
        private async void Cut() {
            var selectedLamps = getSelectedLamps();
            RemoveLamp();
            var xml = await XML.Instance.AsyncSerializeToString(selectedLamps.Select(lamp => lamp.Lamp).ToList());
            Clipboard.SetText(xml);
        }

        // The selected lamps are copied to clipboard as xml
        private async void Copy() {
            var xml = await XML.Instance.AsyncSerializeToString(getSelectedLamps().Select(lamp => lamp.Lamp).ToList());
            Clipboard.SetText(xml);
        }

        // The lamps in the clipboard are pasted
        private async void Paste() {
            // We retrieve the xml from clipboard and deserialize into Lamps
            var xml = Clipboard.GetText();
            List<Lamp> lamps = await XML.Instance.AsyncDeserializeFromString(xml);

            // We only paste if there is something to paste which can be converted to lamps
            if (lamps.Count() != 0) {
                
                // All the lamps are turned into viewmodels
                foreach (Lamp lamp in lamps) {
                    string path = "OptiLight.ViewModel." + lamp.viewModel;
                    Type lampTypeVM = Type.GetType(path, true);
                    object[] argsVM = { lamp };
                    LampViewModel lampVM = (LampViewModel) Activator.CreateInstance(lampTypeVM, argsVM);

                    // We place the lamp 50 pixels down and right of the original lamp
                    // but only if it is inside the canvas
                    var newPosX = lampVM.X + 50;
                    var newPosY = lampVM.Y + 50;

                    if (newPosX + lampVM.Width > canvas.width) {
                        newPosX = canvas.width - lampVM.Width;
                    }

                    if (newPosY + lampVM.Height > canvas.height) {
                        newPosY = canvas.height - lampVM.Height;
                    }

                    // The lamp is added to the collection and their coordinates are changed
                    lampVM.X = newPosX;
                    lampVM.Y = newPosY;
                    undoRedoController.AddAndExecute(new AddLamp(Lamps, lampVM));
                }
            }            
        }

        #endregion Cut / Copy / Paste

        #region Add / Remove

        // Method for adding lamps
        
        //    
        // IS ONLY PUBLIC FOR TESTING PURPOSES!!!
        //
        public void AddNewLamp(IList selectedAddingLamp) {

            // We get the selected lamp from the View
            Lamp selectedLamp = selectedAddingLamp.Cast<Lamp>().ToList().First();
            


            //We either choose a type of lamp to add or stop adding
            if (sidePanel.addingLampSelected == null) {
                sidePanel.AddingColor = Colors.RosyBrown;
                sidePanel.addingLampSelected = selectedLamp;
            }
            else if (sidePanel.addingLampSelected.name.Equals(selectedLamp.name)) {
                sidePanel.addingLampSelected = null;
                sidePanel.AddingColor = Colors.Transparent;
            }
            else {
                sidePanel.addingLampSelected = selectedLamp;
                sidePanel.AddingColor = Colors.RosyBrown;
            }
        }

        // We remove the selected lamps
        private void RemoveLamp() {
            LampViewModel lamp = getSelectedLamps()[0];
            lamp.IsSelected = false;
            sidePanel.ShowSidePanelBox = Visibility.Collapsed;
            undoRedoController.AddAndExecute(new RemoveLamp(Lamps, new List<LampViewModel>(){lamp}));
        }

        #endregion Add / Remove

        #region Select / Deselect

        // Returns all selected lamps
        public List<LampViewModel> getSelectedLamps() {
            return Lamps.Where(lamp => lamp.IsSelected).ToList();
        }

        // We check whether we can remove lamps
        public bool LampsAreSelected() {
            foreach (var lamp in Lamps) {
                if (lamp.IsSelected) return true;
            }
            return false;
        }

        // Unselects all lamps
        public void UnSelectAllLamps() {
            foreach (var lamp in Lamps) {
                lamp.IsSelected = false;
            }
        }

        #endregion Select / Deselect

        #region Light Switch

        // Method for turning all lamps on / off
        private void LightSwitch() {
            foreach (var lamp in Lamps) {
                lamp.IsTurnedOn = LightsOn;
            }
            LightsOn = !LightsOn;
        }

        // Method that switches the light of a single lamp
        private void singleLampLightSwitch() {
            LampViewModel lamp = getSelectedLamps()[0];
            undoRedoController.AddAndExecute(new ToggleLightOnLamp(lamp, !lamp.IsTurnedOn));
        }

        #endregion Light Switch

        #region Grid Snap / Show

        // Method for turning the grid on / off
        public void toggleSnapping() {
            canvas.SnapActive = !canvas.SnapActive;
        }

        // Method for making the grid visible or not
        public void toggleVisibility() {
            canvas.toggleVisibility();
        }

        #endregion Grid

    }
}