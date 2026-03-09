namespace PR_Spc_Tester.UserForms
{
    partial class FrmParams
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.btnRefresh = new Sunny.UI.UISymbolButton();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.dgv_TestProject = new Sunny.UI.UIDataGridView();
            this.Col_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_USL_Val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_LSL_Val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_UpdateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TestProject)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.btnRefresh);
            this.uiPanel1.Controls.Add(this.uiLabel1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 35);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(718, 41);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefresh.Location = new System.Drawing.Point(10, 4);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(73, 31);
            this.btnRefresh.Symbol = 61473;
            this.btnRefresh.SymbolSize = 18;
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(95, 10);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(171, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "点击\'编辑\'按钮进行编辑";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgv_TestProject
            // 
            this.dgv_TestProject.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgv_TestProject.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_TestProject.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgv_TestProject.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_TestProject.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_TestProject.ColumnHeadersHeight = 32;
            this.dgv_TestProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_TestProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_ID,
            this.Col_ProjectName,
            this.Col_USL_Val,
            this.Col_LSL_Val,
            this.Col_UpdateTime,
            this.Col_Edit});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_TestProject.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_TestProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_TestProject.EnableHeadersVisualStyles = false;
            this.dgv_TestProject.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv_TestProject.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.dgv_TestProject.Location = new System.Drawing.Point(0, 76);
            this.dgv_TestProject.Name = "dgv_TestProject";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_TestProject.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_TestProject.RowHeadersWidth = 12;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgv_TestProject.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_TestProject.RowTemplate.Height = 23;
            this.dgv_TestProject.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgv_TestProject.SelectedIndex = -1;
            this.dgv_TestProject.Size = new System.Drawing.Size(718, 582);
            this.dgv_TestProject.TabIndex = 2;
            this.dgv_TestProject.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_TestProject_CellEndEdit);
            // 
            // Col_ID
            // 
            this.Col_ID.HeaderText = "序号";
            this.Col_ID.Name = "Col_ID";
            this.Col_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Col_ID.Width = 70;
            // 
            // Col_ProjectName
            // 
            this.Col_ProjectName.HeaderText = "参数名称";
            this.Col_ProjectName.Name = "Col_ProjectName";
            this.Col_ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Col_ProjectName.Width = 220;
            // 
            // Col_USL_Val
            // 
            this.Col_USL_Val.HeaderText = "标准上限(USL)";
            this.Col_USL_Val.Name = "Col_USL_Val";
            this.Col_USL_Val.Width = 130;
            // 
            // Col_LSL_Val
            // 
            this.Col_LSL_Val.HeaderText = "标准下限(LSL)";
            this.Col_LSL_Val.Name = "Col_LSL_Val";
            this.Col_LSL_Val.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Col_LSL_Val.Width = 110;
            // 
            // Col_UpdateTime
            // 
            this.Col_UpdateTime.HeaderText = "修改时间";
            this.Col_UpdateTime.Name = "Col_UpdateTime";
            this.Col_UpdateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Col_UpdateTime.Width = 190;
            // 
            // Col_Edit
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "编辑";
            this.Col_Edit.DefaultCellStyle = dataGridViewCellStyle3;
            this.Col_Edit.HeaderText = "编辑";
            this.Col_Edit.Name = "Col_Edit";
            this.Col_Edit.Text = "编辑";
            // 
            // FrmParams
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(718, 658);
            this.Controls.Add(this.dgv_TestProject);
            this.Controls.Add(this.uiPanel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmParams";
            this.Text = "系统参数设置";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.Load += new System.EventHandler(this.FrmParams_Load);
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TestProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIDataGridView dgv_TestProject;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UISymbolButton btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_USL_Val;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_LSL_Val;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_UpdateTime;
        private System.Windows.Forms.DataGridViewButtonColumn Col_Edit;
    }
}