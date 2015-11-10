using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OptiLight.ViewModel {
    //Base viewModel
    //Should contain:
    //Add methods
    //All design patterns, usch as undo redo, copy paste. etc

    //Implements the Galasoft ViewModelBase
    public abstract class BaseViewModel : ViewModelBase {

        public static ObservableCollection<LampViewModel> Lamps { get; set; }

        public ICommand AddRoundCommand { get; }
        public ICommand AddSquareCommand { get; }
        public ICommand AddRectangleCommand { get; }

        //Constructor 
        //TODO: Be able ro Remove lamps, by unexecuting
        public BaseViewModel() {
            AddRoundCommand = new RelayCommand(AddRoundLamp);
            AddRectangleCommand = new RelayCommand(AddRectangleLamp);
            AddSquareCommand = new RelayCommand(AddSquareLamp);
          
        }
        // Method for executing the AddLampCommand
        private void AddRoundLamp() {
            new Command.AddLamp(Lamps, new RoundLampViewModel(new Model.RoundLamp())).Execute();
        }

        private void AddRectangleLamp() {
            new Command.AddLamp(Lamps, new RectangleLampViewModel(new Model.RectangleLamp())).Execute();
        }

        private void AddSquareLamp() {
            new Command.AddLamp(Lamps, new SquareLampViewModel(new Model.SquareLamp())).Execute();
        }

    }
}