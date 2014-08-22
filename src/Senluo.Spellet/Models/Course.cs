using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Course : AbstractModel
    {
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public int? Duration { get; set; }
    }

    public class CourseQuery : AbstractQuery<Course>
    {
        
    }
}