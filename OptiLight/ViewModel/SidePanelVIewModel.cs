using OptiLight.Command;
using OptiLight.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace OptiLight.ViewModel {
    public class SidePanelViewModel : BaseViewModel {

        //We use the Singleton design pattern for our constructor
        public static SidePanelViewModel Instance { get; } = new SidePanelViewModel();

        // The currently selected lamp type to add represented. Null when none is selected.
        public Lamp addingLampSelected { get; set; }

        // Values for displaying radius on the sidepanel
        private double currentLampHoriRadius { get; set; }
        private double currentLampVertRadius { get; set; }

        // Constructor
        public SidePanelViewModel() {
            CurrentLampHoriRadius = 0;
            CurrentLampVertRadius = 0;
        }

        // The color of the selected lamp in the side menu - either transparent or darkgray
        private Color addingColor = Colors.Transparent;
        public Color AddingColor {
            get { return addingColor; }
            set { addingColor = value; RaisePropertyChanged(); }
        }

        // The visisbility of the box in the sidepanel
        private Visibility showSidePanelBox = Visibility.Collapsed;
        public Visibility ShowSidePanelBox {
            get { return showSidePanelBox; }
            set { showSidePanelBox = value; RaisePropertyChanged(); }
        }

        // Method for getting/setting currentLampBrightness
        public double CurrentLampBrightness {
            get {
                if (Lamps != null && LampsAreSelected()) {
                    LampViewModel lamp = getSelectedLamps()[0];
                    return Math.Round(lamp.Brightness, 0);
                }
                return 0;
            }
            set {
                if (Lamps != null && LampsAreSelected()) {
                    LampViewModel lamp = getSelectedLamps()[0];
                    undoRedoController.AddAndExecute(new ChangeBrightness(lamp, value - lamp.Brightness));
                    CurrentLampVertRadius = (lamp.VerticalUp + lamp.VerticalDown) / 2;
                    CurrentLampHoriRadius = (lamp.HorizontalLeft + lamp.HorizontalRight) / 2;
                }
                RaisePropertyChanged();
            }
        }

        // Method for getting/setting currentLampHeight
        public double CurrentLampHeight {
            get {
                if (Lamps != null && LampsAreSelected()) {
                    LampViewModel lamp = getSelectedLamps()[0];
                    return Math.Round(lamp.LampHeight, 1);
                }
                return 0;

            }
            set {
                if (Lamps != null && LampsAreSelected()) {
                    LampViewModel lamp = getSelectedLamps()[0];
                    undoRedoController.AddAndExecute(new ChangeHeight(lamp, value - lamp.LampHeight));
                    CurrentLampVertRadius = (lamp.VerticalUp + lamp.VerticalDown) / 2;
                    CurrentLampHoriRadius = (lamp.HorizontalLeft + lamp.HorizontalRight) / 2;
                }
                RaisePropertyChanged();
            }
        }

        // Method for getting/setting currentLampVertRadius
        public double CurrentLampVertRadius {
            get { return Math.Round(currentLampVertRadius * (-1), 0); }
            set {
                currentLampVertRadius = value;
                RaisePropertyChanged();
            }
        }

        // Method for getting/setting currentLampHoriRadius
        public double CurrentLampHoriRadius {
            get { return Math.Round(currentLampHoriRadius * (-1), 0); }
            set {
                currentLampHoriRadius = value;
                RaisePropertyChanged();
            }
        }
    }
}
