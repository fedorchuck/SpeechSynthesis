using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpeechSynthesis
{
    [DataContract]
    public class RecordsLibrary
    {
        [DataMember]
        public List<Syllable> Records { get; set; }

        public override string ToString()
        {
            return string.Format("Syllable: {0}", Records);
        }
    }
}
