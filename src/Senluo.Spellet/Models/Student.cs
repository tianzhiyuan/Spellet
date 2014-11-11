using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Student : AbstractModel
    {
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string EnglishName { get; set; }
        [MaxLength(255)]
        public string StudentID { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        public bool? Enabled { get; set; }
        [MaxLength(255)]
        public string Headimg { get; set; }
    }

    public class StudentQuery : AbstractQuery<Student>
    {
        public string StudentID { get; set; }
        public string NamePattern { get; set; }
        public string StudentIDPattern { get; set; }

    }
}