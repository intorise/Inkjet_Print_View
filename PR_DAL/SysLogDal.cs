using PR_Model;
using PR_SPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_DAL
{
    public class SysLogDal
    {
        public SysLogDal() { }

        /// <summary>
        /// 添加一个日志;
        /// </summary>
        /// <param name="logInfo"></param>
        /// <returns></returns>
        public bool AddLog(E_SysLog logInfo)
        {
            string sql = "insert into SysLog(UserName,Content,LogType)";
            sql += $" values('{PubInfo.UserName}','{logInfo.Content}',{(int)logInfo.LogType})";
            return DapperHelper.Execute(sql, null) > 0 ? true : false;
        }

        /// <summary>
        /// 获取日志记录
        /// </summary>
        /// <param name="log">查询条件对象</param>
        /// <returns></returns>
        public List<E_SysLog> GetList(string startTime,string endTime)
        {
            string sql = "select * from SysLog where 1=1 ";
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                sql += $" and AddTime >=  '{startTime}'";
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                sql += $" and AddTime <=  '{endTime}'";
            }
            return DapperHelper.Query<E_SysLog>(sql, null);
        }
    }
}
