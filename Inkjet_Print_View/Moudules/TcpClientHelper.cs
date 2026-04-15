using PR_SPC;
using PR_Spc_Tester.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication;
using PR_Model;


namespace PR_Spc_Tester.Moudules
{
    /// <summary>
    /// TCP 帮助类; 如没有心跳的情况下，用完清及时关闭;
    /// </summary>
    public class TcpClientHelper
    {
        private TcpClient mClient;
        private IPEndPoint endPoint;
        private NetworkStream NetworkStream;

        public TcpClientHelper()
        {
            string plcIp = ConfigAppSettings.GetValue("GunIP").ToString();
            int port = Convert.ToInt32(ConfigAppSettings.GetValue("GunPort"));
            mClient = new TcpClient();
            endPoint = new IPEndPoint(IPAddress.Parse(plcIp), port);
        }

        /// <summary>
        /// 进行连接
        /// </summary>
        public bool ConnectServer()
        {
            try
            {
                mClient.Connect(endPoint);
                NetworkStream = mClient.GetStream();
                NetworkStream.WriteTimeout = 1000;
                NetworkStream.ReadTimeout = 3000;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"连接发生错误，消息：[{ex.Message}][{ex.StackTrace}]");
                return false;
            }
        }

        /// <summary>
        /// 断开TCP连接;
        /// </summary>
        /// <returns></returns>
        public bool DisConnect()
        {
            try
            {
                if (mClient != null)
                {
                    mClient.Close();
                    mClient = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"关闭TCP连接异常，消息：[{ex.Message}][{ex.StackTrace}]");
            }
            return false;
        }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return mClient != null && mClient.Connected;
            }
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="Send"></param>
        public TestData SendString(TestData testData, int switchPowder)
        {
            if (!IsConnected)
            {
                ConnectServer();
            }
            try
            {
                byte[] Send = { 0x00, 0x2B, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x00, 0x00, 0x00, 0x32 };
                NetworkStream.Write(Send, 0, Send.Length);
                string hexStringWithSpaces = BitConverter.ToString(Send).Replace("-", " ");
               // LogService.AddLogToEnqueue($"发送命令{hexStringWithSpaces}");
                Thread.Sleep(300);
                byte[] bRec = new byte[1024];
                int icount = NetworkStream.Read(bRec, 0, bRec.Length);
                hexStringWithSpaces = BitConverter.ToString(bRec).Replace("-", " ");
               // LogService.AddLogToEnqueue($"获取原始数据为{hexStringWithSpaces}");
                byte[] floatBytes = new byte[28];
                Array.Copy(bRec, 9, floatBytes, 0, 28);
                hexStringWithSpaces = BitConverter.ToString(floatBytes).Replace("-", " ");
               // LogService.AddLogToEnqueue($"提取原始数据为{hexStringWithSpaces}");
                // 处理字节序（假设数据是大端序，而系统是小端序）
                if (BitConverter.IsLittleEndian)
                {
                    for (int i = 0; i < floatBytes.Length; i += 4)
                    {
                        Array.Reverse(floatBytes, i, 4);
                    }
                }
                // 转换为 float 数组
                float[] floatArray = new float[7];
                for (int i = 0; i < 7; i++)
                {
                    floatArray[i] = BitConverter.ToSingle(floatBytes, i * 4);
                }
                //testData.Temperature = floatArray[2];
                //testData.NitrogenPressure = floatArray[3];
                
                //LogService.AddLogToEnqueue($"读取切换粉罐的值为{resultRest.Content}");
                if (switchPowder == 2)
                {
                    testData.ThreadRotation = floatArray[6];
                }
                else if (switchPowder == 1)
                {
                    testData.ThreadRotation = floatArray[5];
                }
                return testData;
               

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"读取冷喷漆数据异常，消息：[{ex.Message}][{ex.StackTrace}]");
                return testData;
            }
        }
        public void Dispose()
        {
            DisConnect();
        }

        /// <summary>
        /// 读取冷喷枪温度压力
        /// </summary>
        /// <param name="Send"></param>
        public float[] ReadGun()
        {
            if (!IsConnected)
            {
                ConnectServer();
            }
            try
            {
                // 00 2B 00 00 00 06 01 03 00 00 00 32
                byte[] Send = { 0x00, 0x2B, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x00, 0x00, 0x00, 0x32 };
                NetworkStream.Write(Send, 0, Send.Length);
                string hexStringWithSpaces = BitConverter.ToString(Send).Replace("-", " ");
               // LogService.AddLogToEnqueue($"发送命令{hexStringWithSpaces}");
                Thread.Sleep(300);
                byte[] bRec = new byte[1024];
                int icount = NetworkStream.Read(bRec, 0, bRec.Length);
                hexStringWithSpaces = BitConverter.ToString(bRec).Replace("-", " ");
               // LogService.AddLogToEnqueue($"获取原始数据为{hexStringWithSpaces}");
                byte[] floatBytes = new byte[28];
                Array.Copy(bRec, 9, floatBytes, 0, 28);
                hexStringWithSpaces = BitConverter.ToString(floatBytes).Replace("-", " ");
                //LogService.AddLogToEnqueue($"提取原始数据为{hexStringWithSpaces}");
                // 处理字节序（假设数据是大端序，而系统是小端序）
                if (BitConverter.IsLittleEndian)
                {
                    for (int i = 0; i < floatBytes.Length; i += 4)
                    {
                        Array.Reverse(floatBytes, i, 4);
                    }
                }
                // 转换为 float 数组
                float[] floatArray = new float[7];
                for (int i = 0; i < 7; i++)
                {
                    floatArray[i] = BitConverter.ToSingle(floatBytes, i * 4);
                }
                return floatArray;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"读取冷喷温度压力异常，消息：[{ex.Message}][{ex.StackTrace}]");
                return new float [7] { 0,0,0,0,0,0,0};
            }
        }
    }
}
