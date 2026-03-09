using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PR_SPC
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举值的Description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription<T>(this T value) where T : struct
        {
            string result = value.ToString();
            Type type = typeof(T);
            FieldInfo info = type.GetField(value.ToString());
            if (info != null)
            {
                var attributes = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes != null && attributes.FirstOrDefault() != null)
                {
                    result = (attributes.First() as DescriptionAttribute).Description;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据Description获取枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetValueByDescription<T>(this string description) where T : struct
        {
            Type type = typeof(T);
            foreach (var field in type.GetFields())
            {
                if (field.Name == description)
                {
                    return (T)field.GetValue(null);
                }

                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes != null && attributes.FirstOrDefault() != null)
                {
                    if (attributes.First().Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "Description");
        }

        /// <summary>
        /// 获取string获取枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetValue<T>(this string value) where T : struct
        {
            T result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", value), "Value");
        }

        /// <summary>
        /// 根据Enum的FieldName得到对应的Description值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetDescriptionByFieldName<T>(string fieldName)
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                return "";
            }

            string desc = "";
            string[] arrFieldName = Enum.GetNames(type);

            foreach (string tmpFieldName in arrFieldName)
            {

                if (tmpFieldName.Equals(fieldName))
                {
                    //得到当前枚举项
                    FieldInfo fieldInfo = type.GetField(fieldName);
                    if (fieldInfo != null)
                    {
                        object[] arrAttr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组

                        if (arrAttr.Length > 0)
                        {
                            desc = ((DescriptionAttribute)arrAttr[0]).Description;   //属性描述
                        }
                        else
                        {
                            desc = fieldName;
                        }
                    }
                    break;
                }
            }

            return desc;
        }
    }
}
