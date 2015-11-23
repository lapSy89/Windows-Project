using Microsoft.Win32;
using System;
using System.Windows;

namespace OptiLight.View {

    public class DialogViews {

        private static OpenFileDialog openDialog = new OpenFileDialog() { Title = "Open Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CheckFileExists = true };
        private static SaveFileDialog saveDialog = new SaveFileDialog() { Title = "Save Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };

        // Create new file - showing window if changes aren't saved.
        public bool NewFile() =>
            MessageBox.Show("Forsæt uden at gemme?", "OptiLight", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        // Open file - showing window if changes aren't saved
        public string OpenFile(bool changesMade) => 
            changesMade ?
                MessageBox.Show("Fortsæt uden at gemme?", "OptiLight", MessageBoxButton.YesNo) == MessageBoxResult.Yes ?
                    openDialog.ShowDialog() == true ? 
                        openDialog.FileName 
                        : null 
                : null 
            : openDialog.ShowDialog() == true ? 
                openDialog.FileName 
            : null;

        // Save file - showing window to specify save file
        public string SaveFile() => saveDialog.ShowDialog() == true ? saveDialog.FileName : null;
    }
}
