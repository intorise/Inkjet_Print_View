namespace PR_Spc_Tester.UserForms
{
    partial class FrmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.txt_userName = new Sunny.UI.UITextBox();
            this.txt_Password = new Sunny.UI.UITextBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.btn_login = new Sunny.UI.UIButton();
            this.btn_cancel = new Sunny.UI.UIButton();
            this.lb_version = new Sunny.UI.UILabel();
            this.uiLine1 = new Sunny.UI.UILine();
            this.SuspendLayout();
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("黑体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(144, 61);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(179, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "数据采集系统";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(100, 122);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(79, 23);
            this.uiLabel2.TabIndex = 1;
            this.uiLabel2.Text = "用户名：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_userName
            // 
            this.txt_userName.ButtonSymbolOffset = new System.Drawing.Point(0, 0);
            this.txt_userName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_userName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_userName.Location = new System.Drawing.Point(186, 119);
            this.txt_userName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_userName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txt_userName.Name = "txt_userName";
            this.txt_userName.Padding = new System.Windows.Forms.Padding(5);
            this.txt_userName.ShowText = false;
            this.txt_userName.Size = new System.Drawing.Size(180, 29);
            this.txt_userName.TabIndex = 2;
            this.txt_userName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt_userName.Watermark = "";
            // 
            // txt_Password
            // 
            this.txt_Password.ButtonSymbolOffset = new System.Drawing.Point(0, 0);
            this.txt_Password.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_Password.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Password.Location = new System.Drawing.Point(186, 170);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_Password.MinimumSize = new System.Drawing.Size(1, 16);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Padding = new System.Windows.Forms.Padding(5);
            this.txt_Password.PasswordChar = '*';
            this.txt_Password.ShowText = false;
            this.txt_Password.Size = new System.Drawing.Size(180, 29);
            this.txt_Password.TabIndex = 4;
            this.txt_Password.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt_Password.Watermark = "";
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(100, 173);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(79, 23);
            this.uiLabel3.TabIndex = 3;
            this.uiLabel3.Text = "密  码：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_login
            // 
            this.btn_login.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_login.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_login.Location = new System.Drawing.Point(186, 217);
            this.btn_login.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(73, 35);
            this.btn_login.TabIndex = 5;
            this.btn_login.Text = "登录";
            this.btn_login.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_cancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancel.LightStyle = true;
            this.btn_cancel.Location = new System.Drawing.Point(295, 217);
            this.btn_cancel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(71, 35);
            this.btn_cancel.TabIndex = 6;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lb_version
            // 
            this.lb_version.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_version.Location = new System.Drawing.Point(145, 274);
            this.lb_version.Name = "lb_version";
            this.lb_version.Size = new System.Drawing.Size(180, 23);
            this.lb_version.TabIndex = 7;
            this.lb_version.Text = "当前版本：";
            this.lb_version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLine1
            // 
            this.uiLine1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLine1.Location = new System.Drawing.Point(3, 87);
            this.uiLine1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiLine1.Name = "uiLine1";
            this.uiLine1.Size = new System.Drawing.Size(463, 29);
            this.uiLine1.TabIndex = 8;
            // 
            // FrmLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(469, 309);
            this.Controls.Add(this.uiLine1);
            this.Controls.Add(this.lb_version);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.uiLabel3);
            this.Controls.Add(this.txt_userName);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.Text = "系统登录";
            this.TextAlignment = System.Drawing.StringAlignment.Center;
            this.TitleFont = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 566);
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox txt_userName;
        private Sunny.UI.UITextBox txt_Password;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UIButton btn_login;
        private Sunny.UI.UIButton btn_cancel;
        private Sunny.UI.UILabel lb_version;
        private Sunny.UI.UILine uiLine1;
    }
}