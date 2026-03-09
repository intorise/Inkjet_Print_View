using MiniExcelLibs;
using OfficeOpenXml;
using PR_DAL;
using PR_Model;
using PR_SPC;
using PR_Spc_Tester.UserForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR_Spc_Tester
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //限制多程序启动
            bool createdNew;
            Mutex instance = new Mutex(true, "数据采集软件v1.0", out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("程序已启动,请不要启动多个程序!");
                GC.Collect();
                Environment.Exit(0);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
           
            Application.Run(new FrmMain());
        }
    }
}
