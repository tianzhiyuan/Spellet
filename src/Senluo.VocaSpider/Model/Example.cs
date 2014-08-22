using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ors.Core.Data;

namespace Senluo.VocaSpider.Model
{
    public class Example
    {
        
        public string Origin { get; set; }
        public string Trans { get; set; }
        /// <summary>
        /// 例句中关键词。一般来讲就是对应词条的那个单词，但是例句中这个单词可能有词形变化
        /// 如果无法通过词条识别，那么就需要手动设置这个值。如果有多个，以逗号分隔。
        /// </summary>
        public string Keyword { get; set; }
        public Entry Entry { get; set; }
    }
    
}
