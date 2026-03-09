using System;
using System.Windows.Forms;

namespace PRBakerProject.Moudules.Waiting
{
    public partial class WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();
        }
        public delegate void SetUISomeInfo();
        private void WaitingForm_Load(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }
        /// <summary>
        /// 关闭命令
        /// </summary>
        public void closeOrder()
        {
            if (this.InvokeRequired)
            {
                //这里利用委托进行窗体的操作，避免跨线程调用时抛异常，后面给出具体定义
                SetUISomeInfo UIinfo = new SetUISomeInfo(new Action(() =>
                {
                    while (!this.IsHandleCreated)
                    {
                        ;
                    }
                    if (this.IsDisposed)
                        return;
                    if (!this.IsDisposed)
                    {
                        this.Dispose();
                    }

                }));
                this.Invoke(UIinfo);
            }
            else
            {
                if (this.IsDisposed)
                    return;
                if (!this.IsDisposed)
                {
                    this.Dispose();
                }
            }
        }
    }
}
