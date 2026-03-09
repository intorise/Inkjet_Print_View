using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    /// <summary>
    /// 系统操作日志实体类
    /// </summary>
    public class E_SysLog
    {
        public int ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogMsgType LogType { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime? AddTime { get; set; }
    }
}
