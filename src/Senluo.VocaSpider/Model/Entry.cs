using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ors.Core.Data;

namespace Senluo.VocaSpider.Model
{
    public class Entry 
    {
        public string Word { get; set; }
        public string Phonetic_UK { get; set; }
        public string Phonetic_US { get; set; }
        public string Phonetic_UK_Audio { get; set; }
        public string Phonetic_US_Audio { get; set; }
        public ICollection<Translation> Translations { get; set; }
        public ICollection<Example> Examples { get; set; } 
    }

    
}
