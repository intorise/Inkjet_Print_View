using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.ModBus;
using PR_Model;
using PR_SPC;
using static HslCommunication.Profinet.Knx.KnxCode;

namespace PR_Spc_Tester.Moudules
{
    public class ModBusTcpHelper : IDisposable
    {
        string plcIp;
        int port;
        ModbusTcpNet md_net = null;
        bool _isConnected;
        public ModBusTcpHelper(string ip, int port)
        {
            this.plcIp= ip;
            this.port= port;
        }
        public bool Connect()
        {
            if (md_net != null)
            {
                md_net.ConnectClose();
            }
            Thread.Sleep(50);
            
            md_net = new ModbusTcpNet(plcIp, port);
            OperateResult result = md_net.ConnectServer();
            md_net.DataFormat = HslCommunication.Core.DataFormat.CDAB;
            _isConnected = result.IsSuccess;
            return _isConnected;
        }
        public OperateResult<TestData> GetTestData()
        {
            try
            {
                if (md_net == null)
                {
                    Connect();
                }
                byte[] dataToSend = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
                md_net.Write("", dataToSend);
                float temperature = md_net.ReadFloat("2").Content;
                float nitrogenPressure = md_net.ReadFloat("3").Content;
                TestData testData = new TestData()
                {
                   //Temperature= temperature,
                   //NitrogenPressure=nitrogenPressure
                };
                return new OperateResult<TestData>()
                {
                    Content = testData,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("读取冷喷枪数据C异常：" + ex.Message + "\r\n" + ex.StackTrace);
                return new OperateResult<TestData>()
                {
                    Content = null,
                    ErrorCode = -1,
                    Message = ex.Message
                };
            }
        }
        public void Disconnect()
        {
            md_net?.ConnectClose(); // 关闭连接
        }
        public void Dispose()
        {

        }
        

    }
    
}
