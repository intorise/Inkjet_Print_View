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
    public class MecPlcHelper
    {
        
        public MecPlcHelper() { }
        MelsecMcNet mc_net = null;
        bool _isConnected;

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns>是否连接成功</returns>
        public bool ConnectPLC()
        {
            string plcIp = ConfigAppSettings.GetValue("PLCIP").ToString();
            int port = Convert.ToInt32(ConfigAppSettings.GetValue("PLCPort"));
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
                return mc_net.ReadInt16("D5500");
            }catch(Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败"+ex.Message);
            }
        }

        /// <summary>
        /// 获取[称重2]读取准备好信号
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> GetReadReady2()
        {
            try
            {
                return mc_net.ReadInt16("D5699");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "[称重2]读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 获取读PLC报警
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> ReadPLCAlarm(string vaule)
        {
            try
            {
                return mc_net.ReadInt16(vaule);
               
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 读取冷喷二维码
        /// </summary>
        /// <returns></returns>
        public OperateResult<string> GetColdSprayCode()
        {
            try
            {
                string code = mc_net.ReadString("D5570", 49).Content?.RemoveControlChars();
                return new OperateResult<string>()
                {
                    Content = code,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                return new OperateResult<string>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 读取监控二维码
        /// </summary>
        /// <returns></returns>
        public OperateResult<string> GetMonitorCode()
        {
            try
            {
                string code = mc_net.ReadString("D5620", 49).Content?.RemoveControlChars();
                return new OperateResult<string>()
                {
                    Content = code,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                return new OperateResult<string>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 读取称重二维码
        /// </summary>
        /// <returns></returns>
        public OperateResult<string> GetHeaverCode()
        {
            try
            {
                string code = mc_net.ReadString("D5520", 49).Content?.RemoveControlChars();
                return new OperateResult<string>()
                {
                    Content = code,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                return new OperateResult<string>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 读取[称重2]维码
        /// </summary>
        /// <returns></returns>
        public OperateResult<string> GetHeaverCode2()
        {
            try
            {
                string code = mc_net.ReadString("D5935", 34).Content?.RemoveControlChars();//D5935-D5969	重量2二维码2

                return new OperateResult<string>()
                {
                    Content = code,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                return new OperateResult<string>(-1, "[称重2]读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 读取比对二维码
        /// </summary>
        /// <returns></returns>
        public OperateResult<string> GetCompareCode()
        {
            try
            {
                string code = mc_net.ReadString("D5850", 49).Content?.RemoveControlChars();
                return new OperateResult<string>()
                {
                    Content = code,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                return new OperateResult<string>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 读取比对2二维码
        /// </summary>
        /// <returns></returns>
        public OperateResult<string> GetCompareCode2()
        {
            try
            {
                string code = mc_net.ReadString("D5900", 34).Content?.RemoveControlChars();
                return new OperateResult<string>()
                {
                    Content = code,
                    ErrorCode = 0,
                    Message = $"获取数据成功！"
                };
            }
            catch (Exception ex)
            {
                return new OperateResult<string>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 获取读取冷喷准备好信号
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> GetColdSprayReadReady(string address="D5670")
        {
            try
            {
                return mc_net.ReadInt16(address);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 读取进气流量
        /// </summary>
        /// <returns></returns>
        public OperateResult<float> GetIntakeFlow()
        {
            try
            {
                return mc_net.ReadFloat("D5696");
            }
            catch (Exception ex)
            {
                return new OperateResult<float>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 获取读取监控准备好信号
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> GetMonitorReadReady()
        {
            try
            {
                return mc_net.ReadInt16("D5671");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 获取进气压力
        /// </summary>
        /// <returns></returns>
        public OperateResult<float> GetIntakePressure()
        {
            try
            {
                return mc_net.ReadFloat("D5518");
            }
            catch (Exception ex)
            {
                return new OperateResult<float>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <returns></returns>
        public OperateResult ResetReady()
        {
            try
            {
                return mc_net.Write("D5700", (short)2);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <returns></returns>
        public OperateResult ResetReady2()
        {
            try
            {
                return mc_net.Write("D5714", (short)2);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "[称重2]-读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 冷喷复位,1号：5710，2号：5713
        /// </summary>
        /// <returns></returns>
        public OperateResult ResetColdSprayReady(string address= "D5710")
        {
            try
            {
                return mc_net.Write(address, (short)2);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 监控复位
        /// </summary>
        /// <returns></returns>
        public OperateResult ResetMonitorReady()
        {
            try
            {
                return mc_net.Write("D5711", (short)2);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 异常复位
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> ResetAbnormal()
        {
            try
            {
                return mc_net.ReadInt16("D5674");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 供粉速度
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> ReadSpeed()
        {
            try
            {
                return mc_net.ReadInt16("D5677");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 喷嘴高度
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> ReadHeight()
        {
            try
            {
                return mc_net.ReadInt16("D5678");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 是否比对
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> ReadCompare(string address="D5676")
        {
            try
            {
                return mc_net.ReadInt16(address);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 比对2ready? 1:yes
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> ReadCompare2()
        {
            try
            {
                return mc_net.ReadInt16("D5698");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }
        /// <summary>
        /// 切换粉罐
        /// </summary>
        /// <returns></returns>
        public OperateResult<short> SwitchPowderCans()
        {
            //if (mc_net == null)
            //{
            //    ConnectPLC();
            //}
            try
            {
                return mc_net.ReadInt16("D5672");
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "读取失败" + ex.Message);
            }
        }

        /// <summary>
        /// 写入喷枪温度
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteTemperature(float val)
        {
            try
            {
                if (mc_net == null)
                {
                    ConnectPLC();
                }
                return mc_net.Write("D5732", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<float>(-1, "写入喷枪温度" + ex.Message);
            }
        }


        /// <summary>
        /// 写入喷枪压力
        /// </summary>
        /// <returns></returns>
        public OperateResult WritePressure(float val)
        {
            try
            {
                if (mc_net == null)
                {
                    ConnectPLC();
                }
                return mc_net.Write("D5730", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<float>(-1, "写入喷枪压力" + ex.Message);
            }
        }
        /// <summary>
        /// 写入冷喷参数异常
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteColdSpray(short val)
        {
            try
            {
                return mc_net.Write("D5721", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "写入冷喷参数失败" + ex.Message);
            }
        }

        /// <summary>
        /// 数据库中不存在二维码
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteNotCode(short val)
        {
            try
            {
                return mc_net.Write("D5724", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "写入数据库中不存在二维码失败" + ex.Message);
            }
        }

        /// <summary>
        /// 清洗摆放超时
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteOverTime(short val)
        {
            try
            {
               // return mc_net.Write("D5722", val);
                return mc_net.Write("D5712", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "写入清洗摆放超时失败" + ex.Message);
            }
        }

        /// <summary>
        /// 清洗摆放超时
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteOverTime2(short val)
        {
            try
            {
                // return mc_net.Write("D5722", val);
                return mc_net.Write("D5713", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "写入清洗摆放超时失败" + ex.Message);
            }
        }

        /// <summary>
        /// 监控异常
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteMonitoringAbnormality(short val)
        {
            try
            {
                return mc_net.Write("D5723", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "写入监控异常参数失败" + ex.Message);
            }
        }

        /// <summary>
        /// OPC UA报警信号写入PLC：1=有报警
        /// </summary>
        /// <returns></returns>
        public OperateResult WriteOpcUaAlarmFlag(short val)
        {
            try
            {
                return mc_net.Write("D5725", val);
            }
            catch (Exception ex)
            {
                return new OperateResult<short>(-1, "写入OPC UA报警信号失败" + ex.Message);
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

        /// <summary>
        /// 获取测试数据;
        /// </summary>
        /// <returns></returns>
        public TestData GetTestData(TestData testData)
        {
            try
            {
                if(mc_net == null)
                {
                    ConnectPLC();
                }

                float preSprayWeight = mc_net.ReadFloat("D5502").Content;
                float postSprayWeight = mc_net.ReadFloat("D5504").Content;
                float sedimentationWeight = mc_net.ReadFloat("D5506").Content;
                float upperLimit = mc_net.ReadFloat("D5508").Content;
                float lowerLimit = mc_net.ReadFloat("D5510").Content;
                short result = mc_net.ReadInt16 ("D5512").Content;
                float nozzleHeight = mc_net.ReadInt16("D5678").Content/10.0f;
                float powderSupplySpeed= mc_net.ReadInt16("D5677").Content/10.0f;
                float preSprayWeight_1s= mc_net.ReadFloat("D5679").Content;
                float preSprayWeight_1_5s = mc_net.ReadFloat("D5681").Content;
                float preSprayWeight_2s = mc_net.ReadFloat("D5683").Content;
                float preSprayWeight_2_5s = mc_net.ReadFloat("D5685").Content;
                float aftSprayWeight_1s = mc_net.ReadFloat("D5687").Content;
                float aftSprayWeight_1_5s = mc_net.ReadFloat("D5689").Content;
                float aftSprayWeight_2s = mc_net.ReadFloat("D5691").Content;
                float aftSprayWeight_2_5s = mc_net.ReadFloat("D5693").Content;
                testData.PreSprayWeight = preSprayWeight;
                testData.PostSprayWeight = postSprayWeight;
                testData.SedimentationWeight = (float)(Math.Round(Math.Abs(sedimentationWeight), 2)); ;
                testData.WeightUpperLimit = upperLimit;
                testData.WeightLowerLimit = lowerLimit;
                testData.WeightResult = result==1?"OK":"NG";
                testData.Location = "A";
                testData.AddTime = DateTime.Now;
                testData.NozzleHeight = nozzleHeight;
                testData.PowderSupplySpeed= powderSupplySpeed;
                testData.PreSprayWeight_1 = preSprayWeight_1s;
                testData.PreSprayWeight_1_5= preSprayWeight_1_5s;
                testData.PreSprayWeight_2= preSprayWeight_2s;
                testData.PreSprayWeight_2_5= preSprayWeight_2_5s;
                testData.PostSprayWeight_1 = aftSprayWeight_1s;
                testData.PostSprayWeight_1_5= aftSprayWeight_1_5s;
                testData.PostSprayWeight_2= aftSprayWeight_2s;
                testData.PostSprayWeight_2_5= aftSprayWeight_2_5s;

                LogService.AddLogToEnqueue($"获取数据：{testData}");

               return testData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("读取PLC数据Csv异常：" + ex.Message+"\r\n"+ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 获取[称重2]工序数据;
        /// </summary>
        /// <returns></returns>
        public TestData GetTestData2(TestData testData)
        {
            try
            {
                if (mc_net == null)
                {
                    ConnectPLC();
                }

                //[称重2]-数据地址表格里没有，是否有些和[称重1]共用?
                float preSprayWeight = mc_net.ReadFloat("D5970").Content;//喷前重量2(单位;g)
                float postSprayWeight = mc_net.ReadFloat("D5972").Content;//喷后重量2
                float sedimentationWeight = mc_net.ReadFloat("D5974").Content;//沉积重量2
                float upperLimit = mc_net.ReadFloat("D5508").Content;//重量上限-和称重1共用一个地址?
                float lowerLimit = mc_net.ReadFloat("D5510").Content;//重量下限-和称重1共用一个地址?
                short result = mc_net.ReadInt16("D5976").Content;//重量结果2(0:待检测，1:OK,2:NG)
                float nozzleHeight = mc_net.ReadInt16("D5678").Content / 10.0f;//喷嘴高度，和称重1共用一个地址?
                float powderSupplySpeed = mc_net.ReadInt16("D5677").Content / 10.0f;//供粉速度，和称重1共用一个地址?
                float preSprayWeight_1s = mc_net.ReadFloat("D5978").Content;//喷前重量1.0S
                float preSprayWeight_1_5s = mc_net.ReadFloat("D5980").Content;//D5980	喷前重量1.5S
                float preSprayWeight_2s = mc_net.ReadFloat("D5982").Content;//D5982	喷前重量2.0S
                float preSprayWeight_2_5s = mc_net.ReadFloat("D5984").Content;//D5984	喷前重量2.5S
                float aftSprayWeight_1s = mc_net.ReadFloat("D5986").Content;//D5986	喷后重量1.0S
                float aftSprayWeight_1_5s = mc_net.ReadFloat("D5988").Content;//D5988	喷后重量1.5S
                float aftSprayWeight_2s = mc_net.ReadFloat("D5990").Content;//D5990	喷后重量2.0S
                float aftSprayWeight_2_5s = mc_net.ReadFloat("D5992").Content;//D5992	喷后重量2.5S

                testData.PreSprayWeight = preSprayWeight;
                testData.PostSprayWeight = postSprayWeight;
                testData.SedimentationWeight = (float)(Math.Round(Math.Abs(sedimentationWeight), 2)); ;
                testData.WeightUpperLimit = upperLimit;
                testData.WeightLowerLimit = lowerLimit;
                testData.WeightResult = result == 1 ? "OK" : "NG";
                testData.Location = "B";
                testData.AddTime = DateTime.Now;
                testData.NozzleHeight = nozzleHeight;
                testData.PowderSupplySpeed = powderSupplySpeed;
                testData.PreSprayWeight_1 = preSprayWeight_1s;
                testData.PreSprayWeight_1_5 = preSprayWeight_1_5s;
                testData.PreSprayWeight_2 = preSprayWeight_2s;
                testData.PreSprayWeight_2_5 = preSprayWeight_2_5s;
                testData.PostSprayWeight_1 = aftSprayWeight_1s;
                testData.PostSprayWeight_1_5 = aftSprayWeight_1_5s;
                testData.PostSprayWeight_2 = aftSprayWeight_2s;
                testData.PostSprayWeight_2_5 = aftSprayWeight_2_5s;

                LogService.AddLogToEnqueue($"称重2-获取数据：{testData}");

                return testData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog("称重2-读取PLC数据Csv异常：" + ex.Message + "\r\n" + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 获取NG信息;
        /// </summary>
        /// <returns></returns>
        private TestData GetNgInfo(TestData testData)
        {
            string ngResult = "OK";
            string ngReason = "";
            List<string> explicitProp = new List<string>() {
                "ID","Code","AddTime","PressureTestResult","PressureTestResult","Result","NgResult"
            };
            PropertyInfo[] props = typeof(TestData).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                try
                {
                    string propName = prop.Name;
                    E_TestProject project = PubData.TestProjectItems.Find(x => x.ProjectCode == prop.Name);
                    if (project != null)
                    {
                        float lsl = project.LSL_Val;
                        float usl = project.USL_Val;
                        int sType = project.SType; //1:单边公差 2:双边公差

                        if(prop.PropertyType.Name != "Single")
                        {
                            continue;
                        }
                        
                        if (prop.GetValue(testData) != null)
                        {
                            float propValue = (float)prop.GetValue(testData);
                            if (propValue > usl || propValue < lsl)
                            {
                                ngResult = "NG";
                                ngReason += $"*{project.ProjectName}测试值:{propValue}不在范围[{lsl}-{usl}]内";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrLog("采集数据判断NG异常:" + ex.Message);
                    continue;
                }
            }
            //if(ngResult == "OK" && testData.ProductResult!=0)
            //{
            //    ngResult = "NG";
            //    ngReason += testData.PressureTestResult;
            //}
            //testData.Result = ngResult;
            //testData.NgReason = ngReason;
            //if (ngResult == "NG")
            //{
            //    LogService.AddLogToEnqueue($"测试NG原因:" + ngReason);
            //}
            return testData;
        }

        /// <summary>
        /// 保存数据;
        /// </summary>
        //private void SaveTestData(TestData testData)
        //{
        //    if(!string.IsNullOrWhiteSpace(testData.Code))
        //    {
        //        string dataDir = $@"D:\\CsvData\\{DateTime.Now:yyyyMMdd}";
        //        if (!Directory.Exists(dataDir))
        //        {
        //            Directory.CreateDirectory(dataDir);
        //        }
        //        string filePath = $@"{dataDir}\{testData.Code}.csv";
        //        // 准备Excel数据
        //        var excelData = new List<object>
        //{
        //    new
        //    {
        //        二维码 = testData.Code,
        //        喷前重量 = testData.PreSprayWeight,
        //        喷后重量 = testData.PostSprayWeight,
        //        沉积重量 = testData.SedimentationWeight,
        //        重量上限 = testData.UpperLimit,
        //        重量下限=testData.LowerLimit,
        //        结果=testData.Result==1?"合格":"不合格",
        //        测试时间=testData.AddTime
        //    }
        //};
        //        MiniExcel.SaveAs(filePath, excelData, true, "曲线数据", overwriteFile: true);
        //    }
        //}


    }
}
