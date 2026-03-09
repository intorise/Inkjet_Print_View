using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR_SPC
{
    /// <summary>
    /// 控件帮助类
    /// </summary>
    public static class ControlHelper
    {
        public delegate void Del();

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="forms"></param>
        /// <param name="del"></param>
        public static void Invoke(object forms, Del del)
        {
            Control c = forms as Control;
            try
            {
                if (c.Created && c.InvokeRequired)
                {
                    c.Invoke(del);
                }
                else
                {
                    del();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("跨线程更新UI错误：" + ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show("系统错误：" + ex.Message.ToString(), "System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 同步执行 注：外层Try Catch语句不能捕获Code委托中的错误
        /// </summary>
        static public void UIThreadInvoke(this Control control, Action Code)
        {
            try
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(Code);
                    return;
                }
                Code.Invoke();
            }
            catch
            {
                /*仅捕获、不处理！*/
            }
        }

        /// <summary>
        /// 异步执行 注：外层Try Catch语句不能捕获Code委托中的错误
        /// </summary>
        static public void UIThreadBeginInvoke(this Control control, Action Code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(Code);
                return;
            }
            Code.Invoke();
        }

    }
}
