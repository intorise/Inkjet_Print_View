using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PR_SPC
{

    public class CommonUtil
    {
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取和校验值
        /// </summary>
        /// <param name="memorySpage"></param>
        /// <returns></returns>
        public static int GetSumCheck(int[] memorySpage)
        {
            int num = 0;
            for (int i = 0; i < memorySpage.Length; i++)
            {
                num = (num + memorySpage[i]) % 0xffff;
            }
            return num;
        }
        public static string Trimchar(string title)
        {

            var listSign = new List<string> { "|", "'", ",", "&", ".", "!" };
            var notLetter = Regex.Split(title, @"[a-zA-Z]/[0-9]", RegexOptions.IgnoreCase).Where(r => r.Trim() != string.Empty).ToList();
            var newLetter = new List<string>();

            for (int i = notLetter.Count - 1; i >= 0; i--)
            {
                if (notLetter[i].Trim().Length == 0)
                {
                    notLetter.RemoveAt(i);
                    continue;
                }

                if (notLetter[i].Trim().Length > 1)
                {
                    for (int j = 0; j < notLetter[i].Trim().Length; j++)
                    {
                        newLetter.Add(notLetter[i].Trim().Substring(j, 1));
                    }
                    notLetter.RemoveAt(i);
                }
            }

            notLetter.AddRange(newLetter);
            foreach (string sign in notLetter)
            {
                if (sign.Trim().Length == 0) continue;

                if (!listSign.Contains(sign.Trim()))
                {
                    title = title.Replace(sign.Trim(), "");
                }
            }
            return title;

        }

        /// <summary>
        /// 根据字节数组获取字符串
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string GetRealStr(byte[] datas)
        {
            if (datas == null || datas.Length == 0)
            {
                return "";
            }
            string temp = Encoding.ASCII.GetString(datas).Replace("\n", "").Replace("\r\n", "").Trim().Replace("\r", "");

            temp = temp.Replace("\u001e", "").Replace("\u0006", "").Replace("\u0018", "").Replace("\u0013", "").Replace("\a", "").Replace("\u0019", "");
            temp = temp.Replace("\f", "").Replace("u0019", "").Replace("\\", "");
            temp = temp.Replace("\u0000", "");

            return temp;
        }

        /// <summary>
        /// 科学计数法字符串转数字;
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static Decimal ChangeDataToD(string strData)
        {
            Decimal dData = 0.0M;
            if (strData.Contains("E"))
            {
                dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            return dData;
        }

        /// <summary>
        /// 反射得到实体类的字段名称和值
        /// var dict = GetProperties(model);
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="t">实例化</param>
        /// <returns></returns>
        public static Dictionary<object, object> GetProperties<T>(T t)
        {
            var ret = new Dictionary<object, object>();
            if (t == null) { return null; }
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    ret.Add(name, value);
                }
            }
            return ret;
        }
    

        /// <summary>
        /// 将字符串转成List
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static List<string> StringConvertList(string str, char splitChar)
        {
            List<string> list = new List<string>();
            string[] arr = str.Split(splitChar);
            for (int i = 0; i < arr.Length; i++)
            {
                list.Add(arr[i]);
            }
            return list;
        }

        /// <summary>
        /// 将数据转为Ascii码
        /// </summary>
        /// <param name="hexstring"></param>
        /// <returns></returns>
        public static string HexStringToASCII11(short hexstring)
        {
            string lin = hexstring.ToString();
            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }
    }
}
