﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Course : AbstractModel
    {
        [MaxLength(255)]
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public CourseContent[] Contents { get; set; }
    }

    public class CourseQuery : AbstractQuery<Course>
    {
        public Range<DateTime> StartTimeRange { get; set; }
        
    }
}