using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OptiLight.ViewModel
{
    //Base viewModel
    //Should contain:
    //Add methods
    //All design patterns, usch as undo redo, copy paste. etc

    //Implements the Galasoft ViewModelBase
    public abstract class BaseViewModel : ViewModelBase
    {
        public static ObservableCollection<LampViewModel> Lamps { get; set; }


        public ICommand AddRoundCommand { get; }
        public ICommand AddSquareCommand { get; }
        public ICommand AddRectangleCommand { get; }

        public ICommand RemoveLampsCommand { get; }

        //Constructor 
        //TODO: Be able ro Remove lamps, by unexecuting
        public BaseViewModel()
        {
            AddRoundCommand = new RelayCommand(AddRoundLamp);
            AddRectangleCommand = new RelayCommand(AddRectangleLamp);

      
          //  RemoveLampsCommand = new RelayCommand<IList>(RemoveShapes, CanRemoveShapes);
          
        }
        // Method for executing the AddLampCommand
        private void AddRoundLamp()
        {
            new Command.AddLamp(Lamps, new RoundLampViewModel(new Model.RoundLamp())).Execute();
        }

        private void AddRectangleLamp()
        {
            new Command.AddLamp(Lamps, new ViewModel.RectangleLampViewModel(new Model.RectangleLamp())).Execute();
        }

    }
}