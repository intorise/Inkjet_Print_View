using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    /// <summary>
    /// 测试项目
    /// </summary>
    public class E_TestProject
    {
        public int Id { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 测试单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 标准上限
        /// </summary>
        public float USL_Val { get; set; }

        /// <summary>
        /// 标准下限
        /// </summary>
        public float LSL_Val { get; set; }

        public int SType { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 当前测试数据
        /// </summary>
        public List<float> Datas { get; set; } = new List<float>();
    }
}
