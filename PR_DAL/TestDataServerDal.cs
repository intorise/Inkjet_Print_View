using PR_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_DAL
{
    /// <summary>
    /// 远程电脑数据库操作;
    /// </summary>
    public class TestDataServerDal
    {
        private string _connstr = string.Empty;
        public TestDataServerDal(string connstr) 
        { 
            _connstr = connstr;
        }

        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connectStr">远程连接字符串</param>
        /// <returns></returns>
        public bool Add(TestData model)
        {
            model.AddTime= DateTime.Now;
            string sql = "insert into test_data(Code,PreSprayWeight,PostSprayWeight,SedimentationWeight,UpperLimit,LowerLimit,Result,AddTime)";
            sql += " values(@Code,@PreSprayWeight,@PostSprayWeight,@SedimentationWeight,@UpperLimit,@LowerLimit,@Result," +
                "@AddTime)";
            return DapperHelper.Execute(sql, model, _connstr) > 0;
        }
    }
}
