using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    public class TestDataQuery
    {
        /// <summary>
        /// 电池条码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize {  get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 搜索开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 搜索结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}
