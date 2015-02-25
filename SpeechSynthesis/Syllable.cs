using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpeechSynthesis
{
    [DataContract]
    public class Syllable
    {
        private String _forPrinting;
        private String _namedOfLetter;
        private String _inWriting;

        [DataMember]
        public string ForPrinting
        {
            get { return _forPrinting; }
            set { _forPrinting = value; }
        }

        [DataMember]
        public string InWriting
        {
            get { return _inWriting; }
            set { _inWriting = value; }
        }

        public override string ToString()
        {
            return string.Format("in writing: [{0}], to writing{1}", ForPrinting, InWriting);
        }
    }
}
