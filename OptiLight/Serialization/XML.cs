using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OptiLight.Model;
//using LampLibrary; // LampLibrary DLL

namespace OptiLight.Serialization
{
    class XML
    {

        // We use the Singleton design pattern for our constructor
        public static XML Instance { get; } = new XML();
        private XML() { }

        // We create an async method, to save the file in a second process.
        public async void AsyncSaveToFile(Setup setup, string path)
        {
            // The process is awaited, so that no changes are made to the drawing, 
            // before the file is saved.
            await Task.Run(() => ToFile(setup, path));
        }

        // We save the XML file.
        private void ToFile(Setup setup, string path)
        {
            using (FileStream stream = File.Create(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Setup));
                serializer.Serialize(stream, setup);
            }
        }

        // We create an async method, to load the file in a second process
        public Task<Setup> AsyncOpenFromFile(string path)
        {
            return Task.Run(() => FromFile(path));
        }

        // We load the XML file
        private Setup FromFile(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                Setup setup = new Setup();
                XmlSerializer serializer = new XmlSerializer(typeof(Setup));
                try {
                    setup = serializer.Deserialize(stream) as Setup;
                } catch (InvalidOperationException) {}

                return setup;
            }
        }

        public Task<string> AsyncSerializeToString(List<Lamp> lamps)
        {
            return Task.Run(() => SerializeToString(lamps));
        }

        private string SerializeToString(List<Lamp> lamps)
        {
            var stringBuilder = new StringBuilder();

            using (TextWriter stream = new StringWriter(stringBuilder))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lamp>));
                serializer.Serialize(stream, lamps);
            }

            return stringBuilder.ToString();
        }

        public Task<List<Lamp>> AsyncDeserializeFromString(string xml)
        {
            return Task.Run(() => DeserializeFromString(xml));
        }

        private List<Lamp> DeserializeFromString(string xml)
        {
            using (TextReader stream = new StringReader(xml))
            {
                List<Lamp> tempLamps = new List<Lamp>();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lamp>));
                try {
                    tempLamps = serializer.Deserialize(stream) as List<Lamp>;
                } catch (InvalidOperationException) { }

                return tempLamps;
            }
        }
    }
}