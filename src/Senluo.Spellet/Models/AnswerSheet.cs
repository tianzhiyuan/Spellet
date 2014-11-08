using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class AnswerSheet : AbstractModel
    {
        public int? StudentID { get; set; }
        public int? TotalScore { get; set; }
        public int? ExamID { get; set; }
        public Exam Exam { get; set; }
        public Answer[] Answers { get; set; }
        public Student Student { get; set; }
    }
    public class AnswerSheetQuery:AbstractQuery<AnswerSheet>
    {
        public int? StudentID { get; set; }
        public int? ExamID { get; set; }
        public int[] ExamIDList { get; set; }
        public int[] StudentIDList { get; set; }
        public Range<int> TotalScoreRange { get; set; } 
    }
}