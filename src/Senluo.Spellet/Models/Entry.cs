using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Entry : AbstractModel
    {
        [MaxLength(255)]
        public string Word { get; set; }
        [MaxLength(255)]
        public string Phonetic_UK { get; set; }
        [MaxLength(255)]
        public string Phonetic_US { get; set; }
        [MaxLength(255)]
        public string Phonetic_UK_Audio { get; set; }
        [MaxLength(255)]
        public string Phonetic_US_Audio { get; set; }
        public ICollection<Translation> Translations { get; set; }
        public ICollection<Example> Examples { get; set; } 
    }

    public class EntryQuery:AbstractQuery<Entry>
    {
        public string Word { get; set; }
        public string WordPattern { get; set; }
        
    }
}
