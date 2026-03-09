using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua.Client;
using Opc.Ua;
using Prism.Mvvm;
using OpcUaHelper;
using PR_SPC;

namespace PR_Spc_Tester.Moudules
{
    public class OPCuaHelper : IDisposable
    {
        private OpcUaClient opcUaClient = new OpcUaClient();
         public OPCuaHelper()
        {
            try
            {
                string url = ConfigAppSettings.GetValue("MonitorIP").ToString();
                opcUaClient.ConnectServer(url);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"连接OPC发生错误，消息：[{ex.Message}][{ex.StackTrace}]");
               // return false;
            }
        }
        public double readSpeed()
        {
            return opcUaClient.ReadNode<double>("ns=2;s=Sensor_00.CurrentMean.ParticleSpeed");
        }
        public double readDensity()
        {
            return opcUaClient.ReadNode<double>("ns=2;s=Sensor_00.CurrentMean.ParticleDensity");
        }
        private void DisConnect()
        {
            opcUaClient.Disconnect();
        }
        public void Dispose()
        {
            DisConnect();
        }
    }
}
