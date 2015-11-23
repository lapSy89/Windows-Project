using OptiLight.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using OptiLight.View;
using OptiLight.Serialization;
using System.Linq;
using OptiLight.Model;
using System.Collections.Generic;

namespace OptiLight.ViewModel {
    //Base viewModel
    //Should contain:
    //Add methods
    //All design patterns, such as undo redo, copy paste. etc

    //Implements the Galasoft ViewModelBase
    public abstract class BaseViewModel : ViewModelBase {

        //The undoRedoController is created only here once
        protected UndoRedoController undoRedoController = UndoRedoController.Instance;

        // All the single lamps, all the single lamps, all the single lamps, all the single lamps, throw your light up!
        public static ObservableCollection<LampViewModel> Lamps { get; set; }

        public DialogViews dialogWindow { get; set; } // Dialog windows for New, Open and Save
        public bool changesMade = true;               // Changes made to the drawing

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public ICommand AddRoundCommand { get; }
        public ICommand AddSquareCommand { get; }
        public ICommand AddRectangleCommand { get; }

        public ICommand NewDrawingCommand { get; }
        public ICommand SaveDrawingCommand { get; }
        public ICommand LoadDrawingCommand { get; }

        public LampViewModel targetedLamp { get; set; }

        //Constructor 
        public BaseViewModel() {

            dialogWindow = new DialogViews();

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            AddRoundCommand = new RelayCommand(AddRoundLamp);
            AddRectangleCommand = new RelayCommand(AddRectangleLamp);
            AddSquareCommand = new RelayCommand(AddSquareLamp);
        }
          
            NewDrawingCommand = new RelayCommand(NewDrawing);
            LoadDrawingCommand = new RelayCommand(LoadDrawing);
            SaveDrawingCommand = new RelayCommand(SaveDrawing);
          
        }

        // Method for making a new drawing
        private void NewDrawing() {
            // Check if changes are made to the drawing
            if (changesMade) {
                // Pop up window for confirming deleting of changes.
                if (dialogWindow.NewFile()) {
                    // Deleting lamps
                    Lamps.Clear();
                }
            } else {
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
                changesMade = false; // Setting changes made to false since no changes are made after saving.
            }
        }

        // Method for loading drawing
        private async void LoadDrawing() {
            if (changesMade) {
                string loadPath = dialogWindow.OpenFile(true);
                if (loadPath != null) {
                    // Get list of lamps
                    List<Lamp> lamps = await XML.Instance.AsyncOpenFromFile(loadPath);

                    // Clear the board for loading new lamps
                    Lamps.Clear();
                    // Inserting lamps into array of lamps
                    lamps.Select(lamp => lamp is RoundLamp ?
                        (LampViewModel)new RoundLampViewModel(lamp) 
                    : lamp is SquareLamp ?
                        (LampViewModel)new SquareLampViewModel(lamp) 
                    : new RectangleLampViewModel(lamp)).ToList().ForEach(lamp => Lamps.Add(lamp));
                }
            } else {
                string loadPath = dialogWindow.OpenFile(false);
                if (loadPath != null) {
                    List<Lamp> lamps = await XML.Instance.AsyncOpenFromFile(loadPath);

                    Lamps.Clear();
                    lamps.Select(lamp => lamp is RoundLamp ?
                        (LampViewModel)new RoundLampViewModel(lamp)
                    : lamp is SquareLamp ?
                        (LampViewModel)new SquareLampViewModel(lamp)
                    : new RectangleLampViewModel(lamp)).ToList().ForEach(lamp => Lamps.Add(lamp));
                }
            }
        }


        // Method for executing the AddLampCommand
        private void AddRoundLamp() {
            this.undoRedoController.AddAndExecute(new Command.AddLamp(Lamps, new RoundLampViewModel(new Model.RoundLamp())));
        }

        private void AddRectangleLamp() {
            this.undoRedoController.AddAndExecute(new Command.AddLamp(Lamps, new RectangleLampViewModel(new Model.RectangleLamp())));
        }

        private void AddSquareLamp() {
            this.undoRedoController.AddAndExecute(new Command.AddLamp(Lamps, new SquareLampViewModel(new Model.SquareLamp())));
        }

        // We check whether we can remove the lamp
        private bool CanRemoveLamp() => targetedLamp == null; 

        // We remove the selected lamp
        private void RemoveLamp(IList lampToRemove)
        {
            undoRedoController.AddAndExecute(new RemoveLamp(Lamps, lampToRemove.Cast<LampViewModel>().ToList()));
        }
    }
}