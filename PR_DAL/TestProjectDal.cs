using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR_Model;

namespace PR_DAL
{
    /// <summary>
    /// SPC分析项目
    /// </summary>
    public class TestProjectDal
    {
        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <returns></returns>
        public List<E_TestProject> GetList()
        {
            string sql = "select * from test_project order by ID asc";
            return DapperHelper.Query<E_TestProject>(sql, null);
        }

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <returns></returns>
        public bool Update(E_TestProject model)
        {
            string sql = $"update test_project set ProjectName='{model.ProjectName}',USL_Val ='{model.USL_Val}',LSL_Val='{model.LSL_Val}',UpdateTime ='{model.UpdateTime}' where ID={model.Id}";
            int ret = DapperHelper.Execute(sql);
            return ret > 0;
        }
    }
}
