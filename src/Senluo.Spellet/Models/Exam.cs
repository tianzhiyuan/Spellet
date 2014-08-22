using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Exam : AbstractModel
    {
        public int? Count { get; set; }
        public int? DurationMinite { get; set; }
    }
    public class ExamQuery:AbstractQuery<Exam>
    {
        
    }
}