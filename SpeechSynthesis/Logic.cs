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
using Microsoft.DirectX.PrivateImplementationDetails;

namespace SpeechSynthesis
{
    class Logic
    {
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

        public void Dictionary()
        {
            ObjectFromFile = Serialization.DeserializeObject(@"vol\SpeechSynthesis.xml");
        }

        public String GetSyllables(String st)
        {
            Char[] ch = st.ToCharArray();
            List<Syllable> syllables = ObjectFromFile.Records;

            for (int i = 0; i < st.Length; i++)
            {
                foreach (var syllable in syllables)
                {
                    //UNDONE: rules must be here
                    if (syllable.ForPrinting.Equals(ch[i].ToString()))
                    {
                        //search and replace object with a large letter on the same but a small letter.
                        #region
                        if ((syllable.ForPrinting.Equals(ch[i].ToString().ToUpper())) &&
                            ((!String.IsNullOrWhiteSpace(ch[i + 1].ToString())
                                /*) || (!String.IsNullOrEmpty(ch[i + 1].ToString())*/)))
                        {
                            foreach (var newSyllable in syllables)
                            {
                                if (newSyllable.ForPrinting.Equals(ch[i].ToString().ToLower()))
                                {
                                    syllable.InWriting = newSyllable.InWriting;
                                    syllable.ForPrinting = newSyllable.ForPrinting;
                                    ch[i] = Convert.ToChar(newSyllable.ForPrinting);
                                    break;
                                }
                            }
                        }
                        #endregion

                        //duplicate letters.
                        if (syllable.ForPrinting.Equals(ch[i + 1].ToString())) break;

                        //special rules (reading).
                        if ((i < 1) || ch[i - 1].Equals(' '))
                        {
                            //if it's first letter
                            #region
                            if (ch.Length > 1)
                            {
                                switch (syllable.ForPrinting)
                                {
                                    case "a":
                                        if ((ch[i + 1].Equals('i')) && (ch[i + 2].Equals('r'))) break;
                                        else if ((ch[i + 1].Equals('r')) && (ch[i + 2].Equals('e'))) break;
                                        else if (ch[i + 1].Equals('i')) break;
                                        else if (ch[i + 1].Equals('y')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "b":
                                        if (ch[i + 1].Equals('t')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "c":
                                        if (ch[i + 1].Equals('e')) AddSyllable('s');
                                        else if (ch[i + 1].Equals('i')) AddSyllable('s');
                                        else if (ch[i + 1].Equals('y')) AddSyllable('s');
                                        else if (ch[i + 1].Equals('k')) break;
                                        else if (ch[i + 1].Equals('h')) break;
                                        else if (ch[i + 1].Equals('a')) AddSyllable('k');
                                        else AddSyllable('k');
                                        break;
                                    case "e":
                                        if (ch[i + 1].Equals('a')) break;
                                        else if (ch[i + 1].Equals('i')) break;
                                        else if (ch[i + 1].Equals('w')) break;
                                        else if (ch[i + 1].Equals('y')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "g":
                                        if (ch[i + 1].Equals('n')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "j":
                                        AddSyllable("10220");
                                        break;
                                    case "k":
                                        if (ch[i + 1].Equals('n')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "o":
                                        if (ch[i + 1].Equals('r')) AddSyllable("10007");
                                        else if (ch[i + 1].Equals('u')) break;
                                        else if (ch[i + 1].Equals('i')) break;
                                        else if (ch[i + 1].Equals('y')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "p":
                                        if (ch[i + 1].Equals('h')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "s":
                                        if (ch[i + 1].Equals('h')) break;
                                        else if (ch[i + 1].Equals('u')) AddSyllable("10217");
                                        else AddSyllable('s');
                                        break;
                                    case "t":
                                        if (ch[i + 1].Equals('h')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "u":
                                        AddSyllable("10010");
                                        break;
                                    case "w":
                                        if (ch[i + 1].Equals('r')) break;
                                        else if ((ch[i + 1].Equals('h')) && (ch[i + 2].Equals('o')))
                                            AddSyllable('h');
                                        else if (ch[i + 1].Equals('h')) AddSyllable('w');
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    default:
                                        AddSyllable(syllable.InWriting);
                                        break;
                                }
                            }
                            else AddSyllable(syllable.InWriting);
                            #endregion
                        }
                        else
                        {
                            //if it isn't first sybol string
                            #region
                            if (syllable.ForPrinting.Equals(ch[i - 1].ToString())) //duplicate letters
                            {
                                #region
                                switch (syllable.ForPrinting)
                                {
                                    case "u":
                                        AddSyllable("10012");
                                        break;
                                    case "o":
                                        if (ch[i + 1].Equals('r')) break;
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    case "e":
                                        AddSyllable("10001");
                                        break;
                                    case "c":
                                        if (i > 1 && (ch[i - 2].Equals('o'))) AddSyllable('k');
                                        else AddSyllable(syllable.InWriting);
                                        break;
                                    default:
                                        AddSyllable(syllable.InWriting);
                                        break;
                                }
                                break;
                                #endregion
                            }
                            switch (syllable.ForPrinting)
                            {
                                #region
                                case "a":
                                    if ((ch[i - 1].Equals('w')) && (ch[i + 1].Equals('r')))
                                        AddSyllable("10007");
                                    else if ((ch[i + 1].Equals('i')) && (ch[i + 2].Equals('r'))) break;
                                    else if ((ch[i + 1].Equals('r')) && (ch[i + 2].Equals('e'))) break;
                                    else if (ch[i - 1].Equals('e')) AddSyllable("10001");
                                    else if (ch[i + 1].Equals('r')) AddSyllable("10005");
                                    else if (ch[i + 1].Equals('i')) break;
                                    else if (ch[i + 1].Equals('y')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "b":
                                    if (ch[i + 1].Equals('t')) break;
                                    if (ch[i - 1].Equals('m')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "c":
                                    if (ch[i + 1].Equals('e')) AddSyllable('s');
                                    else if (ch[i + 1].Equals('i')) AddSyllable('s');
                                    else if (ch[i + 1].Equals('y')) AddSyllable('s');
                                    else if (ch[i + 1].Equals('k')) break;
                                    else if (ch[i + 1].Equals('h')) break;
                                    else if (ch[i + 1].Equals('a')) AddSyllable('k');
                                    else AddSyllable('k');
                                    break;
                                case "e":
                                    if ((ch[i + 1].Equals('r')) && (ch[i + 2].Equals('e'))) break;
                                    else if (i > 1 && ((ch[i - 1].Equals('r')) && (ch[i - 2].Equals('e'))))
                                        AddSyllable("10018");
                                    else if ((ch[i - 1].Equals('e')) && (ch[i + 1].Equals('r'))) break;
                                    else if (i > 1 && ((ch[i - 1].Equals('r')) && (ch[i - 2].Equals('a'))))
                                        AddSyllable("10020");
                                    else if (i > 1 && ((ch[i - 1].Equals('r')) && (ch[i - 2].Equals('u'))))
                                        AddSyllable("10019");
                                    else if (ch[i - 1].Equals('e')) AddSyllable("10001");
                                    else if (ch[i - 1].Equals('h')) AddSyllable("10004");
                                    else if (ch[i + 1].Equals('\r')) break;
                                    else if (ch[i + 1].Equals('r')) AddSyllable("10009");
                                    else if (ch[i + 1].Equals('w')) break;
                                    else if (ch[i + 1].Equals('a')) break;
                                    else if (ch[i + 1].Equals('i')) break;
                                    else if (ch[i + 1].Equals('y')) break;
                                    else if (ch[i + 1].Equals(' ')) break;
                                    else AddSyllable("10001");
                                    break;
                                case "t":
                                    if (ch[i + 1].Equals('h')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "h":
                                    if (ch[i - 1].Equals('t')) AddSyllable("10228");
                                    else if (ch[i - 1].Equals('s')) AddSyllable("10217");
                                    else if (ch[i - 1].Equals('c')) AddSyllable("10219");
                                    else if (ch[i - 1].Equals('w')) break;
                                    else if (ch[i - 1].Equals('p')) AddSyllable('f');
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "p":
                                    if (ch[i + 1].Equals('h')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "k":
                                    if (ch[i + 1].Equals('n')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "u":
                                    if ((ch[i + 1].Equals('r')) && (ch[i + 2].Equals('e'))) break;
                                    else if (ch[i + 1].Equals(' ')) AddSyllable("10011");
                                    else if (ch[i + 1].Equals('l')) AddSyllable("10011");
                                    else if (ch[i - 1].Equals('r')) AddSyllable("10010");
                                    else if (ch[i - 1].Equals('a')) AddSyllable("10011");
                                    else if (ch[i - 1].Equals(' ')) AddSyllable("10010");
                                    else if (ch[i - 1].Equals('f')) AddSyllable("10010");
                                    else if (ch[i + 1].Equals('\r')) AddSyllable("10011");
                                    else if (ch[i - 1].Equals('o')) AddSyllable("10016");
                                    else if (ch[i - 1].Equals('q')) AddSyllable('w');
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "o":
                                    if ((ch[i + 1].Equals('o')) && (ch[i + 2].Equals('r'))) break;
                                    else if ((ch[i - 1].Equals('o')) && (ch[i + 1].Equals('r'))) break;
                                    else if ((ch[i - 1].Equals('w')) && (ch[i + 1].Equals('r')))
                                        AddSyllable("10008");
                                    else if (ch[i + 1].Equals('r')) AddSyllable("10007");
                                    else if (ch[i + 1].Equals('u')) break;
                                    else if (ch[i + 1].Equals('i')) break;
                                    else if (ch[i + 1].Equals('y')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "r":
                                    if ((ch[i - 1].Equals('a')) && (ch[i + 1].Equals('e'))) break;
                                    else if ((ch[i - 1].Equals('i')) && (ch[i - 2].Equals('a')))
                                        AddSyllable("10020");
                                    else if ((ch[i - 1].Equals('e')) && (ch[i + 1].Equals('e'))) break;
                                    else if ((ch[i - 1].Equals('e')) && (ch[i - 2].Equals('e')))
                                        AddSyllable("10018");
                                    else if ((ch[i - 1].Equals('o')) && (ch[i - 2].Equals('o')))
                                        AddSyllable("10019");
                                    else if ((ch[i - 1].Equals('u')) && (ch[i + 1].Equals('e')))
                                        break;
                                    else if (ch[i - 1].Equals('o')) break;
                                    else if (ch[i + 1].Equals('\r')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "n":
                                    if (ch[i + 1].Equals('g')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "g":
                                    if (ch[i - 1].Equals('n')) AddSyllable("10226");
                                    else if (ch[i + 1].Equals('e')) AddSyllable("10220");
                                    else if (ch[i + 1].Equals('i')) AddSyllable("10220");
                                    else if (ch[i + 1].Equals('n')) break;
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "i":
                                    if (ch[i - 1].Equals('e')) AddSyllable("10014");
                                    else if ((ch[i - 1].Equals('a')) && (ch[i + 1].Equals('r'))) break;
                                    else if (ch[i - 1].Equals('a')) AddSyllable("10014");
                                    else if (ch[i - 1].Equals('o')) AddSyllable("10015");
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "y":
                                    if (ch[i - 1].Equals('a')) AddSyllable("10014");
                                    else if (ch[i - 1].Equals('e')) AddSyllable("10014");
                                    else if (ch[i - 1].Equals('y')) AddSyllable("10015");
                                    else if (ch[i - 1].Equals('t')) AddSyllable("10010");
                                    else if (ch[i - 1].Equals('k')) AddSyllable("10010");
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "s":
                                    if (ch[i + 1].Equals('h')) break;
                                    else if (ch[i + 1].Equals('\r')) AddSyllable('z');
                                    else if (ch[i - 1].Equals('a')) AddSyllable('z');
                                    else if (ch[i - 1].Equals('e')) AddSyllable('z');
                                    else if (ch[i - 1].Equals('i')) AddSyllable('z');
                                    else if (ch[i - 1].Equals('o')) AddSyllable('z');
                                    else if (ch[i - 1].Equals('u')) AddSyllable('z');
                                    else if (ch[i - 1].Equals('y')) AddSyllable('z');
                                    else if (ch[i - 1].Equals('\'')) AddSyllable('s');
                                    else if (ch[i + 1].Equals('u')) AddSyllable("10217");
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                case "w":
                                    if ((ch[i + 1].Equals('h')) && (ch[i + 2].Equals('o'))) break;
                                    else if (ch[i - 1].Equals('e')) AddSyllable("10221");
                                    break;
                                case "q":
                                    if (ch[i - 1].Equals('a')) AddSyllable('k');
                                    else if (ch[i - 1].Equals('e')) AddSyllable('k');
                                    else if (ch[i - 1].Equals('i')) AddSyllable('k');
                                    else if (ch[i - 1].Equals('o')) AddSyllable('k');
                                    else if (ch[i - 1].Equals('u')) AddSyllable('k');
                                    else if (ch[i - 1].Equals('y')) AddSyllable('k');
                                    else if (ch[i + 1].Equals('u')) AddSyllable('k');
                                    else AddSyllable(syllable.InWriting);
                                    break;
                                default:
                                    AddSyllable(syllable.InWriting);
                                    break;
                                    #endregion
                            }
                            #endregion
                        }

                        break;
                    }
                }
            }

            return Transcription; 
        }

        public void SpeachText(String stringSyllables)
        {
            List<String> queue = new List<string>();
            String.Join(stringSyllables, ' ');

            String[] toSay = stringSyllables.Split(' ');
            for (int i = 0; i < toSay.Length; i++)
                if (!String.IsNullOrEmpty(toSay[i])||!String.IsNullOrWhiteSpace(toSay[i]))
                    queue.Add(toSay[i]);


            Reproduce create = new Reproduce();
            create.Speach(queue);
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