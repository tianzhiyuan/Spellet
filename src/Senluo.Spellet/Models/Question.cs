using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Question : AbstractModel
    {
        public int? ContentID { get; set; }
        public int? Score { get; set; }
        public string Expect { get; set; }
        public int? ExamID { get; set; }
        public Example Example { get; set; }
    }
    public class QuestionQuery : AbstractQuery<Question>
    {
        public int[] ExamIDList { get; set; }
        public int[] ContentIDList { get; set; }
    }
}