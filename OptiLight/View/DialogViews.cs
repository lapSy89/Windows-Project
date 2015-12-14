using Microsoft.Win32;
using System;
using System.Windows;

namespace OptiLight.View {

    public class DialogViews {

        // Dialog to get path for saving file.
        private static OpenFileDialog openDialog = new OpenFileDialog() { Title = "Open Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CheckFileExists = true };
        // Dialog to get path for loading file.
        private static SaveFileDialog saveDialog = new SaveFileDialog() { Title = "Save Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };

        // Create new file - showing window if changes aren't saved.
        public bool NewFile() =>
            MessageBox.Show("Continue without saving?", "OptiLight", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        // Open file - showing window if changes aren't saved
        public string OpenFile(bool changesMade) => 
            changesMade ?
                MessageBox.Show("Continue without saving?", "OptiLight", MessageBoxButton.YesNo) == MessageBoxResult.Yes ?
                    openDialog.ShowDialog() == true ? 
                        openDialog.FileName 
                        : null 
                : null 
            : openDialog.ShowDialog() == true ? 
                openDialog.FileName 
            : null;

        // Save file - showing window to specify save file
        public string SaveFile() => saveDialog.ShowDialog() == true ? saveDialog.FileName : null;

        // Close window - showing before window is closed and drawing isn't saved
        public bool CloseWindow() =>
            MessageBox.Show("Save before closing?", "OptiLight", MessageBoxButton.YesNo) == MessageBoxResult.Yes; 

        // Pop up window for when something goes wrong with the program.
        public void popUpError() {
             MessageBox.Show("Error opening file!", "OptiLight - Error", MessageBoxButton.OK);
        }
    }
}
