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
    public partial class FrmParamEdit : UIForm
    {
        private E_TestProject _projectInfo;
        private bool isEdit = false;

        public FrmParamEdit(E_TestProject projectInfo)
        {
            _projectInfo = projectInfo;
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmParamEdit_Load(object sender, EventArgs e)
        {
            if(_projectInfo != null)
            {
                tb_projectName.Text = _projectInfo.ProjectName;
                tb_usl.Text = _projectInfo.USL_Val.ToString("F2");
                tb_lsl.Text = _projectInfo.LSL_Val.ToString("F2");
                cbb_sType.SelectedIndex = _projectInfo.SType - 1;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                //int sType = cbb_sType.SelectedIndex;
                //if(sType<0)
                //{
                //    MessageBox.Show("请选择规格类型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                _projectInfo.USL_Val = float.Parse(tb_usl.Text.Trim());
                _projectInfo.LSL_Val = float.Parse(tb_lsl.Text.Trim());
               // _projectInfo.SType = sType+1;

                SysLogDal _LogDal = new SysLogDal();
                TestProjectDal dal = new TestProjectDal();
                bool result = dal.Update(_projectInfo);
                if (result)
                {
                    //string sTypestr = _projectInfo.SType == 1 ? "单侧" : "双侧";
                    string tip = $"修改参数成功,修改[{_projectInfo.ProjectName}]为:{_projectInfo.LSL_Val}-{_projectInfo.USL_Val}";
                    _LogDal.AddLog(new E_SysLog
                    {
                        UserName = PubInfo.UserName,
                        LogType = LogMsgType.Info,
                        Content = $"用户{PubInfo.UserName}" + tip
                    }); 
                    
                    UIMessageBox.ShowSuccess(tip);
                    isEdit = true;
                }
                else
                {
                    PubCommon.ShowMessageBox_Error("修改参数信息失败！");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("请填写正确后提交","错误",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void FrmParamEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isEdit)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
