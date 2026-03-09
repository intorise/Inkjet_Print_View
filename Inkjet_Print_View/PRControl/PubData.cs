using PR_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Spc_Tester.PRControl
{
    /// <summary>
    /// SPC分析数据集;
    /// </summary>
    public class PubData
    {
        /// <summary>
        /// 测试项目,可根据测试项目获取到对应的USL,LSL
        /// </summary>
        public static List<E_TestProject> TestProjectItems = new List<E_TestProject>();
    }
}
