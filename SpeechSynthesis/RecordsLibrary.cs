using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpeechSynthesis
{

    [DebuggerTypeProxy(typeof(RecordsLibraryDebugView))]
    [DebuggerDisplay("Syllable: {Records.Count}")]
    [DataContract]
    public class RecordsLibrary
    {
        [DataMember]
        public List<Syllable> Records { get; set; }

        public override string ToString()
        {
            if (Records != null)
                return String.Join(Environment.NewLine, Records);
            else
                return String.Format("not performed de(se)rialization.");
        }
    }

    internal sealed class RecordsLibraryDebugView
    {
        private ICollection<Syllable> collection;

        public RecordsLibraryDebugView(RecordsLibrary collection)
        {
            if (collection == null)
                throw new Exception();

            this.collection = collection.Records;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Syllable[] Items
        {
            get
            {
                Syllable[] items = new Syllable[collection.Count];
                collection.CopyTo(items, 0);
                return items;
            }
        }
    } 
}
