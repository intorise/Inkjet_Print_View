using PRBakerProject.Moudules.Waiting;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PR_DAL;
using PR_Model;
using PR_SPC;
using Masuit.Tools.Security;

namespace PR_Spc_Tester.UserForms
{
    public partial class FrmLogin : UIForm
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 登录点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_login_Click(object sender, EventArgs e)
        {
            string userName = this.txt_userName.Text;
            string password = this.txt_Password.Text;

            if(string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("请填写完整后提交", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                LoadingHelper.ShowLoadingScreen("登录中...请稍等");
                SysUserDal  userDal = new SysUserDal();
                SysLogDal _LogDal = new SysLogDal();
                E_SysUser userInfo = userDal.GetUserInfo(userName);
                if(userInfo != null)
                {
                    string pwd_md5 = password.MDString(ConfigAppSettings.GetValue("Md5_Salt"));
                    if (pwd_md5 != userInfo.PassWord)
                    {
                        MessageBox.Show($"用户{userName}密码错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _LogDal.AddLog(new E_SysLog()
                        {
                            UserName = userName,
                            LogType = LogMsgType.Error,
                            Content = $"用户{userName}密码错误!"
                        });
                    }
                    else
                    {
                        PubInfo.UserName = userInfo.UserName;
                        PubInfo.UserType = userInfo.UserType;

                        string[] userTypes = new string[] { "管理员", "工艺", "操作员" };
                        PubInfo.UserTypeStr = userTypes[userInfo.UserType-1];

                        _LogDal.AddLog(new E_SysLog()
                        {
                            UserName = userName,
                            LogType = LogMsgType.Info,
                            Content = $"用户{userName}登录成功!"
                        });
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    MessageBox.Show($"用户{userName}不存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadingHelper.CloseForm();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"登录异常：" + ex.Message);
                MessageBox.Show($"登录异常:{ex.Message}");
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lb_version.Text = "当前版本：V"+version;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
