using PR_Model;
using PR_SPC;
using System;
using System.Collections.Generic;

namespace PR_DAL
{
    /// <summary>
    /// 用户数据操作
    /// </summary>
    public class SysUserDal
    {
        public SysUserDal() { }

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public E_SysUser GetUserInfo(string UserName)
        {
            try
            {
                string sql = $"select * from SysUser where UserName='{UserName}' limit 1";
                E_SysUser user = DapperHelper.QueryFirstOrDefault<E_SysUser>(sql, null);
                return user;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"获取用户信息失败：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 判断是否已经存在该用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsExisted(string userName)
        {
            try
            {
                string sql_check = "select count(*) from sysuser where UserName=@UserName";
                if (Convert.ToInt32(DapperHelper.ExecuteScalar(sql_check, new
                {
                    userName
                })) == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("判断用户是否存在异常:" + ex.Message + "\r\n" + ex.StackTrace);
                return true;
            }
            
        }

        /// <summary>
        /// 添加用户;
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Add(E_SysUser user)
        {
            try
            {
                string sql = $"insert into sysuser(UserName,PassWord,UserType) values('{user.UserName}','{user.PassWord}','{user.UserType}')";
                int ret = DapperHelper.Execute(sql);
                return ret > 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"添加用户异常：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="user">待修改的用户信息</param>
        /// <returns></returns>
        public bool Edit(E_SysUser user)
        {
            string sql = $"update sysuser set PassWord=@PassWord,UserType=@UserType";
            sql += $" where ID=@ID";
            int ret = DapperHelper.Execute(sql,new
            {
                user.PassWord,
                user.UserType,
                user.ID
            });
            return ret > 0;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Delete(E_SysUser user)
        {
            string sql = $"delete from sysuser where ID={user.ID}";
            int ret = DapperHelper.Execute(sql);
            return ret > 0;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<E_SysUser> GetList()
        {
            string sql = "select * from sysuser order by ID asc";
            return DapperHelper.Query<E_SysUser>(sql, null);
        }
    }
}
