using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Answer : AbstractModel
    {
        public int? QuestionID { get; set; }

        public int? SheetID { get; set; }
        public int? Score { get; set; }
        public string Fill { get; set; }
    }
    public class AnswerQuery : AbstractQuery<Answer>
    {
        public int[] QuestionIDList { get; set; }
        public int[] SheetIDList { get; set; }

    }
}