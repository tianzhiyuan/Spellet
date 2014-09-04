using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class CourseContent : AbstractModel
    {
        public int? CourseID { get; set; }
        public int? ContentID { get; set; }
        public Entry Entry { get; set; }
    }
    public class CourseContentQuery : AbstractQuery<CourseContent>
    {
        public int[] CourseIDList { get; set; }
        public int[] ContentIDList { get; set; }
    }
}