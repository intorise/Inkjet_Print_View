using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using PR_SPC;

namespace PR_DAL
{
    public class DapperHelper
    {
        private static string connectionString =
       ConfigurationManager.AppSettings["ConString"].ToString();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public static List<T> Query<T>(string sql, object param = null) where T : class
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                IEnumerable<T> list = conn.Query<T>(sql, param);
                if (list != null)
                {
                    return list.ToList();
                }
                return null;
            }
        }
        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QueryFirst<T>(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.QueryFirst<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.QueryFirstOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QuerySingle<T>(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.QuerySingle<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QuerySingleOrDefault<T>(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.QuerySingleOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL执行参数</param>
        /// <param name="connectServer">远程数据库数据库连接</param>
        /// <returns></returns>
        public static int Execute(string sql, object param = null,string connectServer = null)
        {
            string connStr = connectionString;
            if(!string.IsNullOrEmpty(connectServer))
            {
                connStr = connectServer;
            }
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                return conn.Execute(sql, param);
            }
        }

        /// <summary>
        /// Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.ExecuteReader(sql, param);
            }
        }

        /// <summary>
        /// 获取Datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var reader = conn.ExecuteReader(sql, param);
                DataTable dt = new DataTable("myTable");
                dt.Load(reader);
                return dt;
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.ExecuteScalar(sql, param);
            }
        }

        /// <summary>
        /// 获取按条件的数量;
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public static int GetCount(string tableName, string whereStr)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = $"select count(*) from {tableName} where 1=1 {whereStr}";
                object count = conn.ExecuteScalar(sql, null);
                return Convert.ToInt32(count);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteScalarForT<T>(string sql, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.ExecuteScalar<T>(sql, param);
            }
        }

        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> ExecutePro<T>(string proc, object param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                List<T> list = conn.Query<T>(proc,
                    param,
                    null,
                    true,
                    null,
                    CommandType.StoredProcedure).ToList();
                return list;
            }
        }

        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <param name="param">param</param>
        /// <returns></returns>
        public static int ExecuteTransaction(List<string> sqlarr)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        for (int i = 0; i < sqlarr.Count; i++)
                        {
                            string sql = sqlarr[i];
                            result += conn.Execute(sql, null, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteErrLog(ex.Message);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <param name="Key">多条SQL</param>
        /// <param name="Value">param</param>
        /// <returns></returns>
        public static int ExecuteTransaction(Dictionary<string, object> dic)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in dic)
                        {
                            result += conn.Execute(sql.Key, sql.Value, transaction);
                        }
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteErrLog(ex.Message);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}
