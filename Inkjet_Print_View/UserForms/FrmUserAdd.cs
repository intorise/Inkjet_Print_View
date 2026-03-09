using Masuit.Tools.Security;
using PR_DAL;
using PR_Model;
using PR_SPC;
using PR_Spc_Tester.Common;
using Sunny.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR_Spc_Tester.UserForms
{
    /// <summary>
    /// 用户添加编辑;
    /// </summary>
    public partial class FrmUserAdd : UIForm
    {
        private readonly SysUserDal userDal = new SysUserDal();
        private readonly SysLogDal logDal = new SysLogDal();

        private E_SysUser _editUser;
        private readonly string[] role_arr = new string[] { "管理员", "工艺员", "操作员" };

        public FrmUserAdd(E_SysUser editUser=null)
        {
            InitializeComponent();
            _editUser = editUser;
            if(editUser != null )
            {
                uiTxtUserName .Text = editUser.UserName;
                uiCmbUserType.SelectedIndex = editUser.UserType-1;
            }
        }

        private void FrmUserAdd_Load(object sender, EventArgs e)
        {
            if (_editUser != null)
            {
                uiTxtUserName.Enabled = false;
            }           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (PubInfo.UserType != 1)
            {
                PubCommon.ShowMessageBox_Warning("当前登录用户类型没有进行该操作的权限！");
            }
            else
            {
                string password = uiTxtPassword.Text.Trim();
                string userName = uiTxtUserName.Text.Trim();
                int sUserType = uiCmbUserType.SelectedIndex+1;
                if (string.IsNullOrEmpty(userName))
                {
                    PubCommon.ShowMessageBox_Warning("用户名不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(password))
                {
                    PubCommon.ShowMessageBox_Warning("密码不能为空！");
                    return;
                }
                if (sUserType <= 0)
                {
                    PubCommon.ShowMessageBox_Warning("用户类型不能为空！");
                    return;
                }
                
                if (_editUser!=null)
                {
                    _editUser.UserName = userName;
                    _editUser.PassWord = password.MDString(ConfigAppSettings.GetValue("Md5_Salt"));
                    _editUser.UserType = sUserType;

                    bool ret = userDal.Edit(_editUser);
                    if (ret)
                    {
                        string tip = $"编辑{_editUser.UserName}信息成功!";
                        logDal.AddLog(new E_SysLog()
                        {
                            UserName = PubInfo.UserName,
                            LogType = LogMsgType.Info,
                            Content = $"用户{PubInfo.UserName}"+tip
                        });

                        UIMessageBox.ShowSuccess(tip);
                    }
                    else
                    {
                        PubCommon.ShowMessageBox_Error("编辑用户信息失败！");
                    }

                } else
                {
                    //添加用户;
                    E_SysUser userModel = new E_SysUser();
                    userModel.UserName = uiTxtUserName.Text;
                    userModel.PassWord = password.MDString(ConfigAppSettings.GetValue("Md5_Salt"));
                    userModel.UserType = sUserType;

                    if (userDal.IsExisted(userModel.UserName))
                    {
                        PubCommon.ShowMessageBox_Error($"用户名:{userModel.UserName}已存在,请更换用户名!");
                        return;
                    }

                    bool result = userDal.Add(userModel);
                    if (result)
                    {
                        string tip = $"添加{userModel.UserName}信息成功!";
                        logDal.AddLog(new E_SysLog()
                        {
                            UserName = PubInfo.UserName,
                            LogType = LogMsgType.Info,
                            Content = $"用户{PubInfo.UserName}" + tip
                        }) ;
                        UIMessageBox.ShowSuccess(tip);
                    }
                    else
                    {
                        PubCommon.ShowMessageBox_Error("添加用户信息失败！");
                    }
                }
                
                
            }
        }
    }
}
