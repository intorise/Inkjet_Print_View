using PR_DAL;
using PR_Model;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace PR_Spc_Tester.UserForms
{
    public partial class FrmLog : UIForm
    {
        SysLogDal dal = new SysLogDal();
        public FrmLog()
        {
            InitializeComponent();
        }

        private void FrmLog_Load(object sender, EventArgs e)
        {
            DateTime timeNow = DateTime.Now;
            string today = timeNow.ToString("yyyy-MM-dd");
            dtpStart.Value = Convert.ToDateTime($"{today} 00:00:00");
            dtpEnd.Value = Convert.ToDateTime($"{today} 23:59:59");
            BindData();
        }

        private void BindData() 
        {
            List<E_SysLog> list = null;
            string startTime = dtpStart.Value.ToString("G");
            string endTime = dtpEnd.Value.ToString("G");
            Task task = Task.Run(() => {
                list = dal.GetList(startTime, endTime);
            });
            task.ContinueWith(t =>
            {
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        dgv_log.Invoke(new Action(() => {
                            dgv_log.Rows.Clear();
                            dgv_log.Rows.Add(list.Count);
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgv_log.Rows[i].Cells["col_UserName"].Value = list[i].UserName;
                                dgv_log.Rows[i].Cells["col_Content"].Value = list[i].Content;
                                dgv_log.Rows[i].Cells["col_Time"].Value = list[i].AddTime;
                            }
                        }));
                    }
                }
            });
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
