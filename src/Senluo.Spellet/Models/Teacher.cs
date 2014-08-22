using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Teacher : AbstractModel
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }

    public class TeacherQuery : AbstractQuery<Teacher>
    {
        public string Name { get; set; }
        public string Account { get; set; }
    }
}