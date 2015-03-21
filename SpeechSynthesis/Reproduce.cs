using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DirectX;
using Microsoft.DirectX.AudioVideoPlayback;

namespace SpeechSynthesis
{
    class Reproduce : IReproduce<List<String>>
    {
        public void Speach(List<String> queue)
        {
            foreach (String stringNumbers in queue)
            {
                if (String.IsNullOrEmpty(stringNumbers) || String.IsNullOrWhiteSpace(stringNumbers))
                    Console.Beep(700, 100);
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
                            Thread.Sleep((int)(voice.Duration * 1000 - 100));
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
    }
}
