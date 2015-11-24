using OptiLight.Model;
using OptiLight.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OptiLight.Serialization
{
    class XML
    {

        public static XML Instance { get; } = new XML();

        private XML() { }

        public async void AsyncSaveToFile(List<Lamp> lamps, string path)
        {
            await Task.Run(() => ToFile(lamps, path));
        }

        private void ToFile(List<Lamp> lamps, string path)
        {
            using (FileStream stream = File.Create(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lamp>));
                serializer.Serialize(stream, lamps);
            }
        }

        public Task<List<Lamp>> AsyncOpenFromFile(string path)
        {
            return Task.Run(() => FromFile(path));
        }

        private List<Lamp> FromFile(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lamp>));
                return serializer.Deserialize(stream) as List<Lamp>;
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lamp>));
                List<Lamp> lamps = serializer.Deserialize(stream) as List<Lamp>;

                return lamps;
            }
        }
    }
}