using PR_DAL;
using PR_Model;
using PR_SPC;
using PR_Spc_Tester.Common;
using PR_Spc_Tester.Services;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PR_Spc_Tester.UserForms
{
    public partial class FrmUser : UIForm
    {
        private List<E_SysUser> list = null;
        private readonly SysUserDal userdal = new SysUserDal();
        private readonly SysLogDal _LogDal = new SysLogDal();
        public FrmUser()
        {
            InitializeComponent();
        }

        private void FrmUser_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 加载数据;
        /// </summary>
        public void BindData()
        {
            list = userdal.GetList();
            uiDgv_user.Rows.Clear();
            uiDgv_user.Rows.Add(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                uiDgv_user.Rows[i].Cells["col_ID"].Value = list[i].ID.ToString();
                uiDgv_user.Rows[i].Cells["col_UserName"].Value = list[i].UserName.ToString();
                uiDgv_user.Rows[i].Cells["col_UserType"].Value = list[i].UserType.ToString();
                uiDgv_user.Rows[i].Cells["col_AddTime"].Value = list[i].AddTime.ToString();
            }
        }



        /// <summary>
        /// 单元格显示效果
        /// </summary>
        private void uiDgv_user_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (uiDgv_user.Columns[e.ColumnIndex].Name == "col_UserType")
            {
                if (e.Value.ToString() == "1")
                {
                    e.Value = "管理员";
                }
                if (e.Value.ToString() == "2")
                {
                    e.Value = "工艺员";
                }
                if (e.Value.ToString() == "3")
                {
                    e.Value = "操作员";
                }
            }
        }

        /// <summary>
        /// 删除编辑用户;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiDgv_user_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (uiDgv_user.Columns[e.ColumnIndex].Name == "ColDelete")
                {
                    int id = Convert.ToInt32(uiDgv_user.Rows[e.RowIndex].Cells["col_ID"].Value.ToString());
                    string name = uiDgv_user.Rows[e.RowIndex].Cells["col_UserName"].Value.ToString();
                    E_SysUser model = new E_SysUser
                    {
                        ID = id
                    };
                    if (PubInfo.UserType == 1)
                    {
                        if (MessageBox.Show($"确认删除[{name}]的用户信息吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bool result = userdal.Delete(model);
                            if (result)
                            {
                                string tip = $"删除用户[{name}]信息成功!";
                                _LogDal.AddLog(new E_SysLog()
                                {
                                    UserName = PubInfo.UserName,
                                    LogType = LogMsgType.Info,
                                    Content = $"{PubInfo.UserName}"+ tip
                                });
                                BindData();
                                LogService.AddLogToEnqueue(tip);
                            }
                            else
                            {
                                PubCommon.ShowMessageBox_Error("删除用户信息失败");
                            }
                        }
                    }
                    else
                    {
                        PubCommon.ShowMessageBox_Warning("当前登录用户没有进行该操作的权限！");
                    }
                }
                else if (uiDgv_user.Columns[e.ColumnIndex].Name == "ColEdit")
                {
                    E_SysUser model = list[e.RowIndex];
                    FrmUserAdd frmEdit = new FrmUserAdd(model);
                    frmEdit.ShowDialog();
                    BindData();
                }
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            FrmUserAdd frmUserAdd = new FrmUserAdd();
            frmUserAdd.ShowDialog();
            BindData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
