using System.Collections.Generic;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Entry : AbstractModel
    {
        public string Word { get; set; }
        public string Phonetic_UK { get; set; }
        public string Phonetic_US { get; set; }
        public string Phonetic_UK_Audio { get; set; }
        public string Phonetic_US_Audio { get; set; }
        public ICollection<Translation> Translations { get; set; }
        public ICollection<Example> Examples { get; set; } 
    }

    public class EntryQuery:AbstractQuery<Entry>
    {
        public string Word { get; set; }
        public string WrodPattern { get; set; }
        
    }
}
