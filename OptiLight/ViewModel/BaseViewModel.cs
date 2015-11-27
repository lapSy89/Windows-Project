using OptiLight.Command;
using OptiLight.Model;
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
//using LampLibrary; // LampLibrary DLL

namespace OptiLight.ViewModel {

    // Implements the Galasoft ViewModelBase
    public abstract class BaseViewModel : ViewModelBase {

        // The undoRedoController is created only here once
        protected UndoRedoController undoRedoController = UndoRedoController.Instance;

        // All the single lamps, all the single lamps, all the single lamps, all the single lamps, throw your light up!
        public static ObservableCollection<LampViewModel> Lamps { get; set; }

        // Contains a copy of all current types of lamps - used in the sidepanel
        public static List<Lamp> lampTypes { get; } = Lamp.lampTypes;

        // The currently selected lamp type to add represented. Null when none is selected.
        public Lamp addingLampSelected { get; set; }

        // The color of the selected lamp in the side menu - either transparent or darkgray
        private Color addingColor;
        public Color AddingColor { get { return addingColor; } set { addingColor = value; RaisePropertyChanged(); } }

        // Dialog windows for New, Load and Save
        public DialogViews dialogWindow { get; set; }

        // Whether the global lights of the lamps are on of off
        public bool lightsOn = true;

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

        public ICommand LightSwitchCommand { get; }

        //Constructor 
        public BaseViewModel() {

            //The initial color of the sidepanal
            AddingColor = Colors.Transparent;

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
        
            LightSwitchCommand = new RelayCommand(LightSwitch);
            LightSwitchCommand = new RelayCommand(LightSwitch);
        }

        #region New / Save / Load

        // Method for making a new drawing
        private void NewDrawing() {
            // Check if changes are made to the drawing
            if (undoRedoController.CanUndo() || undoRedoController.CanRedo()) {
                // Pop up window for confirming deleting of changes.
                if (dialogWindow.NewFile()) {
                    // Deleting lamps
                    clearWorkspace();
                    Lamps.Clear();
                }
            } else {
                clearWorkspace();
                Lamps.Clear();
            }
        }


        // Method for saving drawing
        private void SaveDrawing() {
            // Path for saving the file
            string savePath = dialogWindow.SaveFile();
            if (savePath != null) {
                // Saving the file.
                XML.Instance.AsyncSaveToFile(Lamps.Select(x => x.Lamp).ToList(), savePath);
            }
        }

        // Method for loading drawing
        private async void LoadDrawing() {
            string loadPath;
            if (undoRedoController.CanUndo() || undoRedoController.CanRedo()) {
                loadPath = dialogWindow.OpenFile(true);  
            } else {
                loadPath = dialogWindow.OpenFile(false);
            }

            if (loadPath != null) {
                // Get list of lamps
                List<Lamp> lamps = await XML.Instance.AsyncOpenFromFile(loadPath);

                // If there is an error in opening the file.
                if(lamps.Count == 0) {
                    dialogWindow.popUpError();
                } else {
                    // Clear the board for loading new lamps
                    Lamps.Clear();
                    // Inserting lamps into array of lamps
                    lamps.Select(lamp => lamp is RoundLamp ?
                        (LampViewModel)new RoundLampViewModel(lamp)
                    : lamp is SquareLamp ?
                        (LampViewModel)new SquareLampViewModel(lamp)
                    : new RectangleLampViewModel(lamp)).ToList().ForEach(lamp => Lamps.Add(lamp));
                    clearWorkspace();
                }
                
            }
        }

        // We clear the workspace for loading or new workspace
        private void clearWorkspace() {
            undoRedoController.ClearStacks();
        }

        #endregion New / Save / Load

        #region Cut / Copy / Paste

        // The selected lamps are removed and moved to the clipboard as xml
        private async void Cut() {
            var selectedLamps = Lamps.Where(lamp => lamp.IsSelected).ToList();
            undoRedoController.AddAndExecute(new RemoveLamp(Lamps,selectedLamps));
            var xml = await XML.Instance.AsyncSerializeToString(selectedLamps.Select(lamp => lamp.Lamp).ToList());
            Clipboard.SetText(xml);
        }

        // The selected lamps are copied to clipboard as xml
        private async void Copy() {
            var selectedLamps = Lamps.Where(lamp => lamp.IsSelected).ToList();
            var xml = await XML.Instance.AsyncSerializeToString(selectedLamps.Select(lamp => lamp.Lamp).ToList());
            Clipboard.SetText(xml);
        }

        // The lamps in the clipboard are pasted
        private async void Paste() {
            // We retrieve the xml from clipboard and deserialize into Lamps
            var xml = Clipboard.GetText();
            List<Lamp> lamps = await XML.Instance.AsyncDeserializeFromString(xml);

            if (lamps.Count() == 0) {
                // Do nothing when the paste data isn't correct
            } else {
                // All the lamps are turned into viewmodels
                List<LampViewModel> lampsVM = lamps.Select(lamp => lamp is RoundLamp ?
                        (LampViewModel)new RoundLampViewModel(lamp)
                    : lamp is SquareLamp ?
                        (LampViewModel)new SquareLampViewModel(lamp)
                    : new RectangleLampViewModel(lamp)).ToList();

                // All the lamps are added to the collection and their coordinates are changed
                foreach (var lamp in lampsVM) {
                    lamp.X = lamp.X + 50;
                    lamp.Y = lamp.Y + 50;
                    undoRedoController.AddAndExecute(new AddLamp(Lamps, lamp));
                }
            }            
        }

        #endregion Cut / Copy / Paste

        // Method for adding lamps
        private void AddNewLamp(IList selectedAddingLamp) {

            // We get the selected lamp from the View
            Lamp selectedLamp = selectedAddingLamp.Cast<Lamp>().ToList().First();

            //We either choose a type of lamp to add or stop adding
            if (addingLampSelected == null) {
                AddingColor = Colors.DarkGray;
                addingLampSelected = selectedLamp;
            }
            else if (addingLampSelected.name.Equals(selectedLamp.name)) {
                addingLampSelected = null;
                AddingColor = Colors.Transparent;
            }
            else {
                addingLampSelected = selectedLamp;
                AddingColor = Colors.DarkGray;
            }
        }

        // We remove the selected lamps
        private void RemoveLamp() {
            undoRedoController.AddAndExecute(new RemoveLamp(Lamps, getSelectedLamps()));
        }

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

        // Method for turning all lamps on / off
        private void LightSwitch() {
            foreach (var lamp in Lamps) {
                lamp.IsTurnedOn = lightsOn;
            }
            lightsOn = !lightsOn;
        }
    }
}