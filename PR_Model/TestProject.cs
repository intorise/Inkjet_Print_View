using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    /// <summary>
    /// 测试内容
    /// </summary>
    public class TestProject
    {
        public int ID { get; set; }

        /// <summary>
        /// 测试项目
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal Usl_Val { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal Lsl_Val { get; set; }

        public int SType { get; set; } = 2;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
