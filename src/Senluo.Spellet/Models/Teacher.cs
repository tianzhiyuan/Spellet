using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Teacher : AbstractModel
    {
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Account { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
    }

    public class TeacherQuery : AbstractQuery<Teacher>
    {
        public string Name { get; set; }
        public string Account { get; set; }
    }
}