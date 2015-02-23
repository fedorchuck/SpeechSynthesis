using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SpeechSynthesis
{
    class Logic
    {
        private String _text;
        private RecordsLibrary _object_from_file;//=new RecordsLibrary();
//List<Syllable> records = _object_from_file.Records;
        public RecordsLibrary ObjectFromFile
        {
            get { return _object_from_file; }
            set { _object_from_file = value;}
        }

        public string Text
        {
            get { return _text; }
            set { if (!String.IsNullOrEmpty(value)) _text = value; }
        }

        public void Dictionary()
        {
            ObjectFromFile = Serialization.DeserializeObject("c:\\tmp\\SpeechSynthesis.xml");
        }

        public String GetSyllables()
        {
            if (ObjectFromFile != null)
                return ObjectFromFile.ToString();
            else
                return String.Format("Object reference not set to an instance of an object.");
        }

        public void CreateSpeach()
        {
            
        }

        public void SpeachText()
        {
            
        }
    }
}
