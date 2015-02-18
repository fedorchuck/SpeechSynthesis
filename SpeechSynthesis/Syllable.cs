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
        private String _name;
        private Int32 _value;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: [{0}], Value: {1}", Name, Value);
        }
    }
}
