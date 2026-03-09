using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    public class SamplingData
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public double temperature { get; set; }
        /// <summary>
        /// 压力
        /// </summary>
        public double pressure { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public double speed { get; set; }
        /// <summary>
        /// 浓度
        /// </summary>
        public double concentration { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public double position { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
    }
}
