using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpeechSynthesis
{
    class Serialization
    {
        private static DataContractSerializer imageLibrarySerializer = null;

        public static void SerializeObject(String fileName, RecordsLibrary imageLibrary)
        {
            if (imageLibrarySerializer == null)
                imageLibrarySerializer = new DataContractSerializer(typeof(RecordsLibrary));

            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true }))
            {
                imageLibrarySerializer.WriteObject(writer, imageLibrary);
            }
        }

        public static RecordsLibrary DeserializeObject(String fileName)
        {
            if (imageLibrarySerializer == null)
                imageLibrarySerializer = new DataContractSerializer(typeof(RecordsLibrary));

            if (!File.Exists(fileName))
                return null;

            using (FileStream file = File.OpenRead(fileName))
            {
                return (RecordsLibrary)imageLibrarySerializer.ReadObject(file);
            }
        }
    }
}
