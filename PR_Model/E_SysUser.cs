using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class E_SysUser
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

        /// <summary>
        /// 用户类型，根据客户需求来修改 
        ///1:管理员 2：工艺 3：操作员
        /// </summary>
        public int UserType { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
