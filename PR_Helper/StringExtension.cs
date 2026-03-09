using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PR_SPC
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        private static readonly Regex ControlCharReg = new Regex(@"[\p{C}]", RegexOptions.Compiled);

        /// <summary>
        /// 字符串移除非法字符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveControlChars(this string text)
        {
            return ControlCharReg.Replace(text, string.Empty).Replace("\r", "");
        }

        /// <summary>
        /// 判断是否整形
        /// </summary>
        /// <param name="text">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(this string text)
        {
            return Regex.IsMatch(text, @"^\d+$|^(\d+)(\.\d+)?$");
        }
    }
}
