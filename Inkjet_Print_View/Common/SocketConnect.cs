using PR_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PR_Spc_Tester.Common
{
    public class SocketConnect
    {
        public enum ConnectType 
        {
            刻印机 =1
        }
        /// <summary>
        /// 刻印机
        /// </summary>
        public  Socket SocketMarking;

        #region TCP连接
        public bool Connect(ConnectType socketType,ConnectConfig config)
        {
            try
            {
                bool ConnectIsOK = false;

                Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReceiveTimeout,1500);
                IPAddress address =null;
                IPEndPoint port = null;

                switch (socketType.ToString())
                {
                    case "刻印机":
                        address = IPAddress.Parse(config.Marking_IP);
                        port = new IPEndPoint(address,int.Parse(config.Markong_Port));
                        break;
                }
                int cyclicNum = 0;
                while (cyclicNum<3)
                {
                    if (socket.Connected)
                    {
                        switch (socket.ToString())
                        {
                            case "刻印机":
                                SocketMarking = socket;
                                break;
                            default:
                                break;
                        }
                        ConnectIsOK = true;
                        break;
                    }
                    socket.Connect(port);
                    Thread.Sleep(500);
                }
                return ConnectIsOK;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        #endregion

        #region TCP发送
        public  bool socketSend(string socketType, byte[] bytes) 
        {
            try
            {
                bool sendIsOk = false;
                switch (socketType.ToString())
                {
                    case "刻印机":
                        if (SocketMarking != null)
                        {
                            SocketMarking.Send(bytes);
                            sendIsOk = true;
                        }
                        break;
                }
                
                return sendIsOk;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 刻印机接收
        int length = 0;
        byte[] bytes = null;
        string str = null;
        public  void MarkingReceiveCallBack(IAsyncResult async) 
        {
            try
            {
                length = SocketMarking.EndReceive(async);  
                if (length > 0)
                {
                    str = Encoding.UTF8.GetString(bytes, 0,length);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string MarkingReceive() 
        {
            try
            {
                int cyclicNum = 0;
                bytes = new byte[1024*1024*2];
                length = 0;
                str = "";
                while (cyclicNum<5)
                {
                    SocketMarking.BeginReceive(bytes,0,bytes.Length,SocketFlags.None,new AsyncCallback(MarkingReceiveCallBack),null);
                    Thread.Sleep(1000);
                    cyclicNum++;
                    if (str!="" && length>0)
                    {
                        break;
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

    }
}
