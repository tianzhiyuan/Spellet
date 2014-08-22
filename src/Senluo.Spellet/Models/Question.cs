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
    }
    public class QuestionQuery : AbstractQuery<Question>
    {
        
    }
}