using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    public class PLCAlarm
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 报警内容
        /// </summary>
        public string AlarmMessage { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }
    }
}
