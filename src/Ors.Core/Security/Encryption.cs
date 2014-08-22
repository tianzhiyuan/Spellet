using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Security
{
    public static class Encryption
    {
        private static string Reverse(string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private const string Base62CodingSpace = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private const string Clist = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly char[] Clistarr = Clist.ToCharArray();

        /// <summary>
        /// MD5 算法
        /// </summary>
        public const string MD5 = "MD5";
        /// <summary>
        /// SHA1算法
        /// </summary>
        public const string SHA1 = "SHA1";

        /// <summary>
        /// 将指定的字符串取哈希值，结果以十六进制字符串展示
        /// 如 "origin" ===> "327B5A9328F00E5C67B213E5A44A28A1"
        /// </summary>
        /// <param name="content">待哈希的字符串</param>
        /// <param name="format">算法(默认MD5)</param>
        /// <returns></returns>
        public static string Hash(this string content, string format = null)
        {
            if (content == null) return null;
            return Hash(content, "", format);
        }
        /// <summary>
        /// 将指定的字符串与哈希盐混淆取哈希值，结果以十六进制字符串返回
        /// </summary>
        /// <param name="content">待哈希的字符串</param>
        /// <param name="salt">哈希盐</param>
        /// <param name="format">算法(默认MD5)</param>
        /// <returns></returns>
        public static string Hash(this string content, string salt, string format)
        {
            if (content == null) return null;
            var contentAndMagic = content + salt;
            if (String.IsNullOrWhiteSpace(format))
            {
                format = MD5;
            }
            using (var algorithm = HashAlgorithm.Create(format))
            {
                if (algorithm == null)
                {
                    throw new ArgumentException("Hash Format");
                }
                var byteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(contentAndMagic));
                return BitConverter.ToString(byteArray).Replace("-", "");
            }
        }

        /// <summary>
        /// 将指定的字符串取哈希值，结果以Base64编码返回
        /// 例如 "orgin" ===> MntakyjwDlxnshPlpEoooQ==
        /// </summary>
        /// <param name="content">待哈希字符串</param>
        /// <param name="format">算法(默认MD5)</param>
        /// <returns></returns>
        public static string HashBase64(this string content, string format = null)
        {
            if (content == null) return null;
            return HashBase64(content, "", format);
        }
        /// <summary>
        /// 将指定的字符串取哈希值，结果以Base64编码返回
        /// 例如 "orgin" ===> MntakyjwDlxnshPlpEoooQ==
        /// </summary>
        /// <param name="content">待哈希字符串</param>
        /// <param name="salt">哈希盐</param>
        /// <param name="format">算法(默认MD5)</param>
        /// <returns></returns>
        public static string HashBase64(this string content, string salt, string format)
        {
            if (content == null) return null;
            var contentAndMagic = content + salt;
            if (String.IsNullOrWhiteSpace(format))
            {
                format = MD5;
            }
            using (var algorithm = HashAlgorithm.Create(format))
            {
                if (algorithm == null)
                {
                    throw new ArgumentException("Hash Format");
                }
                var byteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(contentAndMagic));
                var array = Convert.ToBase64String(byteArray);
                return array;
            }
        }

        /// <summary>
        /// 验证哈希字符串是否匹配指定字符串
        /// </summary>
        /// <param name="hashedTxt">哈希后的字符串</param>
        /// <param name="plainTxt">待检验字符串</param>
        /// <param name="format">哈希算法(默认MD5)</param>
        /// <returns></returns>
        public static bool IsMatch(string hashedTxt, string plainTxt, string format = null)
        {
            var encrypted = plainTxt.Hash(format);
            return encrypted == hashedTxt;
        }
        /// <summary>
        /// 验证哈希字符串是否匹配指定字符串
        /// </summary>
        /// <param name="hashedTxt">哈希后的字符串</param>
        /// <param name="plainTxt">待检验字符串</param>
        /// <param name="salt">哈希盐</param>
        /// <param name="format">哈希算法(默认MD5)</param>
        /// <returns></returns>
        public static bool IsMatch(string hashedTxt, string plainTxt, string salt, string format)
        {
            var encryted = plainTxt.Hash(salt, format);
            return encryted == hashedTxt;
        }

        /// <summary>
        /// 将Base36编码的字符串转换为相应的数字
        /// </summary>
        /// <param name="inputString">Base36编码的字符串</param>
        /// <returns>相应的数字,如果格式错误返回-1</returns>
        public static long Base36Decode(string inputString)
        {
            long result = 0;
            var pow = 0;
            for (var i = inputString.Length - 1; i >= 0; i--)
            {
                var c = inputString[i];
                var pos = Clist.IndexOf(c);
                if (pos > -1)
                    result += pos * (long)Math.Pow(Clist.Length, pow);
                else
                    return -1;
                pow++;
            }
            return result;
        }
        /// <summary>
        /// 将数字以Base36方式编码
        /// </summary>
        /// <param name="inputNumber">待编码数字</param>
        /// <returns>编码字符串</returns>
        public static string Base36Encode(long inputNumber)
        {
            var sb = new StringBuilder();
            do
            {
                sb.Append(Clistarr[inputNumber % (long)Clist.Length]);
                inputNumber /= (long)Clist.Length;
            } while (inputNumber != 0);
            return Reverse(sb.ToString());
        }

        /// <summary>
        /// 将数字以Base62算法编码
        /// </summary>
        /// <param name="inputNumber">待编码数字</param>
        /// <returns>编码字符串</returns>
        public static string ToBase62(long inputNumber)
        {
            var result = new StringBuilder();
            do
            {
                var a = inputNumber % 62;
                result.Insert(0, Base62CodingSpace[(int)a]);
                inputNumber = inputNumber / 62;
            } while (inputNumber > 0);
            return result.ToString();
        }
    }
}
