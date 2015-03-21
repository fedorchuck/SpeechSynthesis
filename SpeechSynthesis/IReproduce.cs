using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechSynthesis
{
    interface IReproduce<T>
    {
        void Speach(T obj);
    }
}
