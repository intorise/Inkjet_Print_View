using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR_Spc_Tester.PRControl
{
    public partial class SignalControl : UIUserControl
    {
        private string _stepName { get; set; }

        /// <summary>
        /// 测试工步
        /// </summary>
        public string StepName
        {
            get
            {
                return _stepName;
            }
            set
            {
                _stepName = value;
                lb_step_name.Text = _stepName;
            }
        }

        private string _batteryCode { get; set; }

        /// <summary>
        /// 电池编码
        /// </summary>
        public string BatteryCode
        {
            get { return _batteryCode; }
            set
            {
                _batteryCode = value;
                lb_code.Text = _batteryCode;
            }
        }

        private bool _onPos { get; set; }

        /// <summary>
        /// 电池编码
        /// </summary>
        public bool OnPos
        {
            get { return _onPos; }
            set
            {
                _onPos = value;
                if (_onPos)
                {
                    lb_status.BackColor = Color.Green;
                } else
                {
                    lb_status.BackColor = Color.Gray;
                }
            }
        }

        private string _result { get; set; }

        /// <summary>
        /// 电池编码
        /// </summary>
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
               lb_result.Text = _result;
            }
        }


        public SignalControl()
        {
            InitializeComponent();
        }

        //都有对象属性和方法  都可以被实例化

        //结构是值类型，存储在栈上面，类是引用类型，存储在堆上面
        //结构实例化的时候不可以用new关键字
        //结构可以声明构造函数，但是必须带参数



        //接口和类都可以被多接口继承
        //接口类似抽象基类，继承接口的任何非抽象类都必须实现接口的全部成员
        //都可以包含方法，属性，索引块和事件

        //接口可以多继承，接口不能实例化，接口不包含方法的实现
        //类只能单继承，类定义在不同的源文件之间进行拆分
    }
}
