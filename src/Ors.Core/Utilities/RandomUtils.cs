using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Utilities
{
    public static class RandomUtils
    {
        /// <summary>
        /// 返回指定长度由随机数字组成的字符串
        /// </summary>
        /// <param name="length">指定的字符串长度</param>
        /// <param name="maxZeroCount">其中最多包含多少0</param>
        /// <returns>长度为count的随机字符串</returns>
        public static string Number(int length = 6, int maxZeroCount = 2)
        {
            if (length <= 0) return "";
            var sb = new StringBuilder(length);
            var rand = new Random();
            var numbers = new int[] { 2, 3, 9, 4, 1, 5, 8, 6, 0, 7 };
            var numbersNine = new int[] { 6, 1, 3, 9, 5, 7, 8, 2, 4 };
            var zeroCount = 0;
            for (var index = 0; index < length; index++)
            {
                int num;
                if (zeroCount > maxZeroCount)
                {
                    num = numbersNine[rand.Next(9)];
                }
                else
                {
                    num = numbers[rand.Next(10)];
                    if (num == 0)
                    {
                        zeroCount++;
                    }
                }

                sb.Append(num);
            }
            return sb.ToString();
        }

        private static readonly char[] alphabeticlist =
            "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        /// <summary>
        /// 返回指定长度的随机字符串(由数字和字母组成)
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>字符串</returns>
        public static string Alpha(int length)
        {
            if (length <= 0) return "";
            var sb = new StringBuilder(length);
            var rand = new Random();
            int alphabeticLength = 62;
            for (var index = 0; index < length; index++)
            {
                sb.Append(alphabeticlist[rand.Next(alphabeticLength - 1)]);
            }
            return sb.ToString();
        }
    }
}
