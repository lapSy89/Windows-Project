using OptiLight.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace OptiLight.ViewModel {
    public class SidePanelViewModel : BaseViewModel {

        // Variables for showing and editing lamps (Brightness, Height and Radius)
        private double currentLampBrightness { get; set; }
        private double currentLampHeight { get; set; }
        private double currentLampVertRadius { get; set; }
        private double currentLampHoriRadius { get; set; }

        //We use the Singleton design pattern for our constructor
        public static SidePanelViewModel Instance { get; } = new SidePanelViewModel();
        private SidePanelViewModel() {

            // Initialize Current values for editing selected lamp
            CurrentLampBrightness = 0;
            CurrentLampHeight = 0;
            CurrentLampVertRadius = 0;
            CurrentLampHoriRadius = 0;
        }

        // The currently selected lamp type to add represented. Null when none is selected.
        public Lamp addingLampSelected { get; set; }

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
            get { return currentLampBrightness; }
            set {
                currentLampBrightness = value;
                if (Lamps != null && LampsAreSelected()) {
                    LampViewModel lamp = getSelectedLamps()[0];
                    lamp.Brightness = value;
                    CurrentLampVertRadius = lamp.Vertical;
                    CurrentLampHoriRadius = lamp.Horizontal;
                }
                RaisePropertyChanged();
            }
        }

        // Method for getting/setting currentLampHeight
        public double CurrentLampHeight {
            get { return currentLampHeight; }
            set {
                currentLampHeight = value;
                if (Lamps != null && LampsAreSelected()) {
                    LampViewModel lamp = getSelectedLamps()[0];
                    lamp.LampHeight = value;
                    CurrentLampVertRadius = lamp.Vertical;
                    CurrentLampHoriRadius = lamp.Horizontal;
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
