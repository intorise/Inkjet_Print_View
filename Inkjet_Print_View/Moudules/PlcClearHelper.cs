using HslCommunication.Profinet.Melsec;
using HslCommunication;
using PR_SPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PR_Model;
using System.IO;
using MiniExcelLibs;
using PR_Spc_Tester.Services;
using PR_Spc_Tester.PRControl;
using System.Reflection;
using static HslCommunication.Profinet.Knx.KnxCode;
using ScottPlot.Palettes;
using System.Windows.Documents;
using System.Windows.Media.Media3D;

namespace PR_Spc_Tester.Moudules
{
    /// <summary>
    /// 三菱PLC操作
    /// </summary>
    public class PlcClearHelper: IDisposable
    {
        
        public PlcClearHelper() { }
        MelsecMcNet mc_net = null;
        bool _isConnected;

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns>是否连接成功</returns>
        public bool ConnectPLC()
        {
            string plcIp = ConfigAppSettings.GetValue("PLC_Clear_IP").ToString();
            int port = Convert.ToInt32(ConfigAppSettings.GetValue("PLC_Clear_Port"));
            if (mc_net != null)
            {
                mc_net.ConnectClose();
            }
            Thread.Sleep(50);
            mc_net = new MelsecMcNet(plcIp, port);
            mc_net.ReceiveTimeOut = 2000;
            mc_net.ConnectTimeOut = 3000;

            OperateResult result = mc_net.ConnectServer();
            if (!result.IsSuccess)
            {
                LogService.AddLogToEnqueue($"连接PLC失败，原因{result.Message}");
            }
            _isConnected = result.IsSuccess;
            return _isConnected;
        }

        public bool IsConnected()
        {
            return _isConnected;
        }

        /// <summary>
        /// 获取读取准备好信号
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> GetReadReady()
        {
            try
            {
                if (mc_net == null || !_isConnected)
                {
                    if (!ConnectPLC())
                    {
                        return new OperateResult<short>(-1, "PLC未连接");
                    }
                }
                return mc_net.ReadInt16("D5000");
            }catch(Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败"+ex.Message);
            }
        }

        /// <summary>
        /// 读取激光二维码
        /// </summary>
        /// <returns></returns>
        public OperateResult<string> GetBarcode()
        {
            try
            {
                string code = mc_net.ReadString("D6000", 50).Content?.RemoveControlChars();
                return new OperateResult<string>()
                {
                    Content = code,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                return new OperateResult<string>(-1, "读取激光二维码失败" + ex.Message);
            }
        }


        /// <summary>
        /// 读取激光二维码失败
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteAddCodeFailed()
        {
            try
            {
                return mc_net.Write("D5001", 2);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取激光二维码失败" + ex.Message);
            }
        }

        /// <summary>
        /// 写入新增激光二维码OK
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteAddCodeOK()
        {
            try
            {
                return mc_net.Write("D5001", 1);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取激光二维码OK" + ex.Message);
            }
        }


        #region 心跳

        /// <summary>
        /// 程序运行时写1，关闭时写0
        /// </summary>
        /// <returns></returns>
        public OperateResult WritePCHeart(short val)
        {
            try
            {
                return mc_net.Write("D5750", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "写入心跳失败" + ex.Message);
            }
        }

        /// <summary>
        /// 获取PC心跳
        /// </summary>
        /// <returns></returns>
        public OperateResult GetPCHeart()
        {
            try
            {
                return mc_net.ReadBool("D5501.0");
            }
            catch (Exception ex)
            {
                return new OperateResult<bool>(-1, "获取心跳失败" + ex.Message);
            }
        }

        /// <summary>
        /// 上位机读取Plc心跳
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> GetPlcHeart()
        {
            try
            {
                return mc_net.ReadInt16("D5673");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 复位PLC心跳;
        /// </summary>
        /// <returns></returns>
        public OperateResult WritePlcHeart()
        {
            try
            {
                return mc_net.Write("D7139", (short)0);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }

        #endregion

        public void Dispose()
        {
            if (mc_net != null)
            {
                mc_net.ConnectClose();
                mc_net = null;
            }
        }
    }
}
