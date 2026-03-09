using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_SPC
{
    public class ConfigAppSettings
    {
        /// <summary>
        /// 设置配置值
        /// </summary>
        /// <param name="key">appsettings键值</param>
        /// <param name="value">配置值</param>
        public static void SetValue(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">appsettings键值</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                return "";
            }
            else
            {
                return config.AppSettings.Settings[key].Value;
            }
        }
    }
}
