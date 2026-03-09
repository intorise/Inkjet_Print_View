using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    public enum EnumMsgType
    {
        [Description("消息")]
        Info = 0,

        [Description("警告")]
        Warning = 1,

        [Description("错误")]
        Error = 2,

        [Description("异常")]
        Exception = 3,

        [Description("空白")]
        Not = 4,

        [Description("联机")]
        Success = 5,
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum LogMsgType
    {
        [Description("未知")]
        Unknown = 0,

        [Description("消息")]
        Info = 1,

        [Description("警告")]
        Warning = 2,

        [Description("错误")]
        Error = 3,

        [Description("异常")]
        Exception = 4,
    }

    /// <summary>
    /// 测试项目
    /// </summary>
    public enum DataType
    {
        [Description("弹簧高度")]
        SpringHeight = 1,
        [Description("弹簧角度")]
        SpringAngle = 2,
        [Description("弹簧拉力")]
        SpringTension = 3,
        [Description("纸片高度")]
        PaperHeight = 4
    }

    public class PREnum
    {
    }
}
