namespace PR_Spc_Tester.PRControl
{
    partial class SignalControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.lb_status = new Sunny.UI.UILabel();
            this.lb_step_name = new Sunny.UI.UILabel();
            this.lb_result = new Sunny.UI.UILabel();
            this.lb_code = new Sunny.UI.UILabel();
            this.SuspendLayout();
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(24, 90);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiLabel2.TabIndex = 1;
            this.uiLabel2.Text = "测试结果:";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(24, 55);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "测试项目：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(24, 126);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(100, 23);
            this.uiLabel3.TabIndex = 2;
            this.uiLabel3.Text = "工件条码:";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel4
            // 
            this.uiLabel4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel4.Location = new System.Drawing.Point(24, 22);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(100, 23);
            this.uiLabel4.TabIndex = 3;
            this.uiLabel4.Text = "状态：";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_status
            // 
            this.lb_status.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_status.Location = new System.Drawing.Point(143, 22);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(100, 23);
            this.lb_status.TabIndex = 4;
            this.lb_status.Text = "待机";
            this.lb_status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_step_name
            // 
            this.lb_step_name.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_step_name.Location = new System.Drawing.Point(143, 55);
            this.lb_step_name.Name = "lb_step_name";
            this.lb_step_name.Size = new System.Drawing.Size(100, 23);
            this.lb_step_name.TabIndex = 5;
            this.lb_step_name.Text = "-";
            this.lb_step_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_result
            // 
            this.lb_result.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_result.Location = new System.Drawing.Point(143, 90);
            this.lb_result.Name = "lb_result";
            this.lb_result.Size = new System.Drawing.Size(100, 23);
            this.lb_result.TabIndex = 6;
            this.lb_result.Text = "OK";
            this.lb_result.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_code
            // 
            this.lb_code.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_code.Location = new System.Drawing.Point(143, 126);
            this.lb_code.Name = "lb_code";
            this.lb_code.Size = new System.Drawing.Size(100, 23);
            this.lb_code.TabIndex = 7;
            this.lb_code.Text = "-";
            this.lb_code.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SignalControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lb_code);
            this.Controls.Add(this.lb_result);
            this.Controls.Add(this.lb_step_name);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.uiLabel4);
            this.Controls.Add(this.uiLabel3);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.Name = "SignalControl";
            this.Size = new System.Drawing.Size(293, 167);
            this.ResumeLayout(false);

        }

        #endregion
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UILabel lb_status;
        private Sunny.UI.UILabel lb_step_name;
        private Sunny.UI.UILabel lb_result;
        private Sunny.UI.UILabel lb_code;
    }
}
