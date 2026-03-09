using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    public class CacheLog
    {
        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public int MsgType { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime LogTime { get; set; }
    }
}
