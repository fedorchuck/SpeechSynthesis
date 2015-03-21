using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechSynthesis
{       
    class Handler
    {
        public String HandlerText(String text)
        {
            StringBuilder processedText = new StringBuilder(text);

            for (int i = 0; i < processedText.Length; i++)
            {
                switch (processedText[i])
                {
                    case '0':
                        processedText.Replace("0", " zero");
                        break;
                    case '1':
                        processedText.Replace("1", " one");
                        break;
                    case '2':
                        processedText.Replace("2", " two");
                        break;
                    case '3':
                        processedText.Replace("3", " three");
                        break;
                    case '4':
                        processedText.Replace("4", " four");
                        break;
                    case '5':
                        processedText.Replace("5", " five");
                        break;
                    case '6':
                        processedText.Replace("6", " six");
                        break;
                    case '7':
                        processedText.Replace("7", " seven");
                        break;
                    case '8':
                        processedText.Replace("8", " eight");
                        break;
                    case '9':
                        processedText.Replace("9", " nine");
                        break;
                    case '\n':
                        processedText.Remove(i, 1);
                        break;
                    case '\r':
                        processedText.Remove(i, 1);
                        break;
                    default: break;
                }
            }

            return processedText.ToString();
        }
    }
}
