using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace UserControls
{
    /// <summary>
    /// 信号灯
    /// <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public class my_Signal : UserControl
    {
        /// <summary>
        /// 显示边框
        /// </summary>
        private bool isShowBorder = false;

        /// <summary>
        /// 获取或设置一个值，该值指示此实例是否为显示边框。
        /// </summary>
        [Description("是否显示边框"), Category("自定义")]
        public bool IsShowBorder
        {
            get { return isShowBorder; }
            set
            {
                isShowBorder = value;
                Refresh();
            }
        }

        /// <summary>
        /// 灯的颜色
        /// </summary>
        private Color[] lampColor = new Color[] { Color.FromArgb(255, 77, 59) };

        /// <summary>
        /// 获取或设置灯的颜色。
        /// </summary>
        /// <value>灯的颜色。</value>
        [Description("灯颜色，当需要闪烁时，至少需要2个及以上颜色，不需要闪烁则至少需要1个颜色"), Category("自定义")]
        public Color[] LampColor
        {
            get { return lampColor; }
            set
            {
                if (value == null || value.Length <= 0)
                    return;
                lampColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// 强度
        /// </summary>
        private bool isHighlight = true;

        /// <summary>
        /// 获取或设置一个值，该值指示此实例是否突出显示
        /// </summary>
        [Description("是否高亮显示"), Category("自定义")]
        public bool IsHighlight
        {
            get { return isHighlight; }
            set
            {
                isHighlight = value;
                Refresh();
            }
        }

        /// <summary>
        /// 闪烁的速度
        /// </summary>
        private int twinkleSpeed = 0;

        /// <summary>
        /// 获取或设置闪烁速度。
        /// </summary>
        [Description("闪烁间隔时间（毫秒），当为0时不闪烁"), Category("自定义")]
        public int TwinkleSpeed
        {
            get { return twinkleSpeed; }
            set
            {
                if (value < 0)
                    return;
                twinkleSpeed = value;
                if (value == 0 || lampColor.Length <= 1)
                {
                    timer.Enabled = false;
                }
                else
                {
                    intColorIndex = 0;
                    timer.Interval = value;
                    timer.Enabled = true;
                }
                Refresh();
            }
        }
        /// <summary>
        /// 计时器
        /// </summary>
        Timer timer;
        /// <summary>
        /// 颜色索引
        /// </summary>
        int intColorIndex = 0;


        public my_Signal()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Size = new Size(50, 50);
            this.SizeChanged += UCSignalLamp_SizeChanged;
            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += timer_Tick;
        }

        /// <summary>
        /// 处理定时器控件的Tick事件。
        /// </summary>
        void timer_Tick(object sender, EventArgs e)
        {
            intColorIndex++;
            if (intColorIndex >= lampColor.Length)
                intColorIndex = 0;
            Refresh();
        }
        /// <summary>
        ///控件的SizeChanged事件。
        /// </summary>
        void UCSignalLamp_SizeChanged(object sender, EventArgs e)
        {
            var maxSize = Math.Min(this.Width, this.Height);
            if (this.Width != maxSize)
                this.Width = maxSize;
            if (this.Height != maxSize)
                this.Height = maxSize;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            Color c1 = lampColor[intColorIndex];
            g.FillEllipse(new SolidBrush(c1), new Rectangle(this.ClientRectangle.Location, this.ClientRectangle.Size - new Size(1, 1)));

            if (isHighlight)
            {
                GraphicsPath gp = new GraphicsPath();

                Rectangle rec = new Rectangle(5, 5, this.Width - 10 - 1, this.Height - 10 - 1);
                gp.AddEllipse(rec);

                Color[] surroundColor = new Color[] { c1 };
                PathGradientBrush pb = new PathGradientBrush(gp);
                pb.CenterColor = Color.White;
                pb.SurroundColors = surroundColor;
                g.FillPath(pb, gp);
            }

            if (isShowBorder)
            {
                g.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 2), new Rectangle(4, 4, this.Width - 1 - 8, this.Height - 1 - 8));
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // my_Signal
            // 
            this.Name = "my_Signal";
            this.Size = new System.Drawing.Size(50, 50);
            this.ResumeLayout(false);

        }
    }
}
