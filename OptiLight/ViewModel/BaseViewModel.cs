using OptiLight.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

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

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public ICommand AddRoundCommand { get; }
        public ICommand AddSquareCommand { get; }
        public ICommand AddRectangleCommand { get; }

        public LampViewModel targetedLamp { get; set; }

        //Constructor 
        public BaseViewModel() {
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            AddRoundCommand = new RelayCommand(AddRoundLamp);
            AddRectangleCommand = new RelayCommand(AddRectangleLamp);
            AddSquareCommand = new RelayCommand(AddSquareLamp);
        }

        // Methods for adding lamps
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