using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Senluo.Spellet.Areas.Student.Models
{
    public class AnswerSheetVM
    {
        public AnswerSheetVM()
        {
            
        }
        public int ExamID { get; set; }
        public int ID { get; set; }
        public int Count { get; set; }
        public int TotalScore { get; set; }
        public IEnumerable<AnswerVM> Answers { get; set; }
        
    }
    public class AnswerVM
    {
        /// <summary>
        /// 是否答对了
        /// </summary>
        public bool IsRight { get; set; }
        /// <summary>
        /// 填了什么
        /// </summary>
        public string Filling { get; set; }
        /// <summary>
        /// 正确答案
        /// </summary>
        public string Expect { get; set; }
        /// <summary>
        /// 原始句子
        /// </summary>
        public string OriginSentense { get; set; }
        /// <summary>
        /// 翻译
        /// </summary>
        public string Translation { get; set; }
    }
}