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
        }

        public String GetSyllables()
        {
            String st = Text;
            Char[] ch = st.ToCharArray();

            List<Syllable> syllables = ObjectFromFile.Records;

            for (int i = 0; i < st.Length; i++)
            {
                foreach (var syllable in syllables)
                {//TODO: rules must be here
                    if (syllable.ForPrinting.Equals(ch[i].ToString()))
                    {
                        if ((syllable.ForPrinting.Equals(ch[i].ToString().ToUpper())) &&
                            ((!String.IsNullOrWhiteSpace(ch[i + 1].ToString())/*) || (!String.IsNullOrEmpty(ch[i + 1].ToString())*/)))
                        {
                            //search and replace object with a large letter on the same but a small letter.
                            foreach (var newSyllable in syllables)
                            {
                                if (newSyllable.ForPrinting.Equals(ch[i].ToString().ToLower()))
                                { 
                                    syllable.InWriting = newSyllable.InWriting;
                                    break;
                                }
                            }
                        }

                        //duplicate letters.
                        if (syllable.ForPrinting.Equals(ch[i + 1].ToString()))   break;

                        //special rules (reading).
                        if (i < 1) //if it's first letter
                        {
                            if (ch.Length > 3)
                            {
                                switch (syllable.ForPrinting)
                                {
                                    case "u":
                                        AddSyllable("10010");
                                        break;
                                    case "j":
                                        AddSyllable("10220");
                                        break;
                                    case "c":
                                        if (ch[i + 1].ToString().ToLower().Equals("e")) AddSyllable('s');
                                        else if (ch[i + 1].ToString().ToLower().Equals("i")) AddSyllable('s');
                                        else if (ch[i + 1].ToString().ToLower().Equals("y")) AddSyllable('s');
                                        else if (ch[i + 1].ToString().ToLower().Equals("k")) break;
                                        else if (ch[i + 1].ToString().ToLower().Equals("h")) break;
                                        else if (ch[i + 1].ToString().ToLower().Equals("a")) AddSyllable('k');
                                        else AddSyllable('k');
                                        break;
                                    default: AddSyllable(syllable.InWriting);   break;
                                }
                            }
                            else AddSyllable(syllable.InWriting); 
                        }
                        else
                        {
                            if (ch.Length > 3)
                            {
                                switch (syllable.ForPrinting)
                                {
                                    case "c":
                                        if (ch[i + 1].ToString().ToLower().Equals("e")) AddSyllable('s');
                                        else if (ch[i + 1].ToString().ToLower().Equals("i")) AddSyllable('s');
                                        else if (ch[i + 1].ToString().ToLower().Equals("y")) AddSyllable('s');
                                        else if (ch[i + 1].ToString().ToLower().Equals("k")) break;
                                        else if (ch[i + 1].ToString().ToLower().Equals("h")) break;
                                        else if (ch[i + 1].ToString().ToLower().Equals("a")) AddSyllable('k');
                                        else AddSyllable('k');
                                        break;
                                    case "t":
                                        if (ch[i + 1].ToString().ToLower().Equals("h")) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "h":
                                        if (ch[i - 1].ToString().ToLower().Equals("t")) AddSyllable("10228");
                                        else if (ch[i - 1].ToString().ToLower().Equals("s")) AddSyllable("10217");
                                        else if (ch[i - 1].ToString().ToLower().Equals("c")) AddSyllable("10219");
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "u":
                                        if (ch[i + 1].ToString().ToLower().Equals(" ")) AddSyllable("10011");
                                        else if (ch[i + 1].ToString().ToLower().Equals("l")) AddSyllable("10011");
                                        else if (ch[i - 1].ToString().ToLower().Equals("r")) AddSyllable("10010");
                                        else if (ch[i - 1].ToString().ToLower().Equals(" ")) AddSyllable("10010");
                                        else if (ch[i - 1].ToString().ToLower().Equals("f")) AddSyllable("10010");
                                        else if (ch[i + 1].ToString().ToLower().Equals("\r")) AddSyllable("10011");
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "e":
                                        if (ch[i - 1].ToString().ToLower().Equals("e")) AddSyllable("10001");
                                        else if (ch[i - 1].ToString().ToLower().Equals("h")) AddSyllable("10004");
                                        else if (ch[i + 1].ToString().ToLower().Equals("\r")) break;
                                        else AddSyllable("10001");
                                        break;
                                    case "o":
                                        if (ch[i + 1].ToString().ToLower().Equals("r")) AddSyllable("10007");
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "r":
                                        if (ch[i - 1].ToString().ToLower().Equals("o")) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "n":
                                        if (ch[i + 1].ToString().ToLower().Equals("g")) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "g":
                                        if (ch[i - 1].ToString().ToLower().Equals("n")) AddSyllable("10226");
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "s":
                                        if (ch[i + 1].ToString().ToLower().Equals("h")) break;
                                        else if (ch[i + 1].ToString().ToLower().Equals("\r")) AddSyllable("10209");
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    default:    AddSyllable(syllable.InWriting);    break;
                                }
                            }
                            else AddSyllable(syllable.InWriting);    
                        }

                        break;
                    }
                }
            }

            return Transcription;
        }

        public void CreateSpeach(List<String> queue)
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
            List<String> queue = new List<string>();
            String.Join(stringSyllables, ' ');

            String[] toSay = stringSyllables.Split(' ');
            for (int i = 0; i < toSay.Length; i++)
                if (!String.IsNullOrEmpty(toSay[i])||!String.IsNullOrWhiteSpace(toSay[i]))
                    queue.Add(toSay[i]);

            CreateSpeach(queue);
        }

        private void AddSyllable(String symbol)
        {
            Transcription = String.Concat(symbol, ' ');//symbol + " ";
        }

        private void AddSyllable(Char symbol)
        {
            List<Syllable> syllables = ObjectFromFile.Records;
            foreach (var syllable in syllables)
            {
                if (syllable.ForPrinting.Equals(symbol.ToString()))
                    AddSyllable(syllable.InWriting);
            }
        }
    }
}