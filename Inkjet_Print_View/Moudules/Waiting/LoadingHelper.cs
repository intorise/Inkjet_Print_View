using System;
using System.Threading;

namespace PRBakerProject.Moudules.Waiting
{
    /// <summary>
    /// 加载窗口
    /// </summary>
    public class LoadingHelper
    {
        #region 相关变量定义
        // Usage
        // LoadingHelper.ShowLoadingScreen();//显示
        // LoadingHelper.CloseForm();//关闭
        /// <summary>
        /// 定义委托进行窗口关闭
        /// </summary>
        private delegate void CloseDelegate();
        private static WaitingForm loadingForm;
        private static string tip_info = "";
        private static readonly Object syncLock = new Object();  //加锁使用

        #endregion

        //private LoadingHelper()
        //{

        //}

        /// <summary>
        /// 显示loading框
        /// </summary>
        public static void ShowLoadingScreen(string info = "")
        {
            // Make sure it is only launched once.
            if (loadingForm != null)
                return;
            tip_info = info;
            Thread thread = new Thread(new ThreadStart(LoadingHelper.ShowForm));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

        }
        /// <summary>
        /// 显示窗口
        /// </summary>
        private static void ShowForm()
        {
            if (loadingForm != null)
            {
                loadingForm.closeOrder();
                loadingForm = null;
            }
            loadingForm = new WaitingForm();
            loadingForm.TopMost = true;
            loadingForm.lb_info.Text = tip_info;
            loadingForm.ShowDialog();
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static void CloseForm()
        {
            Thread.Sleep(50); //可能到这里线程还未起来，所以进行延时，可以确保线程起来，彻底关闭窗口
            if (loadingForm != null)
            {
                lock (syncLock)
                {
                    Thread.Sleep(50);
                    if (loadingForm != null)
                    {
                        Thread.Sleep(50);  //通过三次延时，确保可以彻底关闭窗口
                        loadingForm.Invoke(new CloseDelegate(LoadingHelper.CloseFormInternal));
                    }
                }
            }
        }
        /// <summary>
        /// 关闭窗口，委托中使用
        /// </summary>
        private static void CloseFormInternal()
        {
            loadingForm.closeOrder();
            loadingForm = null;
        }
    }
}
