using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PR_Model;
using PR_DAL;

namespace PR_Spc_Tester.Common
{
    public class PubCommon
    {


        /// <summary>
        /// 提示 --Warning
        /// </summary>
        /// <param name="str"></param>
        public static void ShowMessageBox_Warning(string str)
        {
            MessageBox.Show(str, "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 提示错误信息
        /// </summary>
        /// <param name="errMsg">错误消息</param>
        public static void ShowMessageBox_Error(string errMsg)
        {
            MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
