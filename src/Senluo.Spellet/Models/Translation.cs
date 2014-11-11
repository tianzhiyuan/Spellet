using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Translation:AbstractModel
    {
        public int? EntryID { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
    }
    public class TranslationQuery:AbstractQuery<Translation>
    {
        public int[] EntryIDList { get; set; }
    }
}
