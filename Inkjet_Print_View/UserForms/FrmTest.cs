using PR_Model;
using PR_Spc_Tester.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PR_Spc_Tester.Common.SocketConnect;

namespace PR_Spc_Tester.UserForms
{
    public partial class FrmTest : Form
    {
        Socket SocketMarking;
        bool  Isok = false;
        public FrmTest()
        {
            InitializeComponent();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            SocketMarking = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPAddress ip = IPAddress.Parse(uiTextBox1.Text);
            //IPEndPoint port = new IPEndPoint(ip, int.Parse(uiTextBox2.Text));
            //socketSend.Connect(port);

            ConnectConfig config = new ConnectConfig();
            config.Marking_IP = "127.0.0.1";
            config.Markong_Port = "888";

            Isok = Connect(config);
            uiTextBox3.AppendText("连接服务端成功！\r\n");
            Thread thread = new Thread(Receive);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Receive()
        {
            while (true)
            {
                try
                {
                    //byte[] buffer = new byte[1024 * 1024 * 2];
                    //int r = SocketMarking.Receive(buffer);
                    //if (r == 0)
                    //{
                    //    break;
                    //}
                    string str = MarkingReceive();
                    if (str!="")
                    {
                        //string str = Encoding.UTF8.GetString(buffer, 0, r);
                        uiTextBox3.AppendText(SocketMarking.RemoteEndPoint + ": " + str + "\r\n");
                    }
                }
                catch (Exception)
                {
                }
                Thread.Sleep(500);
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            string str = uiTextBox4.Text;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);

            //socketSend.Send(buffer);
            socketSendS(buffer);
        }

        private void FrmTest_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public bool Connect(ConnectConfig config) 
        {
            Isok = false;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1500);
            IPAddress address = null;
            IPEndPoint port = null;
            address = IPAddress.Parse(config.Marking_IP);
            port = new IPEndPoint(address, int.Parse(config.Markong_Port));
            socket.Connect(port);
            SocketMarking = socket;
            Isok = true;
            return Isok;
        }

        public void socketSendS(byte[] bytes) 
        {
            SocketMarking.Send(bytes);
        }

        #region 刻印机接收
        int length = 0;
        byte[] bytes = null;
        string str = null;
        public void MarkingReceiveCallBack(IAsyncResult async)
        {
            try
            {
                //var socket = async.AsyncState as Socket;
                length = SocketMarking.EndReceive(async);
                if (length > 0)
                {
                    str = Encoding.ASCII.GetString(bytes, 0, length);
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
                bytes = new byte[1024 * 1024 * 2];
                length = 0;
                str = "";
                while (cyclicNum < 5)
                {
                    SocketMarking.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(MarkingReceiveCallBack), null);
                    Thread.Sleep(1000);
                    cyclicNum++;
                    if (str == "" && length == 0)
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
