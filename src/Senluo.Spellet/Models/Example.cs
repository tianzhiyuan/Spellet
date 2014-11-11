using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Ors.Core.Data;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Models
{
    public class Example:AbstractModel
    {
        public int? EntryID { get; set; }
        [MaxLength(2000)]
        public string Origin { get; set; }
        [MaxLength(2000)]
        public string Trans { get; set; }
        /// <summary>
        /// 例句中关键词。一般来讲就是对应词条的那个单词，但是例句中这个单词可能有词形变化
        /// 如果无法通过词条识别，那么就需要手动设置这个值。如果有多个，以逗号分隔。
        /// </summary>
        [MaxLength(255)]
        public string Keyword { get; set; }
        public Entry Entry { get; set; }
    }
    public class ExampleQuery:AbstractQuery<Example>
    {
        public int[] EntryIDList { get; set; }
    }
}
