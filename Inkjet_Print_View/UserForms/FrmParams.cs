using PR_DAL;
using PR_Model;
using PR_SPC;
using PR_Spc_Tester.Common;
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

namespace PR_Spc_Tester.UserForms
{
    public partial class FrmParams : UIForm
    {
        SysLogDal _LogDal = new SysLogDal();
        TestProjectDal dal = new TestProjectDal();
        List<E_TestProject> list = null;
        public FrmParams()
        {
            InitializeComponent();
        }

        private void FrmParams_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData() 
        {
            list = dal.GetList();
            if (list!=null)
            {
                if (list.Count>0)
                {
                    dgv_TestProject.Rows.Clear();
                    dgv_TestProject.Rows.Add(list.Count);
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgv_TestProject.Rows[i].Cells[0].Value = list[i].Id;
                        dgv_TestProject.Rows[i].Cells[1].Value = list[i].ProjectName;
                        dgv_TestProject.Rows[i].Cells[2].Value = list[i].USL_Val.ToString();
                        dgv_TestProject.Rows[i].Cells[3].Value = list[i].LSL_Val.ToString() ;
                        dgv_TestProject.Rows[i].Cells[4].Value = list[i].UpdateTime.ToString();
                    }
                }
            }
        }


        private void dgv_TestProject_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
                int row = e.RowIndex;
                if (row < 0)
                {
                    return;
                }
                int col = e.ColumnIndex;
                try
                {
                    if (col == dgv_TestProject.ColumnCount-1)
                    {
                        if (PubInfo.UserType == 2 || PubInfo.UserType == 1)
                        {
                            E_TestProject model = list[row];
                            FrmParamEdit frm = new FrmParamEdit(model);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                BindData();
                            }
                        }
                        else
                        {
                            PubCommon.ShowMessageBox_Warning("当前登录用户类型没有[工艺]-[管理员]的权限！");
                        }
                    }
                }catch(Exception ex)
                {
                    PubCommon.ShowMessageBox_Warning("请输入数据类型错误,请重新输入！");
                }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
