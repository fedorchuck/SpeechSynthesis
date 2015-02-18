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
        private List<Syllable> SyllableString { get; set; }

        public string Text
        {
            get { return _text; }
            set { if (!String.IsNullOrEmpty(value)) _text = value; }
        }

        public void FillDictionary()
        {
            String[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
            String[] vowels = { "a", "e", "i", "o", };
            String[] consonants = { };
            
            List<Syllable> syllables = new List<Syllable>();
            
            for (int i = 0; i < alphabet.Length; i++)
            {
                Syllable s = new Syllable();
                s.Name = alphabet[i];
                s.Value = i;

                syllables.Add(s);
            }

            RecordsLibrary objectToFile = new RecordsLibrary();
            objectToFile.Records = new List<Syllable>();
            foreach (var syllable in syllables)
            {
                objectToFile.Records.Add(syllable);
            }

            Directory.CreateDirectory("c:\\tmp\\");
            Serialization.SerializeObject("c:\\tmp\\SpeechSynthesis.xml", objectToFile);
        }

        public void Dictionary()
        {
            RecordsLibrary object_from_file = Serialization.DeserializeObject("c:\\tmp\\SpeechSynthesis.xml");

            if (object_from_file == null)
                return;

            //String LastUses = object_from_file.Syllable;
            String output = null;
            List<Syllable> records = object_from_file.Records;
            foreach (Syllable record in records)
            {
                output += record.ToString();
            }
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
