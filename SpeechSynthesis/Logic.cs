using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Microsoft.DirectX;
using Microsoft.DirectX.AudioVideoPlayback;

namespace SpeechSynthesis
{
    class Logic
    {
        private String _text;
        private RecordsLibrary _object_from_file;
        private String _transcription;
        private List<String> queue = new List<string>();

        public string Transcription
        {
            get { return _transcription; }
            set { if (!String.IsNullOrEmpty(value)) _transcription += value; }
        }

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
            ObjectFromFile = Serialization.DeserializeObject(@"vol\SpeechSynthesis.xml");
            /*if (ObjectFromFile != null)
                return ObjectFromFile.ToString();
            else
                return String.Format("Object reference not set to an instance of an object.");*/
        }

        public String GetSyllables()
        {
            String st = Text;
            Char[] ch = st.ToCharArray();

            List<Syllable> syllables = ObjectFromFile.Records;

            //TODO: rules must be here
            for (int i = 0; i < st.Length; i++)
            {
                foreach (var syllable in syllables)
                {
                    if (syllable.ForPrinting == ch[i].ToString())
                    {
                        if (ch.Length>3)
                        { 
                            switch (syllable.ForPrinting)
                            {
                                case "c":
                                        if (ch[i - 1].ToString().ToLower() == "e")  AddSyllable("s");
                                        if (ch[i - 1].ToString().ToLower() == "i")  AddSyllable("s");
                                        if (ch[i - 1].ToString().ToLower() == "y")  AddSyllable("s");
                                    break;
                                case "e":
                                        if (ch[i - 1].ToString().ToLower() == "h")  AddSyllable("e");
                                    break;
                                default:
                                        AddSyllable(syllable.InWriting);
                                    break;
                            }
                        }
                        else
                        {
                            AddSyllable(syllable.InWriting);
                        }
                        break;
                    }
                }
            }

            //return transcription;
            return Transcription;
        }
        
        public void CreateSpeach()
        {
            foreach (String stringNumbers in queue)
            {
                if (String.IsNullOrEmpty(stringNumbers) || String.IsNullOrWhiteSpace(stringNumbers))
                    Console.Beep(700,100);
                else
                {
                    String way = "vol\\";
                    String expansion = ".wav";
                    String number = stringNumbers;
                    try
                    {
                        using (Audio voice = new Audio(way + number + expansion))
                        {
                            voice.Play();
                            Thread.Sleep((int)(voice.Duration*1000));
                            voice.Dispose();
                        }
                    }
                    catch (DirectXException)//no sound on this index.
                    {
                        Console.Beep(700, 100);
                    }
                }
            }
        }

        public void SpeachText(String stringSyllables)
        {
            String[] toSay = stringSyllables.Split(' ');
            for (int i = 0; i < toSay.Length; i++)
                if (!String.IsNullOrEmpty(toSay[i])||!String.IsNullOrWhiteSpace(toSay[i]))
                    queue.Add(toSay[i]);

            CreateSpeach();
        }

        private void AddSyllable(String symbol)
        {
            Transcription = symbol + " ";
        }
    }
}