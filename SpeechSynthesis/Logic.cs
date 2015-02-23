using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpeechSynthesis
{
    class Logic
    {
        private String _text;

        public string Text
        {
            get { return _text; }
            set { if (!String.IsNullOrEmpty(value)) _text = value; }
        }

        public void Dictionary()
        {
            RecordsLibrary object_from_file = Serialization.DeserializeObject("c:\\tmp\\SpeechSynthesis.xml");

            if (object_from_file == null)
                return;

            String output = null;
            List<Syllable> records = object_from_file.Records;
            foreach (Syllable record in records)
            {
                output += record.ToString();
            }
        }

        public String tmp()
        {
            RecordsLibrary object_from_file = Serialization.DeserializeObject("c:\\tmp\\SpeechSynthesis.xml");

            if (object_from_file == null)
                return null;

            String output = null;
            List<Syllable> records = object_from_file.Records;
            foreach (Syllable record in records)
            {
                output += record.ToString();
                output += Environment.NewLine;
            }
            return output;
        }

        public void GetSyllables()
        {

        }

        public void CreateSpeach()
        {
            
        }

        public void SpeachText()
        {
            
        }
    }
}
