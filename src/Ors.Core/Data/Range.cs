using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Data
{
    [Serializable]
    public class Range<T> where T : struct
    {
        public T? Left { get; set; }
        public T? Right { get; set; }
        /// <summary>
        /// 是否为左开区间，默认false
        /// </summary>
        public bool LeftOpen { get; set; }
        /// <summary>
        /// 是否为右开区间，默认false
        /// </summary>
        public bool RightOpen { get; set; }
    }
}
