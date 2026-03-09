using PR_Model;
using PR_SPC;
using System;
using System.Collections.Concurrent;

namespace PR_Spc_Tester.Services
{
    /// <summary>
    /// 应用上下文；
    /// </summary>
    public class LogService
    {
        /// <summary>
        /// 信号
        /// </summary>
        private static readonly object _lockerOne = new object();
        private static readonly object _lockerTwo = new object();

        /// <summary>
        /// 记录消息Queue
        /// </summary>
        public static ConcurrentQueue<CacheLog> LogQueue { set; get; } = new ConcurrentQueue<CacheLog>();

        #region 添加日志到队列
        /// <summary>
        /// 添加日志到队列
        /// </summary>
        /// <param name="msgType">日志类型</param>
        /// <param name="msg">日志信息</param>
        /// <param name="isShow">是否显示</param>
        public static void AddLogToEnqueue( string msg, EnumMsgType msgType=EnumMsgType.Info, bool isShow=true)
        {
            lock (_lockerOne)
            {
                LogQueue.Enqueue(new CacheLog()
                {
                    Message = msg,
                    MsgType = (int)msgType,
                    IsShow = isShow
                });
            }
        }
        #endregion

        #region 获取日志
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <returns></returns>
        public static CacheLog GetLog()
        {
            lock (_lockerOne)
            {
                CacheLog log = null;
                bool isDequeue = LogQueue.TryDequeue(out log);
                return log;
            }
        }
        #endregion

        #region 判断是否需要写日志
        /// <summary>
        /// 判断是否需要写日志
        /// </summary>
        /// <param name="station">站台对象</param>
        /// <param name="enumMsgType">日志类型</param>
        /// <param name="msg">日志</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        public static void WriteLog(EnumMsgType enumMsgType, string msg, bool isShow)
        {

            lock (_lockerTwo)
            {
                try
                {
                    bool result = false;

                    string logMsg = $"{msg}";

                    //日志列表里无记录
                    if (!result)
                    {
                        AddLogToEnqueue(logMsg,enumMsgType, isShow);//将日志添加到队列中
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog($"写日志异常，消息：{ex.Message}");
                }
            }
        }
        #endregion

        #region 删除日志
        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="taskNo">任务号</param>
        /// <returns></returns>
        public static void RemoveLog(int taskNo)
        {

        }
        #endregion
    }
}
