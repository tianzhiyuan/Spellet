﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Ors.Core.Data;

namespace Senluo.Spellet.Models
{
    public class Question : AbstractModel
    {
        public int? ContentID { get; set; }
        public int? Score { get; set; }
        /// <summary>
        /// 正确答案
        /// 暂时不使用这个字段
        /// </summary>
        [MaxLength(255)]
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