using HslCommunication;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;  // for xlsx
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Org.BouncyCastle.Ocsp;
using PR_DAL;
using PR_Model;
using PR_SPC;
using PR_Spc_Tester.Moudules;
using PR_Spc_Tester.PRControl;
using PR_Spc_Tester.Services;
using PR_Spc_Tester.UserForms;
using ScottPlot;
using Sunny.UI;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR_Spc_Tester
{
    public partial class FrmMain : UIForm
    {
        public FrmMain()
        {
            InitializeComponent();
            InitializeAlarmDictionary();
        }
        private Dictionary<string, string> alarmDictionary = new Dictionary<string, string>();
        private static readonly object _lock = new object();
        private SpcHelper spcHelper = new SpcHelper();
        MecPlcHelper plcHelper = new MecPlcHelper();
        PlcClearHelper clearHelper = new PlcClearHelper();
        private int TestCount = 0;
        private int LastId = 0; //最新ID;
        private bool PlcEnable = false;
        private PLCAlarmDal pLCAlarmDal = new PLCAlarmDal();
        private PLCAlarm _pLCAlarm = new PLCAlarm();
        TcpClientHelper gunHelp = new TcpClientHelper();
        SamplingData _samlingData = new SamplingData();
        private static readonly object _fileLock = new object();
        private readonly object _listLock = new object();
        TestProjectDal projectdal = new TestProjectDal();
        List<E_TestProject> list = null;
        /// <summary>
        /// 测试数据
        /// </summary>
        TestDataDal dal = new TestDataDal();
        // Realtime binding structures
        private BindingList<TestData> realtimeList = new BindingList<TestData>();
        private BindingSource realtimeBinding = new BindingSource();
        private Dictionary<string, TestData> realtimeDict = new Dictionary<string, TestData>();
        private int realtimeMaxCount = 500; // max items to keep in realtime list
        // Alarm binding structures
        private BindingList<PLCAlarm> alarmList = new BindingList<PLCAlarm>();
        private BindingSource alarmBinding = new BindingSource();
        // TestDataServerDal dalRemote = new TestDataServerDal(ConfigAppSettings.GetValue("ConStringRemote"));
        /// <summary>
        /// 测试项目
        /// </summary>
        TestProjectDal testProjectDal = new TestProjectDal();
        int placementTimeout = 6;
        private bool allowSensorIdleBeforeActive = false;
        // 配置: 是否发送 SESSION RESET（默认不发送）
        private bool SendSessionReset = false;

        private void InitializeAlarmDictionary()
        {
            alarmDictionary.Add("D5800", "喷淋左移载顶升气缸动作故障");
            alarmDictionary.Add("D5801", "喷淋右移载顶升气缸动作故障");
            alarmDictionary.Add("D5802", "称重正定位气缸动作故障。");
            alarmDictionary.Add("D5803", "称重侧定位气缸动作故障。");
            alarmDictionary.Add("D5804", "机器人前夹爪气缸动作故障。");
            alarmDictionary.Add("D5805", "机器人后夹爪气缸动作故障。");
            alarmDictionary.Add("D5806", "称重顶升气缸动作故障。");
            alarmDictionary.Add("D5807", "下料定位基准气缸动作故障。");
            alarmDictionary.Add("D5808", "下料定位气缸动作故障。");
            alarmDictionary.Add("D5809", "A通道产品没放平。");
            alarmDictionary.Add("D5810", "B通道产品没放平。");
            alarmDictionary.Add("D5811", "扫码失败。。");
            alarmDictionary.Add("D5812", "A通道有产品请拿走后再复位。");
            alarmDictionary.Add("D5813", "B通道有产品请拿走后再复位。");
            alarmDictionary.Add("D5814", "监控检测结构距离异常。");
            alarmDictionary.Add("D5815", "粉尘浓度超上限。");
            alarmDictionary.Add("D5816", "喷淋机器人不在原点。");
            alarmDictionary.Add("D5817", "喷淋机器人不在远程模式。");
            alarmDictionary.Add("D5818", "喷淋机器人上电失败。");
            alarmDictionary.Add("D5819", "喷淋机器人启动失败。");
            alarmDictionary.Add("D5820", "喷淋机器人内部报警。。");
            alarmDictionary.Add("D5821", "喷淋机器人初始化失败。");
            alarmDictionary.Add("D5822", "机器人急停中。");
            alarmDictionary.Add("D5823", "机器人下料位是NG产品请拿走。");
            alarmDictionary.Add("D5824", "产品没清洗或录入二维码。");
            alarmDictionary.Add("D5825", "产品清洗后摆放时间超时。");
            alarmDictionary.Add("D5826", "氮气浓度超上限。");
            alarmDictionary.Add("D5827", "喷淋系统故障。");
            alarmDictionary.Add("D5828", "上位机喷淋数据异常。");
            alarmDictionary.Add("D5829", "上位机监控数据异常。");
            alarmDictionary.Add("D5830", "上位机通信异常。");
            alarmDictionary.Add("D5831", "取放料机器人不在原点。");
            alarmDictionary.Add("D5832", "取放料机器人不在远程模式。");
            alarmDictionary.Add("D5833", "取放料机器人上电失败。");
            alarmDictionary.Add("D5834", "取放料机器人启动失败。");
            alarmDictionary.Add("D5835", "取放料机器人内部报警。");
            alarmDictionary.Add("D5836", "取放料机器人初始化失败。");
            alarmDictionary.Add("D5837", "喷淋房门被打开。");
        }

        private async Task ReadPLCAlarm()
        {
            await Task.Delay(500);
            string overPLCAddress = "D5800";
            try
            {
                while (true)
                {

                    for (int i = 0; i < 38; i++)
                    {
                        string PLCAddress = "D5800";
                        // 提取数字部分并转换为整数
                        int number = int.Parse(PLCAddress.Substring(1));
                        number = number + i; // 增加1

                        // 重新组合字符串
                        PLCAddress = "D" + number.ToString();

                        OperateResult<short> resultAlarm = plcHelper.ReadPLCAlarm(PLCAddress);
                        //LogService.AddLogToEnqueue($"从{PLCAddress}读取结果{resultAlarm.Content}");
                        if (resultAlarm != null && resultAlarm.Content == 1)
                        {
                            overPLCAddress = PLCAddress;
                            if (alarmDictionary.ContainsKey(PLCAddress))
                            {
                                pLCAlarmDal.Add(new PLCAlarm
                                {
                                    AlarmMessage = alarmDictionary[PLCAddress].Trim(),
                                    AddTime = DateTime.Now
                                });
                                _pLCAlarm.AddTime = DateTime.Now;
                                _pLCAlarm.AlarmMessage = alarmDictionary[PLCAddress].Trim();
                                ShowPLCAlarm(_pLCAlarm);
                            }
                        }

                    }
                    while (true)
                    {
                        OperateResult<short> resultAlarmReset = plcHelper.ReadPLCAlarm(overPLCAddress);
                        if (resultAlarmReset.Content == 0)
                        {
                            break;
                        }
                        await Task.Delay(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue($"读取PLC报警错误{ex.Message}{ex.StackTrace}");
            }
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            LogService.AddLogToEnqueue("程序启动...");
            string schemaCheckMessage = DbSchemaInitializer.EnsureSizeDvColumns();
            LogService.AddLogToEnqueue(schemaCheckMessage, schemaCheckMessage.Contains("失败") ? EnumMsgType.Exception : EnumMsgType.Info);
            int.TryParse(ConfigAppSettings.GetValue("PlacementTimeout"), out placementTimeout);
            LogService.AddLogToEnqueue("摆放超时时间: " + placementTimeout+"H");
            allowSensorIdleBeforeActive = (ConfigAppSettings.GetValue("AllowSensorIdleBeforeActive") ?? "0").ToString() == "1";
            LogService.AddLogToEnqueue($"SENSOR IDLE预处理开关: {(allowSensorIdleBeforeActive ? "开启" : "关闭")}", EnumMsgType.Info);
            // 读取 SESSION RESET 发送开关，默认不发送（0）
            SendSessionReset = (ConfigAppSettings.GetValue("SendSessionReset") ?? "0").ToString() == "1";
            LogService.AddLogToEnqueue($"SESSION RESET发送开关: {(SendSessionReset ? "开启" : "关闭")}", EnumMsgType.Info);
            list = projectdal.GetList();
            DateTime timeNow = DateTime.Now;
            this.Invoke(new Action(() =>
            {
                uiLabel2.Text = timeNow.ToString();
            }));
            string dateStr = timeNow.ToString("yyyy-MM-dd");
            uidtp_start.Value = Convert.ToDateTime($"{dateStr} 00:00:00");
            uidtp_end.Value = Convert.ToDateTime($"{dateStr} 23:59:59");

            lb_userName.Text = PubInfo.UserName;
            lb_right.Text = PubInfo.UserTypeStr;

            PlcEnable = ConfigAppSettings.GetValue("PlcEnable").ToString() == "1";

            Thread.Sleep(1000);

            InitDataGridSpc();
            // realtime binding init
            realtimeBinding.DataSource = realtimeList;
            dgv_realtime.AutoGenerateColumns = false;
            dgv_realtime.DataSource = realtimeBinding;
            try
            {
                dgv_realtime.Columns["Col_Code"].DataPropertyName = "Code";
                dgv_realtime.Columns["Col_AddTime"].DataPropertyName = "AddTime";
                dgv_realtime.Columns["Col_PostSprayWeight"].DataPropertyName = "PostSprayWeight";
                dgv_realtime.Columns["Col_SedimentationWeight"].DataPropertyName = "SedimentationWeight";
                dgv_realtime.Columns["Col_Result"].DataPropertyName = "WeightResult";
                dgv_realtime.Columns["Col_UtilizationRate"].DataPropertyName = "UtilizationRate";
            }
            catch { }
            // alarm binding init
            alarmBinding.DataSource = alarmList;
            dgv_alarmmessage.AutoGenerateColumns = false;
            dgv_alarmmessage.DataSource = alarmBinding;
            TestCount = dal.GetTotalCount();

            UpdateCpk();

            Task.Factory.StartNew(new Action(() =>
            {
                ShowLog();
            }), TaskCreationOptions.LongRunning);
            LogService.AddLogToEnqueue("开始连接PLC");
            Task task = Task.Factory.StartNew(new Action(() =>
            {
                if (PlcEnable)
                {
                    while (true)
                    {
                        bool resultPLC = plcHelper.ConnectPLC();
                        bool resultGun = gunHelp.ConnectServer();
                        bool resultClear = clearHelper.ConnectPLC();
                        //bool resultGun = true;
                        if (resultPLC && resultGun && resultClear)
                        {
                            LogService.AddLogToEnqueue("PLC连接成功！");
                            Task.Factory.StartNew(PlcClearAction, TaskCreationOptions.LongRunning);
                            Task.Factory.StartNew(ClearHeartbeatAction, TaskCreationOptions.LongRunning);//clear plc heartbeat
                            Task.Factory.StartNew(PlcHeavyAction, TaskCreationOptions.LongRunning);
                            Task.Factory.StartNew(PlcHeavyAction2, TaskCreationOptions.LongRunning);
                            Task.Factory.StartNew(PlcColdSprayAction, TaskCreationOptions.LongRunning);
                            Task.Factory.StartNew(PlcMonitorAction, TaskCreationOptions.LongRunning);
                            Task.Factory.StartNew(ReadPLCAlarm, TaskCreationOptions.LongRunning);
                            Task.Factory.StartNew(WritePLCTemperature, TaskCreationOptions.LongRunning);
                            Task.Factory.StartNew(WritePLCTemperature2, TaskCreationOptions.LongRunning);
                            break;
                        }
                        else
                        {
                            LogService.AddLogToEnqueue($"连接PLC、冷喷枪或清洗机失败，请检查网络,3S 后程序重连PLC连接{resultPLC}, 冷喷枪连接{resultGun}, 清洗机连接{resultClear}", EnumMsgType.Error);
                            Thread.Sleep(3000);
                        }
                    }
                }
                else
                {
                    LogService.AddLogToEnqueue($"数据端，不读取PLC数据");
                    Task.Factory.StartNew(UpdateNewData, TaskCreationOptions.LongRunning);
                }
            }));


            int FailedTimes = 0;
            int heartBeatTimes = 0;
            Task.Factory.StartNew(new Action(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (plcHelper.IsConnected() == false)
                    {
                        bool ressult = plcHelper.ConnectPLC();
                        LogService.AddLogToEnqueue($"重连PLC结果：{ressult}");
                        continue;
                    }
                    OperateResult result_write = plcHelper.WritePCHeart(1);
                    if (result_write.IsSuccess)
                    {
                        heartBeatTimes++;
                        if (heartBeatTimes == 1000)
                        {
                            heartBeatTimes = 1;
                        }
                        if (heartBeatTimes % 2 == 0)
                        {
                            ControlHelper.Invoke(lb_heartbeat, delegate
                            {
                                lb_heartbeat.BackColor = Color.Green;
                            });
                        }
                        else
                        {
                            lb_heartbeat.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        FailedTimes++;
                        LogService.AddLogToEnqueue("写入心跳失败" + result_write.Message);
                        ControlHelper.Invoke(lb_heartbeat, delegate
                        {
                            lb_heartbeat.BackColor = Color.Red;
                        });
                    }
                    if (FailedTimes >= 3)
                    {
                        FailedTimes = 0;
                        LogService.AddLogToEnqueue($"PLC 通信故障【心跳异常】", EnumMsgType.Error);
                        //重连PLC;
                        plcHelper.ConnectPLC();
                    }
                    Thread.Sleep(1000);
                    plcHelper.WritePCHeart(0);
                }
            }), TaskCreationOptions.LongRunning);
        }
        private void UpdateNewData()
        {
            while (true)
            {
                Thread.Sleep(3000);
                TestData testData = dal.GetLastDataById(LastId);
                if (testData != null)
                {
                    LastId = testData.ID;
                    UpdateUI(testData);
                    LogService.AddLogToEnqueue($"收到一条测试数据：{testData}");
                }
            }
        }
        private void IsOverTime(TestData testData, string code)
        {
            DateTime startTime = DateTime.Now;
            if (testData.PlacementTime == null && testData != null)
            {
                LogService.AddLogToEnqueue($"比对条码{code}摆放时间为空", EnumMsgType.Exception);
                return;
            }
            DateTime placementTime = (DateTime)testData.PlacementTime;
            TimeSpan duration = startTime - placementTime;
            if (duration.TotalHours > placementTimeout)
            {
                LogService.AddLogToEnqueue($"二维码{code}时间间隔大于{placementTimeout}小时", EnumMsgType.Exception);
                if (!plcHelper.WriteOverTime(2).IsSuccess)
                {
                    plcHelper.WriteOverTime(2);
                }
            }
            else
            {
                LogService.AddLogToEnqueue($"二维码{code}时间间隔正常", EnumMsgType.Info);
                if (!plcHelper.WriteOverTime(1).IsSuccess)
                {
                    plcHelper.WriteOverTime(1);
                }
            }
        }

        private void IsOverTime2(TestData testData, string code)
        {
            DateTime startTime = DateTime.Now;
            if (testData.PlacementTime == null && testData != null)
            {
                LogService.AddLogToEnqueue($"比对2->比对条码{code}摆放时间为空", EnumMsgType.Exception);
                return;
            }
            DateTime placementTime = (DateTime)testData.PlacementTime;
            TimeSpan duration = startTime - placementTime;
            if (duration.TotalHours > placementTimeout)
            {
                LogService.AddLogToEnqueue($"比对2->二维码{code}时间间隔大于{placementTimeout}小时", EnumMsgType.Exception);
                if (!plcHelper.WriteOverTime2(2).IsSuccess)
                {
                    plcHelper.WriteOverTime2(2);
                }
            }
            else
            {
                LogService.AddLogToEnqueue($"比对2->二维码{code}时间间隔正常", EnumMsgType.Info);
                if (!plcHelper.WriteOverTime2(1).IsSuccess)
                {
                    plcHelper.WriteOverTime2(1);
                }
            }
        }
        /// <summary>
        /// 获取1号冷喷测试数据;
        /// </summary>
        private async Task PlcColdSprayAction()
        {
            LogService.AddLogToEnqueue($"冷喷->1号对比线程开始", EnumMsgType.Info);
            TestData testData = new TestData();
            DateTime startTime;
            DateTime endTime;
            const int coldSpraySignalPollMs = 50;
            const int opcConfirmPollMs = 50;
            const int opcConfirmTimeoutMs = 200;
            while (true)
            {
                Thread.Sleep(coldSpraySignalPollMs);
                try
                {
                    // 与PLC的握手约定：
                    // D5670 = 1 表示PLC已准备好本次冷喷数据；
                    // 上位机完成冷喷采样后，等待PLC将D5670写为2，随后上位机再写复位信号。
                    OperateResult<short> result = plcHelper.GetColdSprayReadReady();//PLC 5670地址为冷喷读取准备好信号，值为1时准备好，等待PLC写入2表示测试完成
                    if (result.Content == 1)
                    {
                        DateTime coldSprayReadyAt = DateTime.Now;
                        startTime = coldSprayReadyAt;
                        LogService.AddLogToEnqueue("收到冷喷读取数据信号");
                        // 收到冷喷启动信号后优先发送激活命令，减少启动侧等待。
                        HiWatchOpcMonitor opcSession = null;
                        try
                        {
                            string url = ConfigAppSettings.GetValue("MonitorIP").ToString();
                            opcSession = new HiWatchOpcMonitor();
                            await opcSession.ConnectAsync(url);
                            bool activeConfirmed = await opcSession.EnsureSensorStandbyThenActivateAsync(opcConfirmTimeoutMs, opcConfirmPollMs, allowSensorIdleBeforeActive);
                            if (opcSession.LastSensorActiveCommandSentAt.HasValue)
                            {
                                startTime = opcSession.LastSensorActiveCommandSentAt.Value;
                                LogService.AddLogToEnqueue($"冷喷->节拍开始时间取COMMAND SENSOR ACTIVE发送时刻:{startTime:yyyy-MM-dd HH:mm:ss.fff}", EnumMsgType.Info);
                            }
                            else
                            {
                                LogService.AddLogToEnqueue($"冷喷->未获取到COMMAND SENSOR ACTIVE发送时刻，节拍开始时间回退为PLC准备信号时刻:{coldSprayReadyAt:yyyy-MM-dd HH:mm:ss.fff}", EnumMsgType.Exception);
                            }

                            if (activeConfirmed)
                            {
                                LogService.AddLogToEnqueue("COMMAND SENSOR ACTIVE执行确认成功", EnumMsgType.Info);
                            }
                            else
                            {
                                LogService.AddLogToEnqueue("COMMAND SENSOR ACTIVE执行确认失败，按非阻断策略继续后续流程", EnumMsgType.Exception);
                            }

                            if (SendSessionReset)
                            {
                                try
                                {
                                    bool resetConfirmed = await opcSession.WriteSessionResetAndConfirmAsync(opcConfirmTimeoutMs, opcConfirmPollMs);
                                    if (resetConfirmed)
                                    {
                                        LogService.AddLogToEnqueue("SESSION RESET执行确认成功", EnumMsgType.Info);
                                    }
                                    else
                                    {
                                        LogService.AddLogToEnqueue("SESSION RESET执行确认失败，按非阻断策略继续后续流程", EnumMsgType.Exception);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogService.AddLogToEnqueue("写SESSION RESET失败:" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                                }
                            }
                            else
                            {
                                LogService.AddLogToEnqueue("SESSION RESET发送被配置为关闭，跳过发送", EnumMsgType.Info);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogService.AddLogToEnqueue("写SESSION RESET失败:" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                        }

                        OperateResult<string> resultCode = plcHelper.GetColdSprayCode();//D5570 地址为冷喷二维码，长度49
                        if (resultCode.Content == "")
                        {
                            LogService.AddLogToEnqueue($"读取冷喷二维码为{resultCode.Content}");
                            try { opcSession?.Dispose(); } catch { }
                            continue;
                        }

                        string coldSprayCode = resultCode.Content.Trim();
                        LogService.AddLogToEnqueue($"读取冷喷二维码为{coldSprayCode}", EnumMsgType.Info);
                        testData = dal.GetLastDataByCode(coldSprayCode);
                        await Task.Delay(100);
                        if (testData == null)
                        {
                            LogService.AddLogToEnqueue($"冷喷条码{coldSprayCode}不存在于数据库中", EnumMsgType.Exception);
                            if (!plcHelper.WriteNotCode(1).IsSuccess)
                            {
                                plcHelper.WriteNotCode(1);
                            }
                            while (true)
                            {
                                await Task.Delay(1000);
                                OperateResult<short> resultNotCodeRest = plcHelper.ResetAbnormal();
                                if (resultNotCodeRest.Content == 1)
                                {
                                    LogService.AddLogToEnqueue($"冷喷条码{coldSprayCode}不存在于数据库库异常已复位，继续测试");
                                    try { opcSession?.Dispose(); } catch { }
                                    break;
                                }
                            }
                            continue;
                        }
                        DateTime placementTime = (DateTime)testData.PlacementTime;
                        TimeSpan duration = startTime - placementTime;
                        testData.PlacementHour = duration.ToString(@"hh\:mm");
                        OperateResult<short> resultRest = plcHelper.SwitchPowderCans();
                        if (resultRest.Content == 0)
                        {
                            resultRest = plcHelper.SwitchPowderCans();
                        }
                        LogService.AddLogToEnqueue($"冷喷->条码{testData.Code}读取切换粉罐的值为{resultRest.Content}", EnumMsgType.Info);
                        if (testData != null)
                        {
                            testData.Code = coldSprayCode;
                            testData.IntakePressure = plcHelper.GetIntakePressure().Content;
                            while (true)
                            {
                                await Task.Delay(coldSpraySignalPollMs);
                                var updatedData = gunHelp.SendString(testData, resultRest.Content);
                                if (updatedData != null)
                                {
                                    testData = updatedData;
                                }
                                else
                                {
                                    LogService.AddLogToEnqueue($"冷喷->条码{coldSprayCode}读取喷枪数据失败，保留上一帧有效数据继续", EnumMsgType.Exception);
                                }
                                OperateResult<short> resultOver = plcHelper.GetColdSprayReadReady();
                                // PLC写2表示本次冷喷流程结束，上位机可进行收尾与落库。
                                if (resultOver.Content == 2)
                                {
                                    break;
                                }
                            }
                            endTime = DateTime.Now;
                            testData.StartTime = startTime;
                            testData.EndTime = endTime;
                            duration = endTime - startTime;
                            testData.Beat = (float)Math.Round(duration.TotalSeconds, 2);
                            LogService.AddLogToEnqueue($"冷喷->条码{testData.Code}开始:{testData.StartTime:yyyy-MM-dd HH:mm:ss.fff} 结束:{testData.EndTime:yyyy-MM-dd HH:mm:ss.fff} 节拍:{testData.Beat:F2}s", EnumMsgType.Info);
                            if (testData.Beat <= 0)
                            {
                                // 理论上不会<=0，如出现则保底写入最小值并留日志，避免后续利用率分母为0。
                                testData.Beat = 0.01f;
                                LogService.AddLogToEnqueue($"冷喷->条码{testData.Code}节拍异常<=0，已回退为{testData.Beat}s", EnumMsgType.Exception);
                            }
                            if (testData.IntakePressure < testData.IntakePressureLowerLimit)
                            {
                                testData.IntakePressureResult = "NG";
                            }
                            else
                            {
                                testData.IntakePressureResult = "OK";
                            }

                            //进气流量 
                            testData.IntakeFlow = plcHelper.GetIntakeFlow().Content;

                            LogService.AddLogToEnqueue($"冷喷->条码{testData.Code}记录冷喷完成信号=2", EnumMsgType.Info);
                            // 冷喷完成后向 OPC 写入 SESSION STORE {code}
                            try
                            {
                                if (opcSession != null)
                                {
                                    // 仅写 SESSION STORE（不发送 COMMAND SENSOR IDLE）
                                    bool storeConfirmed = await opcSession.WriteSessionStoreAndConfirmAsync(testData.Code, 50);
                                    if (storeConfirmed)
                                    {
                                        LogService.AddLogToEnqueue($"冷喷->条码{testData.Code} SESSION STORE执行确认成功", EnumMsgType.Info);
                                    }
                                    else
                                    {
                                        LogService.AddLogToEnqueue($"冷喷->条码{testData.Code} SESSION STORE执行确认失败，按非阻断策略继续后续流程", EnumMsgType.Exception);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogService.AddLogToEnqueue($"冷喷->条码{testData.Code}写SESSION STORE失败:" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                            }
                            if (!plcHelper.ResetColdSprayReady().IsSuccess)
                            {
                                plcHelper.ResetColdSprayReady();
                            }
                            try { opcSession?.Dispose(); } catch { }
                            if (dal.UpColdSpraydate(testData))
                            {
                                UpdateUI(testData);
                                this.Invoke(new Action(() => { LoadHis(); }));
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue("采集冷喷数据异常:" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                }
            }
        }

        /// <summary>
        /// 读取监控数据
        /// </summary>
        private async Task PlcMonitorAction()
        {
            LogService.AddLogToEnqueue("监控->Start...");
            while (true)
            {
                try
                {
                    OperateResult<short> result = plcHelper.GetMonitorReadReady();
                    if (result.Content == 1)
                    {
                        LogService.AddLogToEnqueue("收到监控读取数据信号");

                        // 1. 从PLC获取二维码
                        OperateResult<string> resultCode = plcHelper.GetMonitorCode();
                        if (string.IsNullOrWhiteSpace(resultCode.Content))
                        {
                            LogService.AddLogToEnqueue("读取监控二维码为空", EnumMsgType.Exception);
                            continue;
                        }

                        string monitorCode = resultCode.Content.Trim();
                        LogService.AddLogToEnqueue($"读取监控二维码为{monitorCode}", EnumMsgType.Info);
                        TestData testData = dal.GetLastDataByCode(monitorCode);

                        if (testData == null)
                        {
                            LogService.AddLogToEnqueue($"监控条码{monitorCode}不存在数据库中", EnumMsgType.Exception);
                            if (!plcHelper.WriteNotCode(1).IsSuccess)
                            {
                                plcHelper.WriteNotCode(1);
                            }
                            while (true)
                            {
                                await Task.Delay(1000);
                                OperateResult<short> resultNotCodeRest = plcHelper.ResetAbnormal();
                                if (resultNotCodeRest.Content == 1)
                                {
                                    LogService.AddLogToEnqueue($"监控条码{monitorCode}不存在数据库异常已复位，继续测试");
                                    break;
                                }
                            }
                            continue;
                        }
                        List<double> temperatureList = new List<double>();
                        List<double> nitrogenPressureList = new List<double>();
                        List<double> speedList = new List<double>();
                        List<double> positionList = new List<double>();
                        List<double> concentrationList = new List<double>();
                        List<DateTime> timesList = new List<DateTime>();

                        using (HiWatchOpcMonitor opcMonitor = new HiWatchOpcMonitor())
                        {
                            string url = ConfigAppSettings.GetValue("MonitorIP").ToString();
                            await opcMonitor.ConnectAsync(url);
                            _samlingData.ID = 0;
                            LogService.AddLogToEnqueue($"监控->条码{testData.Code} OPC UA连接成功", EnumMsgType.Info);
                            // 开始收集OPC UA数据
                            LogService.AddLogToEnqueue($"监控->条码{testData.Code}开始采集OPC UA数据", EnumMsgType.Info);
                            // 每个条码开始监控前清空缓存，避免将上一次条码的样本混入本次统计。
                            opcMonitor.ClearData();
                            // 添加事件处理程序
                            opcMonitor.OnSpeedReceived += (object sender, OpcValueEventArgs e) =>
                            {
                                //_samlingData.speed = Math.Round(e.Value, 2);
                                //speedList.Add((float)e.Value);
                            };
                            opcMonitor.OnPositionReceived += (object sender, OpcValueEventArgs e) =>
                            {
                                //_samlingData.position = Math.Round(e.Value, 2);
                                //positionList.Add((float)e.Value);
                                //LogService.AddLogToEnqueue($"位置数据已添加到队列，当前值: {e.Value}，队列大小: {positionList.Count}");
                            };
                            opcMonitor.OnDensityReceived += (object sender, OpcValueEventArgs e) =>
                            {
                                //lock (_listLock)
                                //{
                                //    _samlingData.ID++;
                                //    _samlingData.concentration = Math.Round(e.Value, 2);
                                //    _samlingData.Time = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                                //    timesList.Add(e.Timestamp);
                                //    concentrationList.Add((float)e.Value);
                                //    temperatureList.Add((float)_samlingData.temperature);
                                //    nitrogenPressureList.Add((float)_samlingData.pressure);
                                //    SaveCurrentDataToCsv(testData.Code);
                                //}

                            };
                            opcMonitor.OnAllDataReceived += (sender, e) =>
                            {
                                lock (_listLock)
                                {
                                    _samlingData.ID++;
                                    _samlingData.concentration = Math.Round(e.Density, 2);//浓度
                                    _samlingData.position = Math.Round(e.Position, 2);//位置
                                    _samlingData.speed = Math.Round(e.Speed, 2);//速度
                                    _samlingData.Time = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                                    timesList.Add(e.Timestamp);
                                    concentrationList.Add((float)e.Density);
                                    positionList.Add((float)e.Position);
                                    speedList.Add((float)e.Speed);
                                    // 温度/氮气压力来自喷枪TCP线程写入的共享结构，不是OPC节点本身。
                                    // 若喷枪链路无数据，这两组序列会被持续写入0。
                                    temperatureList.Add((float)_samlingData.temperature);
                                    nitrogenPressureList.Add((float)_samlingData.pressure);
                                    SaveCurrentDataToCsv(testData.Code);

                                    if (_samlingData.ID % 20 == 0)
                                    {
                                        LogService.AddLogToEnqueue($"监控->条码{testData.Code}已采样{_samlingData.ID}点 速度:{e.Speed:F2} 浓度:{e.Density:F2} 位置:{e.Position:F2}", EnumMsgType.Info);
                                    }
                                }
                            };
                            // 订阅 SizeDV 数据
                            opcMonitor.OnSizeDV50Received += (sender, e) =>
                            {
                                lock (_listLock)
                                {
                                    testData.SizeDV50 = e.Value;
                                }
                            };
                            opcMonitor.OnSizeDV90Received += (sender, e) =>
                            {
                                lock (_listLock)
                                {
                                    testData.SizeDV90 = e.Value;
                                }
                            };
                            opcMonitor.OnAlertMessageReceived += (sender, e) =>
                            {
                                LogService.AddLogToEnqueue($"监控->条码{testData.Code} OPC报警消息触发: {e.Value}", EnumMsgType.Exception);
                                var writeResult = plcHelper.WriteOpcUaAlarmFlag(1);
                                if (!writeResult.IsSuccess)
                                {
                                    LogService.AddLogToEnqueue($"监控->条码{testData.Code}写入PLC报警标志D5725失败: {writeResult.Message}", EnumMsgType.Exception);
                                }
                            };
                            // 等待PLC完成信号
                            while (true)
                            {
                                // 监控完成边界改为10ms轮询，减少1秒级结束抖动。
                                await Task.Delay(10);
                                OperateResult<short> resultOver = plcHelper.GetMonitorReadReady();
                                if (resultOver.Content == 2)
                                {
                                    break;
                                }
                            }
                            LogService.AddLogToEnqueue($"监控->条码{testData.Code}记录监控完成信号=2", EnumMsgType.Info);
                            LogService.AddLogToEnqueue($"监控->条码{testData.Code}采样完成，原始样本数 speed:{speedList.Count} density:{concentrationList.Count} position:{positionList.Count} temp:{temperatureList.Count} pressure:{nitrogenPressureList.Count}", EnumMsgType.Info);
                            opcMonitor.GetCurrentValues(testData);
                            LogService.AddLogToEnqueue($"监控->条码{testData.Code}统计结果 平均速度:{testData.AverageSpeed:F2} 平均浓度:{testData.AverageConcentration:F2} 平均位置:{testData.AveragePosition:F2}", EnumMsgType.Info);
                            testData.AverageSpeedResult = JudgeUpperLower(testData.AverageSpeed, testData.AverageSpeedUpperLimit, testData.AverageSpeedLowerLimit, "平均速度");
                            testData.MinSpeedResult = JudgeUpperLower(testData.MinSpeed, 0, testData.MinSpeedLowerLimit, "最小速度");
                            testData.AverageConcentrationResult = JudgeUpperLower(testData.AverageConcentration, testData.AverageConcentrationUpperLimit, testData.AverageConcentrationLowerLimit, "平均浓度");
                            testData.AverageTemperature = temperatureList.AverageExcludingZero();
                            //testData.MinTemperature = (float)(temperatureList.Count > 0 ? temperatureList.Min() : 0);
                            testData.MinTemperature = temperatureList.MinExcludingZero();
                            testData.MaxTemperature = (float)(temperatureList.Count > 0 ? temperatureList.Max() : 0);
                            testData.AverageTemperatureResult = JudgeUpperLower(testData.AverageTemperature, testData.AverageTemperatureUpperLimit, testData.AverageTemperatureLowerLimit, "平均温度");
                            if (Math.Abs(testData.AverageTemperature) < 0.000001)
                            {
                                testData.AverageTemperatureResult = "NG";
                                LogService.AddLogToEnqueue($"监控->条码{testData.Code}平均温度为0，强制判定NG", EnumMsgType.Exception);
                            }
                            testData.MinTemperatureResult = JudgeUpperLower(testData.MinTemperature, 0, testData.MinTemperatureLowerLimit, "最小温度");
                            testData.AverageNitrogenPressure = nitrogenPressureList.AverageExcludingZero();
                            //testData.MinNitrogenPressure = (float)(nitrogenPressureList.Count > 0 ? nitrogenPressureList.Min() : 0);
                            testData.MinNitrogenPressure = nitrogenPressureList.MinExcludingZero();
                            testData.MaxNitrogenPressure = (float)(nitrogenPressureList.Count > 0 ? nitrogenPressureList.Max() : 0);
                            testData.AverageNitrogenPressureResult = JudgeUpperLower(testData.AverageNitrogenPressure, testData.AverageNitrogenPressureUpperLimit, testData.AverageNitrogenPressureLowerLimit, "平均氮气压力");
                            testData.MinNitrogenPressureResult = JudgeUpperLower(testData.MinNitrogenPressure, 0, testData.MinNitrogenPressureLowerLimit, "最小氮气压力");
                            testData.TemperatureResult = testData.AverageTemperatureResult == "OK" && testData.MinTemperatureResult == "OK" ? "OK" : "NG";
                            testData.NitrogenPressureResult = testData.AverageNitrogenPressureResult == "OK" && testData.MinNitrogenPressureResult == "OK" ? "OK" : "NG";
                            testData.SpeedResult = testData.AverageSpeedResult == "OK" && testData.MinSpeedResult == "OK" ? "OK" : "NG";
                            testData.ConcentrationResult = testData.AverageConcentrationResult == "OK" ? "OK" : "NG";
                            LogService.AddLogToEnqueue($"监控->条码{testData.Code}记录监控完成信号=2", EnumMsgType.Info);
                            // 判定结果：若任一业务维度为 NG，则写入 D5711=4；否则写入 D5711=2 (复位)
                            bool anyNg = testData.WeightResult == "NG" || testData.TemperatureResult == "NG" || testData.AverageNitrogenPressureResult == "NG" || testData.NitrogenPressureResult == "NG" || testData.IntakePressureResult == "NG" || testData.SpeedResult == "NG" || testData.ConcentrationResult == "NG";
                            if (anyNg)
                            {
                                var writeRes = plcHelper.WriteMonitorStatus(4);
                                if (!writeRes.IsSuccess)
                                {
                                    LogService.AddLogToEnqueue($"写入监控结果 D5711=4 失败: {writeRes.Message}", EnumMsgType.Exception);
                                    // 重试一次
                                    writeRes = plcHelper.WriteMonitorStatus(4);
                                    if (!writeRes.IsSuccess)
                                    {
                                        LogService.AddLogToEnqueue($"重试写入监控结果失败: {writeRes.Message}", EnumMsgType.Exception);
                                    }
                                }
                            }
                            else
                            {
                                var resetRes = plcHelper.ResetMonitorReady();
                                if (!resetRes.IsSuccess)
                                {
                                    // 再试一次
                                    resetRes = plcHelper.ResetMonitorReady();
                                    if (!resetRes.IsSuccess)
                                    {
                                        LogService.AddLogToEnqueue($"写入监控复位 D5711=2 失败: {resetRes.Message}", EnumMsgType.Exception);
                                    }
                                }
                            }

                            // 保存数据
                            if (dal.UpMonitordate(testData))
                            {
                                UpdateUI(testData);
                                this.Invoke(new Action(() => { LoadHis(); }));
                            }
                            else
                            {
                                dal.UpMonitordate(testData);
                            }
                            GenerateScatterPlotFromLinkedList(timesList, temperatureList, nitrogenPressureList, speedList, concentrationList, positionList, testData.Code);
                            dal.DeleteByWhere();
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue($"监控数据采集异常: {ex.Message}+{ex.StackTrace}", EnumMsgType.Exception);
                    await Task.Delay(5000); // 错误后延迟重试
                }
                finally
                {
                    await Task.Delay(1000); // 常规间隔
                }
            }
        }

        /// <summary>
        /// 获取称重测试数据;
        /// </summary>
        private void PlcHeavyAction()
        {
            LogService.AddLogToEnqueue("称重1->Start...");
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    OperateResult<short> result = plcHelper.GetReadReady();
                    if (result.Content == 1)
                    {
                        LogService.AddLogToEnqueue("收到称重读取数据信号");
                        OperateResult<string> resultCode = plcHelper.GetHeaverCode();
                        if (resultCode.Content == "")
                        {
                            LogService.AddLogToEnqueue($"读取称重二维码为空", EnumMsgType.Exception);
                            continue;
                        }
                        string heavyCode = resultCode.Content;
                        LogService.AddLogToEnqueue($"读取称重二维码为{heavyCode}", EnumMsgType.Info);
                        TestData testData = new TestData();
                        testData = dal.GetLastDataByCode(heavyCode);
                        Thread.Sleep(100);
                        if (testData == null)
                        {
                            LogService.AddLogToEnqueue($"称重条码{heavyCode}不存在数据库中", EnumMsgType.Exception);
                            if (!plcHelper.WriteNotCode(1).IsSuccess)
                            {
                                plcHelper.WriteNotCode(1);
                            }
                            while (true)
                            {
                                Thread.Sleep(1000);
                                OperateResult<short> resultNotCodeRest = plcHelper.ResetAbnormal();
                                if (resultNotCodeRest.Content == 1)
                                {
                                    LogService.AddLogToEnqueue($"称重条码{heavyCode}不存在数据库异常已复位，继续测试");
                                    break;
                                }
                            }
                            continue;
                        }
                        testData = plcHelper.GetTestData(testData);
                        if (testData != null)
                        {
                            int retryTime = 0;
                            while (retryTime++ < 3)
                            {
                                if (testData.PreSprayWeight == 0 || testData.PostSprayWeight == 0 || testData.SedimentationWeight == 0
                                    || testData.WeightUpperLimit == 0 || testData.WeightLowerLimit == 0)
                                {
                                    Thread.Sleep(retryTime * 200);
                                    LogService.AddLogToEnqueue($"第{retryTime + 1}次读取到0值数据，重读!");
                                    testData = plcHelper.GetTestData(testData);
                                }
                            }
                            // 银粉利用率计算依赖Beat（冷喷节拍，单位秒）：
                            // utilization = 沉积重量 / (供粉速度(g/min) / 60 * Beat(s))
                            // 若Beat=0，分母会变0，后续被判定为Infinity/NaN并强制写成0%。
                            // 这也是“利用率长期为0%”的直接触发条件之一。
                            double denominator = testData.PowderSupplySpeed / 60.0 * testData.Beat;
                            LogService.AddLogToEnqueue($"称重1->利用率输入 条码:{testData.Code} 沉积重量:{testData.SedimentationWeight:F4} 供粉速度:{testData.PowderSupplySpeed:F4} 节拍:{testData.Beat:F2}s 分母:{denominator:F6}", EnumMsgType.Info);
                            if (denominator <= 0)
                            {
                                testData.UtilizationRate = "0%";
                                LogService.AddLogToEnqueue($"称重1->条码{testData.Code}利用率分母<=0，按0%处理", EnumMsgType.Exception);
                            }
                            else
                            {
                                double utilizationRate = testData.SedimentationWeight / denominator;
                                if (double.IsInfinity(utilizationRate) || double.IsNaN(utilizationRate))
                                {
                                    testData.UtilizationRate = "0%";
                                    LogService.AddLogToEnqueue($"称重1->条码{testData.Code}利用率计算结果非法({utilizationRate})，按0%处理", EnumMsgType.Exception);
                                }
                                else
                                {
                                    testData.UtilizationRate = ((float)utilizationRate * 100).ToString("0.00") + "%";
                                    LogService.AddLogToEnqueue($"称重1->条码{testData.Code}利用率={testData.UtilizationRate}", EnumMsgType.Info);
                                }
                            }
                            LogService.AddLogToEnqueue("记录称重完成信号=2");
                            if (!plcHelper.ResetReady().IsSuccess)//->PLC D5700,2
                            {
                                plcHelper.ResetReady();
                            }

                            if (dal.UpHeavydate(testData))
                            {
                                UpdateUI(testData);
                                this.Invoke(new Action(() => { LoadHis(); }));
                            }
                            else
                            {
                                LogService.AddLogToEnqueue($"数据库中不存在{heavyCode}", EnumMsgType.Exception);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue("采集称重数据异常:" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                }
            }
        }

        private void PlcClearAction()
        {
            LogService.AddLogToEnqueue("激光清洗->Start...");
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    OperateResult<short> result = clearHelper.GetReadReady();
                    if (result.Content == 1)
                    {
                        LogService.AddLogToEnqueue("激光清洗->收到二维码读ready信号");
                        OperateResult<string> resultCode = clearHelper.GetBarcode();
                        if (resultCode.Content == "")
                        {
                            LogService.AddLogToEnqueue($"激光清洗->读取二维码为空", EnumMsgType.Exception);
                            if (!clearHelper.WriteAddCodeFailed().IsSuccess)
                            {
                                clearHelper.WriteAddCodeFailed();
                            }
                            continue;
                        }
                        string heavyCode = resultCode.Content;
                        LogService.AddLogToEnqueue($"激光清洗->读取二维码为{heavyCode}", EnumMsgType.Info);
                        // 创建新的测试数据对象
                        TestData testData = new TestData()
                        {
                            Code = heavyCode,                    // 设置二维码
                            PlacementTime = DateTime.Now,    // 设置摆放时间（当前时间）
                        };

                        // 从数据库加载测试项目参数（如果还没加载）
                        if (list == null)
                        {
                            list = projectdal.GetList();  // 获取所有测试项目
                        }

                        // 遍历所有测试项目，设置对应的上下限值
                        foreach (var item in list)
                        {
                            if (item.ProjectCode == "MinTemperature")
                            {
                                testData.MinTemperatureLowerLimit = item.LSL_Val;  // 最低温度下限
                            }
                            else if (item.ProjectCode == "AverageTemperature")
                            {
                                testData.AverageTemperatureLowerLimit = item.LSL_Val;  // 平均温度下限
                                testData.AverageTemperatureUpperLimit = item.USL_Val;  // 平均温度上限
                            }
                            else if (item.ProjectCode == "MinNitrogenPressure")
                            {
                                testData.MinNitrogenPressureLowerLimit = item.LSL_Val;  // 最低氮气压力下限
                            }
                            else if (item.ProjectCode == "AverageNitrogenPressure")
                            {
                                testData.AverageNitrogenPressureLowerLimit = item.LSL_Val;  // 平均氮气压力下限
                                testData.AverageNitrogenPressureUpperLimit = item.USL_Val;  // 平均氮气压力上限
                            }
                            else if (item.ProjectCode == "IntakePressure")
                            {
                                testData.IntakePressureLowerLimit = item.LSL_Val;  // 进气压力下限
                            }
                            else if (item.ProjectCode == "MinSpeed")
                            {
                                testData.MinSpeedLowerLimit = item.LSL_Val;  // 最低速度下限
                            }
                            else if (item.ProjectCode == "AverageSpeed")
                            {
                                testData.AverageSpeedLowerLimit = item.LSL_Val;  // 平均速度下限
                                testData.AverageSpeedUpperLimit = item.USL_Val;  // 平均速度上限
                            }
                            else if (item.ProjectCode == "AverageConcentration")
                            {
                                testData.AverageConcentrationLowerLimit = item.LSL_Val;  // 平均浓度下限
                                testData.AverageConcentrationUpperLimit = item.USL_Val;  // 平均浓度上限
                            }
                        }

                        // 将数据插入数据库
                        var prevRecord = dal.GetLastDataByCode(heavyCode);
                        bool isRework = prevRecord != null;
                        bool ret = dal.addNew(testData);

                        if (ret)
                        {
                            clearHelper.WriteAddCodeOK();
                            UpdateClearUI(testData);
                            LogService.AddLogToEnqueue($"激光清洗->二维码{heavyCode}添加成功，时间:{DateTime.Now}", EnumMsgType.Info);
                            if (isRework)
                            {
                                LogService.AddLogToEnqueue($"激光清洗->条码{heavyCode}为返工，已新增一条记录（使用最新记录进行后续工序）", EnumMsgType.Info);
                            }
                        }
                        else
                        {
                            clearHelper.WriteAddCodeFailed();
                            LogService.AddLogToEnqueue($"激光清洗->二维码{heavyCode}添加失败，原因:数据库插入失败", EnumMsgType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue("激光清洗->读取并新增激光二维码异常:" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                }
            }
        }

        private void ClearHeartbeatAction()
        {
            LogService.AddLogToEnqueue("激光清洗->心跳Start...");
            int failedTimes = 0;
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    if (!clearHelper.IsConnected())
                    {
                        bool result = clearHelper.ConnectPLC();
                        LogService.AddLogToEnqueue($"激光清洗->清洗机PLC重连结果：{result}");
                        continue;
                    }

                    // 交替写入心跳值
                    OperateResult resultWrite = clearHelper.WritePCHeart(1);
                    if (resultWrite.IsSuccess)
                    {
                        // 可选：更新UI显示心跳状态（比如标签闪烁）
                        // 这里可以简化处理
                        ControlHelper.Invoke(lb_heartbeat, delegate
                        {
                            lb_clear_heartbeat.BackColor = Color.Green;
                        });
                        Thread.Sleep(500); // 短暂延时
                        clearHelper.WritePCHeart(0);
                        failedTimes = 0; // 成功则重置失败计数
                    }
                    else
                    {
                        failedTimes++;
                        ControlHelper.Invoke(lb_heartbeat, delegate
                        {
                            lb_clear_heartbeat.BackColor = Color.Red;
                        });
                        LogService.AddLogToEnqueue($"激光清洗->写入清洗机PLC心跳失败: {resultWrite.Message}", EnumMsgType.Error);
                    }

                    if (failedTimes >= 3)
                    {
                        failedTimes = 0;
                        LogService.AddLogToEnqueue("激光清洗->清洗机PLC通信故障，尝试重连...", EnumMsgType.Error);
                        clearHelper.ConnectPLC();
                    }
                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue($"激光清洗->清洗机心跳异常: {ex.Message}", EnumMsgType.Exception);
                }
            }
        }

        /// <summary>
        /// 获取称重2测试数据;
        /// </summary>
        private void PlcHeavyAction2()
        {
            LogService.AddLogToEnqueue("称重2->Start...");
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    OperateResult<short> result = plcHelper.GetReadReady2();
                    if (result.Content == 1)
                    {
                        LogService.AddLogToEnqueue("称重2->-收到称重读取数据信号");
                        OperateResult<string> resultCode = plcHelper.GetHeaverCode2();
                        if (resultCode.Content == "")
                        {
                            LogService.AddLogToEnqueue($"称重2->-读取称重二维码为空", EnumMsgType.Exception);
                            continue;
                        }
                        string heavyCode = resultCode.Content;
                        LogService.AddLogToEnqueue($"称重2->-读取称重二维码为{heavyCode}", EnumMsgType.Info);
                        TestData testData = new TestData();
                        testData = dal.GetLastDataByCode(heavyCode);
                        Thread.Sleep(100);
                        if (testData == null)
                        {
                            LogService.AddLogToEnqueue($"称重2->-称重条码{heavyCode}不存在数据库中", EnumMsgType.Exception);
                            if (!plcHelper.WriteNotCode(1).IsSuccess)
                            {
                                plcHelper.WriteNotCode(1);
                            }
                            while (true)
                            {
                                Thread.Sleep(1000);
                                OperateResult<short> resultNotCodeRest = plcHelper.ResetAbnormal();
                                if (resultNotCodeRest.Content == 1)
                                {
                                    LogService.AddLogToEnqueue($"称重2->-称重条码{heavyCode}不存在数据库异常已复位，继续测试");
                                    break;
                                }
                            }
                            continue;
                        }
                        testData = plcHelper.GetTestData2(testData);
                        if (testData != null)
                        {
                            int retryTime = 0;
                            while (retryTime++ < 3)
                            {
                                if (testData.PreSprayWeight == 0 || testData.PostSprayWeight == 0 || testData.SedimentationWeight == 0
                                    || testData.WeightUpperLimit == 0 || testData.WeightLowerLimit == 0)
                                {
                                    Thread.Sleep(retryTime * 200);
                                    LogService.AddLogToEnqueue($"称重2->-第{retryTime + 1}次读取到0值数据，重读!");
                                    testData = plcHelper.GetTestData2(testData);
                                }
                            }
                            // 与称重1一致，称重2利用率同样依赖Beat。
                            // 若冷喷节拍异常为0，这里会得到Infinity/NaN并被写成0%。
                            double denominator = testData.PowderSupplySpeed / 60.0 * testData.Beat;
                            LogService.AddLogToEnqueue($"称重2->利用率输入 条码:{testData.Code} 沉积重量:{testData.SedimentationWeight:F4} 供粉速度:{testData.PowderSupplySpeed:F4} 节拍:{testData.Beat:F2}s 分母:{denominator:F6}", EnumMsgType.Info);
                            if (denominator <= 0)
                            {
                                testData.UtilizationRate = "0%";
                                LogService.AddLogToEnqueue($"称重2->条码{testData.Code}利用率分母<=0，按0%处理", EnumMsgType.Exception);
                            }
                            else
                            {
                                double utilizationRate = testData.SedimentationWeight / denominator;
                                if (double.IsInfinity(utilizationRate) || double.IsNaN(utilizationRate))
                                {
                                    testData.UtilizationRate = "0%";
                                    LogService.AddLogToEnqueue($"称重2->条码{testData.Code}利用率计算结果非法({utilizationRate})，按0%处理", EnumMsgType.Exception);
                                }
                                else
                                {
                                    testData.UtilizationRate = ((float)utilizationRate * 100).ToString("0.00") + "%";
                                    LogService.AddLogToEnqueue($"称重2->条码{testData.Code}利用率={testData.UtilizationRate}", EnumMsgType.Info);
                                }
                            }
                            LogService.AddLogToEnqueue("称重2->-记录称重完成信号=2");
                            if (!plcHelper.ResetReady2().IsSuccess)
                            {
                                plcHelper.ResetReady2();
                            }

                            if (dal.UpHeavydate(testData))
                            {
                                UpdateUI(testData);
                                this.Invoke(new Action(() => { LoadHis(); }));
                            }
                            else
                            {
                                LogService.AddLogToEnqueue($"称重2->-数据库中不存在{heavyCode}", EnumMsgType.Exception);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue("称重2->-采集称重数据异常:" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                }
            }
        }

        /// <summary>
        /// 判断上下限值
        /// </summary>
        /// <param name="testData"></param>
        private string JudgeUpperLower(double result, double upperLimit, double lowerLimit, string msg)
        {
            if (upperLimit == 0)
            {
                if (result < lowerLimit)
                {
                    LogService.AddLogToEnqueue($"{msg}低于下限{lowerLimit}", EnumMsgType.Error);
                    return "NG";
                }
            }
            else
            {
                if (result > upperLimit)
                {
                    LogService.AddLogToEnqueue($"{msg}高于上限{lowerLimit}", EnumMsgType.Error);
                    return "NG";
                }
                if (result < lowerLimit)
                {
                    LogService.AddLogToEnqueue($"{msg}低于下限{lowerLimit}", EnumMsgType.Error);
                    return "NG";
                }
            }
            return "OK";
        }
        /// <summary>
        /// 报警信息
        /// </summary>
        /// <param name="alarmData"></param>
        private void ShowPLCAlarm(PLCAlarm alarmData)
        {
            if (alarmData != null)
            {
                this.Invoke(new Action(() =>
                {
                    // insert alarm at top of binding list
                    alarmList.Insert(0, alarmData);
                    // keep reasonable size
                    if (alarmList.Count > 500)
                    {
                        alarmList.RemoveAt(alarmList.Count - 1);
                    }
                }));
            }
        }
        /// <summary>
        /// 采集数据
        /// </summary>
        /// <param name="testData"></param>
        private void UpdateUI(TestData testData)
        {
            if (testData != null)
            {
                this.Invoke(new Action(() =>
                {
                    // Update realtime binding list (prevent duplicate Code rows)
                    if (string.IsNullOrEmpty(testData.Code)) return;
                    if (realtimeDict.TryGetValue(testData.Code, out var existing))
                    {
                        // update fields on existing object
                        existing.PreSprayWeight = testData.PreSprayWeight;
                        existing.PreSprayWeight_1 = testData.PreSprayWeight_1;
                        existing.PreSprayWeight_1_5 = testData.PreSprayWeight_1_5;
                        existing.PreSprayWeight_2 = testData.PreSprayWeight_2;
                        existing.PreSprayWeight_2_5 = testData.PreSprayWeight_2_5;
                        existing.PostSprayWeight = testData.PostSprayWeight;
                        existing.PostSprayWeight_1 = testData.PostSprayWeight_1;
                        existing.PostSprayWeight_1_5 = testData.PostSprayWeight_1_5;
                        existing.PostSprayWeight_2 = testData.PostSprayWeight_2;
                        existing.PostSprayWeight_2_5 = testData.PostSprayWeight_2_5;
                        existing.SedimentationWeight = testData.SedimentationWeight;
                        existing.WeightUpperLimit = testData.WeightUpperLimit;
                        existing.WeightLowerLimit = testData.WeightLowerLimit;
                        existing.WeightResult = testData.WeightResult;
                        existing.AddTime = testData.AddTime;
                        existing.UtilizationRate = testData.UtilizationRate;
                        existing.AverageTemperature = testData.AverageTemperature;
                        existing.AverageNitrogenPressure = testData.AverageNitrogenPressure;
                        existing.MinNitrogenPressure = testData.MinNitrogenPressure;
                        existing.PowderSupplySpeed = testData.PowderSupplySpeed;
                        existing.StartTime = testData.StartTime;
                        existing.EndTime = testData.EndTime;
                        existing.Beat = testData.Beat;
                        existing.PlacementTime = testData.PlacementTime;
                        existing.IntakePressure = testData.IntakePressure;
                        existing.IntakeFlow = testData.IntakeFlow;
                        existing.IntakePressureResult = testData.IntakePressureResult;
                        existing.UtilizationRate = testData.UtilizationRate;
                        int idx = realtimeList.IndexOf(existing);
                        if (idx >= 0) realtimeList.ResetItem(idx);
                        // update row color if needed
                        if (idx >= 0)
                        {
                            if (existing.WeightResult == "NG" || existing.TemperatureResult == "NG" || existing.NitrogenPressureResult == "NG" || existing.IntakePressureResult == "NG" || existing.SpeedResult == "NG" || existing.ConcentrationResult == "NG")
                            {
                                dgv_realtime.Rows[idx].DefaultCellStyle.BackColor = Color.LightPink;
                            }
                            else
                            {
                                dgv_realtime.Rows[idx].DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }
                    else
                    {
                        // insert new item at top
                        realtimeList.Insert(0, testData);
                        realtimeDict[testData.Code] = testData;
                        // apply highlight for newest row
                        if (dgv_realtime.Rows.Count > 0)
                        {
                            if (testData.WeightResult == "NG" || testData.TemperatureResult == "NG" || testData.NitrogenPressureResult == "NG" || testData.IntakePressureResult == "NG" || testData.SpeedResult == "NG" || testData.ConcentrationResult == "NG")
                            {
                                dgv_realtime.Rows[0].DefaultCellStyle.BackColor = Color.LightPink;
                            }
                        }
                        // trim list if too long
                        if (realtimeList.Count > realtimeMaxCount)
                        {
                            var last = realtimeList[realtimeList.Count - 1];
                            try { realtimeDict.Remove(last.Code); } catch { }
                            realtimeList.RemoveAt(realtimeList.Count - 1);
                        }
                    }
                }));
                //int Productresult = testData.Result;

                ///清除大于250的数据;
                for (int i = 0; i < PubData.TestProjectItems.Count; i++)
                {
                    if (PubData.TestProjectItems[i].Datas.Count > 250)
                    {
                        PubData.TestProjectItems[i].Datas.Remove(0);
                    }
                }

                //PubData.TestProjectItems[0].Datas.Add(testData.PreSprayWeight);
                //PubData.TestProjectItems[1].Datas.Add(testData.PostSprayWeight);
                //PubData.TestProjectItems[2].Datas.Add(testData.SedimentationWeight);
                //PubData.TestProjectItems[3].Datas.Add(testData.UpperLimit);
                // PubData.TestProjectItems[4].Datas.Add(testData.LowerLimit);
                //PubData.TestProjectItems[5].Datas.Add(testData.Result);
                //PubData.TestProjectItems[6].Datas.Add(testData.AirtighTestTime);
                //PubData.TestProjectItems[7].Datas.Add(testData.AirtighTestPressure);
                //PubData.TestProjectItems[8].Datas.Add(testData.AirtighTestLeakageValue);
                // UpdateCpk();
            }

            TestCount++;
        }
        private void UpdateClearUI(TestData testData)
        {
            if (testData != null)
            {
                this.Invoke(new Action(() =>
                {
                    dgv_clearrealtime.Rows.Insert(0, new object[] {
                        1,
                        testData.Code,
                        testData.PlacementTime
                    });

                    for (int i = 0; i < dgv_clearrealtime.Rows.Count; i++)
                    {
                        dgv_clearrealtime.Rows[i].Cells[0].Value = i + 1;
                    }

                }));
                //int Productresult = testData.Result;

                ///清除大于250的数据;
                for (int i = 0; i < PubData.TestProjectItems.Count; i++)
                {
                    if (PubData.TestProjectItems[i].Datas.Count > 250)
                    {
                        PubData.TestProjectItems[i].Datas.Remove(0);
                    }
                }
            }
        }
        /// <summary>
        /// 生成数据
        /// </summary>
        private void UpdateCpk()
        {
            try
            {
                for (int i = 0; i < PubData.TestProjectItems.Count; i++)
                {
                    float usl = PubData.TestProjectItems[i].USL_Val;
                    float lsl = PubData.TestProjectItems[i].LSL_Val;
                    var datas = PubData.TestProjectItems[i].Datas;

                    float min = spcHelper.Min(datas.ToArray());
                    float agv = spcHelper.Avage(datas.ToArray());
                    float max = spcHelper.Max(datas.ToArray());
                    float stdev = spcHelper.StDev(datas.ToArray());
                    float cp = spcHelper.GetCp(usl, lsl, stdev);
                    float cpl = spcHelper.CpkL(lsl, agv, stdev);
                    float cpu = spcHelper.CpkU(usl, agv, stdev);
                    float cpk = spcHelper.getCPK(datas.ToArray(), usl, lsl);
                }
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue("UpdateCpk：" + ex.Message, EnumMsgType.Exception);
            }
        }

        /// <summary>
        /// 初始化网格Spc
        /// </summary>
        private void InitDataGridSpc()
        {
            try
            {
                List<E_TestProject> list = testProjectDal.GetList();
                for (int i = 0; i < list.Count; i++)
                {
                    PubData.TestProjectItems.Add(new E_TestProject()
                    {
                        LSL_Val = list[i].LSL_Val,
                        USL_Val = list[i].USL_Val,
                        ProjectCode = list[i].ProjectCode,
                        ProjectName = list[i].ProjectName,
                        Datas = new List<float>()
                    });
                }
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue($"{ex.Message}");
            }
        }
        /// <summary>
        /// 保存为.csv文件
        /// </summary>
        /// <param name="code"></param>
        private void SaveCurrentDataToCsv(string code)
        {
            try
            {
                lock (_fileLock)
                {
                    // 新增保存 CSV 的代码
                    string url = "D:\\Temp";
                    // 过滤 code 中的非法字符
                    string validCode = Regex.Replace(code, @"[\\/:*?""<>|]", "");
                    string filePath = $@"{url}\{DateTime.Now:yyyyMM}\{DateTime.Now:yyyyMMdd}\{validCode}\{validCode}.csv";
                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string tempFilePath = filePath + ".tmp";

                    bool fileExists = File.Exists(filePath);
                    List<string> fileLines = fileExists ? File.ReadAllLines(filePath).ToList() : new List<string>();
                    // 准备新数据行
                    string newDataLine = $"{_samlingData.ID}," +
                                       $"{_samlingData.Time}," +
                                       $"{_samlingData.temperature}," +
                                       $"{_samlingData.pressure}," +
                                       $"{_samlingData.speed}," +
                                       $"{_samlingData.concentration}," +
                                       $"{_samlingData.position}";
                    // 使用临时文件写入
                    using (StreamWriter sw = new StreamWriter(tempFilePath, false, Encoding.UTF8))
                    {
                        // 写入标题行（如果文件不存在或为空）
                        if (fileLines.Count == 0)
                        {
                            string header = "ID,Time,Temperature,Pressure,Speed,Concentration,Position";
                            sw.WriteLine(header);
                        }
                        // 追加新行
                        fileLines.Add(newDataLine);

                        // 写入所有行
                        foreach (var line in fileLines)
                        {
                            sw.WriteLine(line);
                        }
                    }

                    // 替换原文件
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    File.Move(tempFilePath, filePath);
                }
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue("保存文件错误" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 分别保存 gun（温度、压力）和 monitor（速度、浓度、位置）两张图片
        /// </summary>
        public void GenerateScatterPlotFromLinkedList(
            List<DateTime> timesList,
            List<double> temperatureList,
            List<double> pressureList,
            List<double> speedList,
            List<double> concentrationList,
            List<double> positionList,
            string code)
        {
            // 1. 确保所有列表长度一致
            int minLength = new[]
            {
            timesList.Count,
            temperatureList.Count,
            pressureList.Count,
            speedList.Count,
            concentrationList.Count,
            positionList.Count
            }.Min();

            // 2. 截取所有列表到相同长度
            timesList = timesList.Take(minLength).ToList();
            temperatureList = temperatureList.Take(minLength).ToList();
            pressureList = pressureList.Take(minLength).ToList();
            speedList = speedList.Take(minLength).ToList();
            concentrationList = concentrationList.Take(minLength).ToList();
            positionList = positionList.Take(minLength).ToList();

            // 3. 转换时间数据
            double[] timeData = timesList.Select(t => t.ToOADate()).ToArray();

            //生成 gun 图片（速度，温度、压力）
            var gunPlot = new Plot(1000, 600);

            // 添加温度数据
            var tempScatter = gunPlot.AddScatterPoints(timeData, temperatureList.ToArray());
            tempScatter.Label = "Temperature";
            tempScatter.Color = System.Drawing.Color.Blue;
            tempScatter.MarkerSize = 7;
            tempScatter.LineStyle = ScottPlot.LineStyle.None;

            // 添加压力数据
            var pressureScatter = gunPlot.AddScatterPoints(timeData, pressureList.ToArray());
            pressureScatter.Label = "Pressure";
            pressureScatter.Color = System.Drawing.Color.Orange;
            pressureScatter.MarkerSize = 7;
            pressureScatter.LineStyle = ScottPlot.LineStyle.None;

            // 添加速度数据
            var speedScatter = gunPlot.AddScatterPoints(timeData, speedList.ToArray());
            speedScatter.Label = "Speed";
            speedScatter.Color = System.Drawing.Color.Gray;
            speedScatter.MarkerSize = 7;
            speedScatter.LineStyle = ScottPlot.LineStyle.None;

            // 设置 gun 图表样式
            gunPlot.Title("(Temperature,Pressure,Speed)");
            gunPlot.XLabel("Time");
            gunPlot.YLabel("Values");
            gunPlot.XAxis.DateTimeFormat(true);
            gunPlot.Legend(location: Alignment.UpperRight);
            gunPlot.AxisAuto();

            // 保存 gun 图片
            SavePlot(gunPlot, code, "gun");

            // 生成 monitor 图片（浓度、位置）
            var monitorPlot = new Plot(1000, 600);
            // 添加浓度数据
            var concScatter = monitorPlot.AddScatterPoints(timeData, concentrationList.ToArray());
            concScatter.Label = "Concentration";
            concScatter.Color = System.Drawing.Color.Blue;
            concScatter.MarkerSize = 7;
            concScatter.LineStyle = ScottPlot.LineStyle.None;

            // 添加位置数据
            var positionScatter = monitorPlot.AddScatterPoints(timeData, positionList.ToArray());
            positionScatter.Label = "Position";
            positionScatter.Color = System.Drawing.Color.Orange;
            positionScatter.MarkerSize = 7;
            positionScatter.LineStyle = ScottPlot.LineStyle.None;

            // 设置 monitor 图表样式
            monitorPlot.Title("Concentration, Position");
            monitorPlot.XLabel("Time");
            monitorPlot.YLabel("Values");
            monitorPlot.XAxis.DateTimeFormat(true);
            monitorPlot.Legend(location: Alignment.UpperRight);
            monitorPlot.AxisAuto();

            // 保存 monitor 图片
            SavePlot(monitorPlot, code, "monitor");
        }

        /// <summary>
        /// 保存图表到文件
        /// </summary>
        private void SavePlot(Plot plot, string code, string plotType)
        {
            string url = "D:\\Temp";
            string validCode = Regex.Replace(code, @"[\\/:*?""<>|]", "");
            string filePath = $@"{url}\{DateTime.Now:yyyyMM}\{DateTime.Now:yyyyMMdd}\{validCode}\[{plotType}].png";

            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // 保存图片
            plot.SaveFig(filePath);
        }
        #region 显示日志
        /// <summary>
        /// 显示日志
        /// </summary>
        private void ShowLog()
        {
            while (true)
            {
                Thread.Sleep(100);
                CacheLog log = LogService.GetLog();//取出一条日志
                if (log != null)
                {
                    int msgType = log.MsgType;
                    ControlHelper.Invoke(dgv_logs, () =>
                    {
                        string msgTypeStr = ((EnumMsgType)msgType).GetDescription();
                        object[] info = new object[]
                        {
                            CommonUtil.GetCurrentTime(),
                            msgTypeStr,
                            log.Message
                        };
                        //写入数据展示控件
                        if (log.IsShow)
                        {
                            dgv_logs.Rows.Insert(0, info);
                            if (msgType == (int)EnumMsgType.Error || msgType == (int)EnumMsgType.Exception)
                            {
                                dgv_logs.Rows[0].DefaultCellStyle.BackColor = Color.Pink;
                            }
                            else if (msgType == (int)EnumMsgType.Warning)
                            {
                                dgv_logs.Rows[0].DefaultCellStyle.BackColor = Color.LightYellow;
                            }
                        }
                        //写入文件
                        LogHelper.WriteLog($"[{msgTypeStr}]{log.Message}");
                    });
                }
            }
        }
        #endregion

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确认要退出系统吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                plcHelper.WritePCHeart(0);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 用户设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButtonUser_Click(object sender, EventArgs e)
        {
            if (PubInfo.UserType == 1)
            {
                FrmUser frmUser = new FrmUser();
                frmUser.ShowDialog();
            }
            else
            {
                FrmLogin frmLogin = new FrmLogin();
                DialogResult dr = frmLogin.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    lb_userName.Text = PubInfo.UserName;
                    lb_right.Text = PubInfo.UserTypeStr;
                }
            }

        }

        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uibtn_log_Click(object sender, EventArgs e)
        {
            FrmLog frmLog = new FrmLog();
            frmLog.ShowDialog();
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_params_Click(object sender, EventArgs e)
        {
            //管理员 或 工艺
            if (PubInfo.UserType == 1 || PubInfo.UserType == 2)
            {
                FrmParams frmParams = new FrmParams();
                frmParams.ShowDialog();
            }
            else
            {
                FrmLogin frmLogin = new FrmLogin();
                DialogResult dr = frmLogin.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    lb_userName.Text = PubInfo.UserName;
                    lb_right.Text = PubInfo.UserTypeStr;
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            FrmTest frmTest = new FrmTest();
            frmTest.ShowDialog();
        }

        #region 历史数据

        private void LoadHis()
        {
            dgv_his.AutoGenerateColumns = false;
            string code = uitb_Code.Text.Trim();
            TestDataQuery param = new TestDataQuery()
            {
                Code = code,
                PageNum = pageData.ActivePage,
                PageSize = pageData.PageSize,
                StartTime = uidtp_start.Text,
                EndTime = uidtp_end.Text
            };
            List<TestData> list = dal.GetList(param);
            int count = dal.GetCount(param);
            pageData.TotalCount = count;
            dgv_his.DataSource = list;
        }
        /// <summary>
        /// 查询历史数据;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiBtnSearch_Click(object sender, EventArgs e)
        {
            LoadHis();
        }

        private void pageData_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            LoadHis();
        }
        #endregion

        private void dgv_spc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uisBtn_Export_Click(object sender, EventArgs e)
        {
            string code = uitb_Code.Text.Trim();
            TestDataQuery param = new TestDataQuery()
            {
                Code = code,
                StartTime = uidtp_start.Text,
                EndTime = uidtp_end.Text
            };

            //开始导出;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel文件(*.xlsx)|*.xlsx";
            sfd.Title = "导出数据";
            sfd.AddExtension = true;
            sfd.DefaultExt = "*.xlsx";
            if (DialogResult.OK == sfd.ShowDialog())
            {
                List<TestData> list = dal.GetExportData(param);

                // 创建 Excel 工作簿
                IWorkbook workbook = new XSSFWorkbook();  // xlsx 格式
                ISheet sheet = workbook.CreateSheet("测试数据");

                // 获取属性列表，并过滤掉被[ExcelIgnore]标记的属性
                var properties = typeof(TestData).GetProperties()
                    .Where(p => !p.IsDefined(typeof(ExcelIgnoreAttribute), false))
                    .ToList();

                // 创建表头行
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < properties.Count; i++)
                {
                    // 获取ExcelColumnName特性值，如果没有则使用属性名
                    var excelColumnNameAttr = properties[i].GetCustomAttribute<ExcelColumnNameAttribute>();
                    string headerText = excelColumnNameAttr?.ExcelColumnName ?? properties[i].Name;

                    headerRow.CreateCell(i).SetCellValue(headerText);
                }

                // 写入数据行
                for (int rowIdx = 0; rowIdx < list.Count; rowIdx++)
                {
                    TestData data = list[rowIdx];
                    IRow dataRow = sheet.CreateRow(rowIdx + 1);  // 从第2行开始

                    for (int colIdx = 0; colIdx < properties.Count; colIdx++)
                    {
                        ICell cell = dataRow.CreateCell(colIdx);
                        object value = properties[colIdx].GetValue(data);
                        cell.SetCellValue(value?.ToString() ?? "");

                        // 如果 Result 是 "NG"，整行标红
                        if (data.WeightResult == "NG" || data.MinTemperatureResult == "NG" ||
                            data.MinNitrogenPressureResult == "NG" || data.MinSpeedResult == "NG" ||
                            data.IntakePressureResult == "NG")
                        {
                            ICellStyle style = workbook.CreateCellStyle();
                            IFont font = workbook.CreateFont();
                            font.Color = IndexedColors.Red.Index;  // 红色字体
                            font.IsBold = true;                    // 加粗
                            style.SetFont(font);
                            cell.CellStyle = style;
                        }
                    }
                }

                // 自动调整列宽
                for (int i = 0; i < properties.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                // 保存 Excel 文件
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                {
                    workbook.Write(fs);
                }

                string info = $"导出数据成功，文件路径：{sfd.FileName}";
                this.ShowMessageDialog(info, "导出结果", false, UIStyle.Blue);
            }
        }

        /// <summary>
        /// 格式化显示：
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_his_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgv_his.Columns[e.ColumnIndex].Name == "col_his_AirtighTestLeakageValue")
            {
                if (e.Value.ToString() == "0")
                {
                    e.Value = "---";
                }
            }

        }

        private void dgv_his_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgv_his.Rows.Count)
            {
                return;
            }

            string[] checkColumns = {
                "col_his_WeightResult",
                "dgv_his_TemperatureResult",
                "dgv_his_NitrogenPressureResult",
                "dgv_his_IntakePressureResult",
                "dgv_his_SpeedResult",
                "dgv_his_ConcentrationResult"
            };

            foreach (string columnName in checkColumns)
            {
                var cell = dgv_his.Rows[e.RowIndex].Cells[columnName];
                if (cell.Value?.ToString() == "NG")
                {
                    dgv_his.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                    return; // 找到一个NG就设置颜色并退出
                }
            }
        }

        private void uiBtn_Exit_Click(object sender, EventArgs e)
        {
            PubInfo.UserName = "tester";
            PubInfo.UserType = 3;
            MessageBox.Show("已退出登录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void uiLabel11_Click(object sender, EventArgs e)
        {

        }

        private void uitb_Code_TextChanged(object sender, EventArgs e)
        {

        }
        private void uitb_ClearCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string code = uitb_ClearCode.Text.Trim();
                try
                {
                    TestData testData = new TestData()
                    {
                        Code = code,
                        PlacementTime = DateTime.Now,
                    };
                    if (list == null)
                    {
                        list = projectdal.GetList();
                    }
                    foreach (var item in list)
                    {
                        if (item.ProjectCode == "MinTemperature")
                        {
                            testData.MinTemperatureLowerLimit = item.LSL_Val;
                        }
                        else if (item.ProjectCode == "AverageTemperature")
                        {
                            testData.AverageTemperatureLowerLimit = item.LSL_Val;
                            testData.AverageTemperatureUpperLimit = item.USL_Val;
                        }
                        else if (item.ProjectCode == "MinNitrogenPressure")
                        {
                            testData.MinNitrogenPressureLowerLimit = item.LSL_Val;
                        }
                        else if (item.ProjectCode == "AverageNitrogenPressure")
                        {
                            testData.AverageNitrogenPressureLowerLimit = item.LSL_Val;
                            testData.AverageNitrogenPressureUpperLimit = item.USL_Val;
                        }
                        else if (item.ProjectCode == "IntakePressure")
                        {
                            testData.IntakePressureLowerLimit = item.LSL_Val;
                        }
                        else if (item.ProjectCode == "MinSpeed")
                        {
                            testData.MinSpeedLowerLimit = item.LSL_Val;
                        }
                        else if (item.ProjectCode == "AverageSpeed")
                        {
                            testData.AverageSpeedLowerLimit = item.LSL_Val;
                            testData.AverageSpeedUpperLimit = item.USL_Val;
                        }
                        else if (item.ProjectCode == "AverageConcentration")
                        {
                            testData.AverageConcentrationLowerLimit = item.LSL_Val;
                            testData.AverageConcentrationUpperLimit = item.USL_Val;
                        }

                    }
                    bool ret = dal.addNew(testData);
                    if (!ret)
                    {
                        MessageBox.Show($"二维码{code}添加数据库失败");
                        return;
                    }
                    UpdateClearUI(testData);
                    uitb_ClearCode.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"二维码{code}添加数据库出错" + ex.Message);
                }
            }
        }

        private void LoadAlarm()
        {
            dgv_alarmmessage.AutoGenerateColumns = false;
            TestDataQuery param = new TestDataQuery()
            {
                PageNum = uiPagAlarm.ActivePage,
                PageSize = uiPagAlarm.PageSize,
                StartTime = ui_time.Text,
            };
            List<PLCAlarm> list = pLCAlarmDal.GetList(param);
            int count = pLCAlarmDal.GetCount(param);
            pageData.TotalCount = count;
            // update binding list instead of replacing DataSource
            alarmList.Clear();
            foreach (var a in list)
            {
                alarmList.Add(a);
            }
        }

        private void btn_AlarmSelect_Click(object sender, EventArgs e)
        {
            LoadAlarm();
        }

        private void WritePLCTemperature()
        {
            LogService.AddLogToEnqueue("对比1->Start...");
            TestData compareData = new TestData();
            float[] data = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    OperateResult<short> resultCompare = plcHelper.ReadCompare();//read from D5676
                    if (resultCompare.Content == 1)
                    {
                        LogService.AddLogToEnqueue("对比1->收到冷喷比对信号");
                        OperateResult<string> resultCompareCode = plcHelper.GetCompareCode();
                        if (resultCompareCode.Content == "")
                        {
                            LogService.AddLogToEnqueue($"对比1->读取冷喷比对二维码为{resultCompareCode.Content}");
                        }
                        else
                        {
                            string compareCode = resultCompareCode.Content.Trim();
                            compareData = dal.GetLastDataByCode(compareCode);
                            if (compareData == null)
                            {
                                LogService.AddLogToEnqueue($"对比1->条码{compareCode}没有录入数据库");
                                if (!plcHelper.WriteOverTime(3).IsSuccess)
                                {
                                    plcHelper.WriteOverTime(3);
                                }

                            }
                            else
                            {
                                IsOverTime(compareData, compareCode);
                            }
                        }

                    }
                    data = gunHelp.ReadGun();
                    _samlingData.temperature = Math.Round(data[2], 2);
                    _samlingData.pressure = Math.Round(data[3], 2);
                    plcHelper.WriteTemperature(data[2]);
                    plcHelper.WritePressure(data[3]);
                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue($"对比1->写入PLC温度和压力异常{ex.Message}{ex.StackTrace}", EnumMsgType.Exception);
                }
            }
        }

        private void WritePLCTemperature2()
        {
            LogService.AddLogToEnqueue("对比2->Start...");
            TestData compareData = new TestData();
            float[] data = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    OperateResult<short> resultCompare = plcHelper.ReadCompare2();//read from D5698
                    if (resultCompare.Content == 1)
                    {
                        LogService.AddLogToEnqueue("对比2->收到冷喷比对信号");
                        OperateResult<string> resultCompareCode = plcHelper.GetCompareCode2();
                        if (resultCompareCode.Content == "")
                        {
                            LogService.AddLogToEnqueue($"对比2->读取冷喷比对二维码为{resultCompareCode.Content}");
                        }
                        else
                        {
                            string compareCode = resultCompareCode.Content.Trim();
                            compareData = dal.GetLastDataByCode(compareCode);
                            if (compareData == null)
                            {
                                LogService.AddLogToEnqueue($"对比2->条码{compareCode}没有录入数据库");
                                if (!plcHelper.WriteOverTime(3).IsSuccess)
                                {
                                    plcHelper.WriteOverTime(3);
                                }

                            }
                            else
                            {
                                IsOverTime(compareData, compareCode);
                            }
                        }

                    }
                    data = gunHelp.ReadGun();
                    _samlingData.temperature = Math.Round(data[2], 2);
                    _samlingData.pressure = Math.Round(data[3], 2);
                    plcHelper.WriteTemperature(data[2]);
                    plcHelper.WritePressure(data[3]);
                }
                catch (Exception ex)
                {
                    LogService.AddLogToEnqueue($"对比2->写入PLC温度和压力异常{ex.Message}{ex.StackTrace}", EnumMsgType.Exception);
                }
            }
        }
    }
}
