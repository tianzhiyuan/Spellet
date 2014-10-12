using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Student : AbstractModel
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string StudentID { get; set; }
        public string Password { get; set; }
        public bool? Enabled { get; set; }
        public string Headimg { get; set; }
    }

    public class StudentQuery : AbstractQuery<Student>
    {
        public string StudentID { get; set; }
        public string NamePattern { get; set; }
        public string StudentIDPattern { get; set; }

    }
}