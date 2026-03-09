namespace PRBakerProject.Moudules.Waiting
{
    partial class WaitingForm
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
            this.lb_info = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_info
            // 
            this.lb_info.BackColor = System.Drawing.SystemColors.Info;
            this.lb_info.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_info.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_info.ForeColor = System.Drawing.Color.Black;
            this.lb_info.Location = new System.Drawing.Point(0, 0);
            this.lb_info.Name = "lb_info";
            this.lb_info.Size = new System.Drawing.Size(200, 54);
            this.lb_info.TabIndex = 0;
            this.lb_info.Text = "加载中，请等待...";
            this.lb_info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WaitingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(200, 54);
            this.Controls.Add(this.lb_info);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WaitingForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WaitingForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.WaitingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lb_info;
    }
}