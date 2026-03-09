using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_SPC
{
    public class LogHelper
    {
        private static object logErr = new object();
        private static object obj = new object();
        private static object objMes = new object();
        private static object objSchedule = new object();

        /// <summary>
        /// 写入错误Log
        /// </summary>
        /// <param name="strLog">日志内容</param>
        public static void WriteErrLog(string strLog)
        {
            lock (logErr)
            {
                string sFilePath = ConfigurationManager.AppSettings["LogErrPath"].ToString();
                string sFileName = "e_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                sFileName = sFilePath + "\\" + sFileName;
                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                FileStream fs;
                StreamWriter sw;
                if (File.Exists(sFileName))
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + strLog);
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 写入日志Log
        /// </summary>
        /// <param name="strLog">日志内容</param>
        public static void WriteLog(string strLog)
        {
            lock (obj)
            {
                string sFilePath = ConfigurationManager.AppSettings["LogPath"].ToString();
                string sFileName = "Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                sFileName = sFilePath + "\\" + sFileName;
                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                FileStream fs;
                StreamWriter sw;
                if (File.Exists(sFileName))
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + strLog);
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// Mes交互的日志;
        /// </summary>
        /// <param name="loginfo"></param>
        public static void WriteWmsLog(string strLog)
        {
            lock (objMes)
            {
                string sFilePath = ConfigurationManager.AppSettings["LogWmsPath"].ToString();
                string sFileName = "Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                sFileName = sFilePath + "\\" + sFileName;
                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                FileStream fs;
                StreamWriter sw;
                if (File.Exists(sFileName))
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + strLog);
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 写入定时日志
        /// </summary>
        /// <param name="strLog"></param>
        public static void WriteScheduleLog(string strLog)
        {
            lock (objSchedule)
            {
                string sFilePath = ConfigurationManager.AppSettings["LogSchedulePath"].ToString();
                string sFileName = "Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                sFileName = sFilePath + "\\" + sFileName;
                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                FileStream fs;
                StreamWriter sw;
                if (File.Exists(sFileName))
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + strLog);
                sw.Close();
                fs.Close();
            }
        }
    }
}
