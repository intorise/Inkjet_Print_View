namespace PR_Spc_Tester.UserForms
{
    partial class FrmUserAdd
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
            this.btnAdd = new Sunny.UI.UIButton();
            this.uiCmbUserType = new Sunny.UI.UIComboBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiTxtPassword = new Sunny.UI.UITextBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiTxtUserName = new Sunny.UI.UITextBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(112, 206);
            this.btnAdd.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(74, 29);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "确定";
            this.btnAdd.TipsFont = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // uiCmbUserType
            // 
            this.uiCmbUserType.DataSource = null;
            this.uiCmbUserType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.uiCmbUserType.FillColor = System.Drawing.Color.White;
            this.uiCmbUserType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiCmbUserType.Items.AddRange(new object[] {
            "管理员",
            "工艺",
            "操作员"});
            this.uiCmbUserType.Location = new System.Drawing.Point(112, 148);
            this.uiCmbUserType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiCmbUserType.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiCmbUserType.Name = "uiCmbUserType";
            this.uiCmbUserType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiCmbUserType.Size = new System.Drawing.Size(150, 29);
            this.uiCmbUserType.TabIndex = 12;
            this.uiCmbUserType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiCmbUserType.Watermark = "";
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(16, 154);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(96, 23);
            this.uiLabel3.TabIndex = 11;
            this.uiLabel3.Text = "用户类型：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiTxtPassword
            // 
            this.uiTxtPassword.ButtonSymbolOffset = new System.Drawing.Point(0, 0);
            this.uiTxtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTxtPassword.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTxtPassword.Location = new System.Drawing.Point(112, 104);
            this.uiTxtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTxtPassword.MinimumSize = new System.Drawing.Size(1, 16);
            this.uiTxtPassword.Name = "uiTxtPassword";
            this.uiTxtPassword.Padding = new System.Windows.Forms.Padding(5);
            this.uiTxtPassword.PasswordChar = '*';
            this.uiTxtPassword.ShowText = false;
            this.uiTxtPassword.Size = new System.Drawing.Size(150, 29);
            this.uiTxtPassword.TabIndex = 10;
            this.uiTxtPassword.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTxtPassword.Watermark = "";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(30, 104);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(82, 23);
            this.uiLabel2.TabIndex = 9;
            this.uiLabel2.Text = "密码：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiTxtUserName
            // 
            this.uiTxtUserName.ButtonSymbolOffset = new System.Drawing.Point(0, 0);
            this.uiTxtUserName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTxtUserName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTxtUserName.Location = new System.Drawing.Point(112, 59);
            this.uiTxtUserName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTxtUserName.MinimumSize = new System.Drawing.Size(1, 16);
            this.uiTxtUserName.Name = "uiTxtUserName";
            this.uiTxtUserName.Padding = new System.Windows.Forms.Padding(5);
            this.uiTxtUserName.ShowText = false;
            this.uiTxtUserName.Size = new System.Drawing.Size(150, 29);
            this.uiTxtUserName.TabIndex = 8;
            this.uiTxtUserName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTxtUserName.Watermark = "";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(30, 59);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(82, 23);
            this.uiLabel1.TabIndex = 7;
            this.uiLabel1.Text = "用户名：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmUserAdd
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(305, 270);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.uiCmbUserType);
            this.Controls.Add(this.uiLabel3);
            this.Controls.Add(this.uiTxtPassword);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiTxtUserName);
            this.Controls.Add(this.uiLabel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUserAdd";
            this.Text = "用户管理";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.Load += new System.EventHandler(this.FrmUserAdd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIButton btnAdd;
        private Sunny.UI.UIComboBox uiCmbUserType;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UITextBox uiTxtPassword;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox uiTxtUserName;
        private Sunny.UI.UILabel uiLabel1;
    }
}