using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Exam : AbstractModel
    {
        public string Name { get; set; }
        public int? Count { get; set; }
        public int? DurationMinite { get; set; }
        public bool? Enabled { get; set; }
        public Question[] Questions { get; set; }
    }
    public class ExamQuery:AbstractQuery<Exam>
    {
        public string NamePattern { get; set; }
    }
}