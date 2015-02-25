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
        private RecordsLibrary _object_from_file;

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
            ObjectFromFile = Serialization.DeserializeObject(@"tmp\SpeechSynthesis.xml");
            /*if (ObjectFromFile != null)
                return ObjectFromFile.ToString();
            else
                return String.Format("Object reference not set to an instance of an object.");*/
        }

        public String GetSyllables()
        {
            String st = Text;
            Char[] ch = st.ToCharArray();
            String transcription = String.Empty;

            List<Syllable> syllables = ObjectFromFile.Records;

            //TODO: rules must be here
            for (int i = 0; i < st.Length; i++)
            {
                foreach (var syllable in syllables)
                {
                    if (syllable.ForPrinting == ch[i].ToString())
                    {
                        transcription += syllable.InWriting;
                        break;
                    }
                }
            }

            return transcription;
        }

        public void CreateSpeach()
        {

        }

        public void SpeachText()
        {
            
        }
    }
}
