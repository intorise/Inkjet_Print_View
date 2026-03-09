using PR_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_DAL
{
    public class PLCAlarmDal
    {
        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(PLCAlarm model)
        {
            model.AddTime = DateTime.Now;
            string sql = "insert into alarm_statistics(AlarmMessage,AddTime)";
            sql += " values(@AlarmMessage,@AddTime)";
            return DapperHelper.Execute(sql, model) > 0;
        }

        private string GetCondition(TestDataQuery param)
        {
            string condition = " where 1=1";
            condition += $" and AddTime<='{param.StartTime}'";
            return condition;
        }

        /// <summary>
        /// 获取分页数据;
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<PLCAlarm> GetList(TestDataQuery param)
        {
            int startIndex = (param.PageNum - 1) * param.PageSize;
            string sql = "select * from alarm_statistics";
            sql += GetCondition(param);
            sql += " order by ID desc";
            sql += $" limit {startIndex},{param.PageSize}";
            List<PLCAlarm> list = DapperHelper.Query<PLCAlarm>(sql);
            return list;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetCount(TestDataQuery param)
        {
            string sql = "select count(*) from alarm_statistics ";
            sql += GetCondition(param);
            return Convert.ToInt32(DapperHelper.ExecuteScalar(sql));
        }

        public int GetTotalCount()
        {
            string sql = "select count(*) from alarm_statistics";
            return Convert.ToInt32(DapperHelper.ExecuteScalar(sql));
        }
        /// <summary>
        /// 根据最新ID获取一个最新数据;
        /// </summary>
        /// <param name="lastID">上一次最新ID</param>
        /// <returns></returns>
        public TestData GetLastDataById(int lastID)
        {
            string sql = $"select * from test_data where ID>{lastID} order by ID desc limit 1";
            return DapperHelper.QueryFirstOrDefault<TestData>(sql);
        }
        /// <summary>
        /// 根据最新ID获取一个最新数据;
        /// </summary>
        /// <param name="lastID">上一次最新ID</param>
        /// <returns></returns>
        public TestData GetLastDataByCode(string code)
        {
            string sql = $"select * from test_data where Code='{code}' ORDER BY ID DESC LIMIT 1 ";
            return DapperHelper.QueryFirstOrDefault<TestData>(sql);
        }
    }
}
