using Opc.Ua.Client;
using Opc.Ua;
using Prism.Mvvm;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using System.Threading.Tasks;
using PR_Spc_Tester.Services;
using PR_Model;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using ScottPlot.Drawing.Colormaps;
using System.Windows.Markup;
using System.Text;
using System.Text.RegularExpressions;

namespace PR_Spc_Tester.Moudules
{
    // 该类用于与 OPC UA 服务器建立连接，监控特定节点的数据，并进行数据处理和存储
    public class HiWatchOpcMonitor : IDisposable
    {
        // OPC UA 会话对象，用于与服务器进行通信
        private Session _session;
        // 订阅对象，用于订阅特定节点的数据变化
        private Subscription _subscription;
        // 并发队列，用于存储粒子速度数据
        private readonly ConcurrentQueue<double> _speedData = new ConcurrentQueue<double>();
        // 并发队列，用于存储粒子密度数据
        private readonly ConcurrentQueue<double> _densityData = new ConcurrentQueue<double>();
        // 并发队列，用于存储粒子位置数据
        private readonly ConcurrentQueue<double> _positionData = new ConcurrentQueue<double>();
        // 包含连接和订阅的相关配置
        private readonly OpcMonitorSettings _settings;
        // 日志记录器，用于记录操作过程中的日志信息
        private readonly ILogger _logger;
        // 表示当前是否已连接到 OPC UA 服务器
        private bool _isConnected;
        // 用于控制重连操作的取消
        private CancellationTokenSource _reconnectCts;
        private const string CommandNodeId = "ns=2;s=Sensor_00.OPERATIONS.COMMAND";
        private const string CameraStateNodeId = "ns=2;s=Sensor_00.SensorStatus.CameraState";
        private const string SessionCurrentLengthNodeId = "ns=2;s=Sensor_00.SessionData.CurrentLength";
        private const string SessionLastOutputPathNodeId = "ns=2;s=Sensor_00.SessionData.LastOutputPath";
        // 队列中最多可存储的数据点数
        private const int MaxDataPoints = 1000;
        private readonly object _dataLock = new object(); // 数据缓存锁
        private double? _latestSpeed; // 最新速度数据（可空类型，标识是否已更新）
        private double? _latestDensity; // 最新密度数据
        private double? _latestPosition; // 最新位置数据
        private DateTime _latestTimestamp; // 数据时间戳（取三者中最新的或一致的）
                                           // 定义事件：当三种数据均采集到后触发
        public event EventHandler<AllDataEventArgs> OnAllDataReceived;
        // 当接收到粒子速度数据时触发的事件
        public event EventHandler<OpcValueEventArgs> OnSpeedReceived;
        // 当接收到粒子密度数据时触发的事件
        public event EventHandler<OpcValueEventArgs> OnDensityReceived;
        // 当接收到粒子位置数据时触发的事件
        public event EventHandler<OpcValueEventArgs> OnPositionReceived;
        // 当接收到 SizeDV50 数据时触发的事件
        public event EventHandler<OpcValueEventArgs> OnSizeDV50Received;
        // 当接收到 SizeDV90 数据时触发的事件
        public event EventHandler<OpcValueEventArgs> OnSizeDV90Received;
        // 当接收到报警字符串时触发的事件
        public event EventHandler<OpcStringEventArgs> OnAlertMessageReceived;

        // 构造函数，接收 OPC 监控设置，若未提供则使用默认设置
        public HiWatchOpcMonitor(OpcMonitorSettings settings = null)
        {
            // 如果 settings 为 null，则使用默认的 OpcMonitorSettings 实例
            _settings = settings ?? new OpcMonitorSettings();
        }

        // 异步方法，用于连接到指定的 OPC UA 服务器端点
        public async Task ConnectAsync(string endpointUrl, CancellationToken cancellationToken = default)
        {
            // 创建一个可取消的令牌源，用于控制重连操作
            _reconnectCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // 循环尝试连接，直到连接成功或取消操作
            while (!_isConnected && !_reconnectCts.IsCancellationRequested)
            {
                try
                {
                    // 创建应用程序配置对象
                    var config = new ApplicationConfiguration
                    {
                        // 应用程序名称
                        ApplicationName = "OPCTemplate",
                        // 应用程序类型为客户端
                        ApplicationType = ApplicationType.Client,
                        // 安全配置
                        SecurityConfiguration = new SecurityConfiguration(),
                        // 应用程序 URI
                        ApplicationUri = "urn:localhost:OPCUA:OPCTemplate",
                        // 客户端配置
                        ClientConfiguration = new ClientConfiguration()
                    };
                    // 验证应用程序配置
                    try
                    {
                        await config.Validate(ApplicationType.Client);
                    }
                    catch (Exception ex)
                    {
                        LogService.AddLogToEnqueue("OPC配置校验跳过：" + ex.Message, EnumMsgType.Exception);
                    }

                    // 选择合适的端点描述
                    var endpointDescription = CoreClientUtils.SelectEndpoint(config, endpointUrl, false);
                    // 创建端点配置
                    var endpointConfiguration = EndpointConfiguration.Create(config);
                    // 设置操作超时时间
                    endpointConfiguration.OperationTimeout = _settings.OperationTimeout;
                    // 创建配置好的端点
                    var endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

                    // 创建会话
                    _session = await Session.Create(
                        config,
                        endpoint,
                        false,
                        "MY OPC UA Client",
                        60000,
                        new UserIdentity(new AnonymousIdentityToken()),
                        null);

                    // 设置返回的诊断信息掩码
                    _session.ReturnDiagnostics = DiagnosticsMasks.All;

                    // 创建订阅对象
                    _subscription = new Subscription(_session.DefaultSubscription)
                    {
                        // 发布间隔
                        PublishingInterval = _settings.PublishingInterval,
                        // 保持活动计数
                        KeepAliveCount = _settings.KeepAliveCount,
                        // 生命周期计数
                        LifetimeCount = _settings.LifetimeCount,
                        // 启用发布
                        PublishingEnabled = true,
                        // 每次发布的最大通知数
                        MaxNotificationsPerPublish = 1000,
                        // 优先级
                        Priority = 1,
                        // 当前发布间隔
                        CurrentPublishingInterval = _settings.PublishingInterval
                    };

                    // 设置监控项
                    SetupMonitoredItems();

                    // 将订阅添加到会话中
                    _session.AddSubscription(_subscription);
                    // 异步创建订阅
                    await _subscription.CreateAsync(cancellationToken);

                    // 标记为已连接
                    _isConnected = true;
                    // 记录连接成功的日志
                    LogService.AddLogToEnqueue("OPC UA连接成功");
                }
                catch (Exception ex)
                {
                    // 标记为未连接
                    _isConnected = false;
                    // 记录连接失败的日志
                    LogService.AddLogToEnqueue("OPC UA连接失败" + ex.Message + ex.StackTrace);
                    // 等待 5 秒后重试
                    await Task.Delay(5000, cancellationToken);
                }
            }
        }

        // 设置要监控的 OPC UA 节点
        private void SetupMonitoredItems()
        {
            // 创建一个监控粒子速度的监控项
            var speedItem = new MonitoredItem(_subscription.DefaultItem)
            {
                // 起始节点 ID
                StartNodeId = "ns=2;s=Sensor_00.CurrentMean.ParticleSpeed",
                // 要监控的属性 ID
                AttributeId = Attributes.Value,
                // 显示名称
                DisplayName = "ParticleSpeed",
                // 节点类为变量
                NodeClass = NodeClass.Variable,
                // 采样间隔
                SamplingInterval = _settings.SamplingInterval,
                // 队列大小
                QueueSize = _settings.QueueSize,
                // 丢弃最旧的数据
                DiscardOldest = true,
                //死区值用来过滤微小的数据变化，避免频繁通知
                Filter = new DataChangeFilter // 触发条件
                {
                    //Trigger = DataChangeTrigger.StatusValue, // 值或状态变化才通知
                    Trigger = DataChangeTrigger.StatusValueTimestamp, // 值、状态或时间戳变化才通知
                    DeadbandType = (uint)DeadbandType.None, // 不设死区
                    DeadbandValue = 0 // 死区值
                }
            };

            // 为速度监控项添加通知事件处理程序
            speedItem.Notification += (item, args) =>
            {
                // 遍历接收到的值
                foreach (var value in item.DequeueValues())
                {
                    // 将值转换为 double 类型
                    var speed = Convert.ToDouble(value.Value);
                    // 将速度数据添加到队列中
                    if (speed != 0)
                    {
                        AddSpeedData(speed);
                    }

                    // 触发速度数据接收事件
                    OnSpeedReceived?.Invoke(this, new OpcValueEventArgs(speed));
                    lock (_dataLock)
                    {
                        _latestSpeed = speed;
                        _latestTimestamp = value.SourceTimestamp; // 使用OPC服务器的时间戳（更准确）
                        CheckAndTriggerAllDataEvent(); // 检查是否触发聚合事件
                    }
                }
            };

            // 创建一个监控粒子密度的监控项
            var densityItem = new MonitoredItem(_subscription.DefaultItem)
            {
                // 起始节点 ID
                StartNodeId = "ns=2;s=Sensor_00.CurrentMean.ParticleDensity",
                // 要监控的属性 ID
                AttributeId = Attributes.Value,
                // 显示名称
                DisplayName = "ParticleDensity",
                // 节点类为变量
                NodeClass = NodeClass.Variable,
                // 采样间隔
                SamplingInterval = _settings.SamplingInterval,
                // 队列大小
                QueueSize = _settings.QueueSize,
                // 丢弃最旧的数据
                DiscardOldest = true,
                //死区值用来过滤微小的数据变化，避免频繁通知
                Filter = new DataChangeFilter // 触发条件
                {
                    //Trigger = DataChangeTrigger.StatusValue, // 值或状态变化才通知
                    Trigger = DataChangeTrigger.StatusValueTimestamp, // 值、状态或时间戳变化才通知
                    DeadbandType = (uint)DeadbandType.None, // 不设死区
                    DeadbandValue = 0 // 死区值
                }
            };

            // 为密度监控项添加通知事件处理程序
            densityItem.Notification += (item, args) =>
            {
                // 遍历接收到的值
                foreach (var value in item.DequeueValues())
                {
                    // 将值转换为 double 类型
                    var density = Convert.ToDouble(value.Value);
                    // 将密度数据添加到队列中
                    if (density != 0)
                    {
                        AddDensityData(density);
                    }
                    // 触发密度数据接收事件
                    OnDensityReceived?.Invoke(this, new OpcValueEventArgs(density));
                    lock (_dataLock)
                    {
                        _latestDensity = density;
                        _latestTimestamp = value.SourceTimestamp;
                        CheckAndTriggerAllDataEvent();
                    }
                }
            };

            // 创建一个监控粒子位置的监控项
            var positionItem = new MonitoredItem(_subscription.DefaultItem)
            {
                // 起始节点 ID
                StartNodeId = "ns=2;s=Sensor_00.CurrentMean.SprayPos",
                // 要监控的属性 ID
                AttributeId = Attributes.Value,
                // 显示名称
                DisplayName = "SprayPos",
                // 节点类为变量
                NodeClass = NodeClass.Variable,
                // 采样间隔
                SamplingInterval = _settings.SamplingInterval,
                // 队列大小
                QueueSize = _settings.QueueSize,
                // 丢弃最旧的数据
                DiscardOldest = true,
                //死区值用来过滤微小的数据变化，避免频繁通知
                Filter = new DataChangeFilter // 触发条件
                {
                    //Trigger = DataChangeTrigger.StatusValue, // 值或状态变化才通知
                    Trigger = DataChangeTrigger.StatusValueTimestamp, // 值、状态或时间戳变化才通知
                    DeadbandType = (uint)DeadbandType.None, // 不设死区
                    DeadbandValue = 0 // 死区值
                }
            };

            // 为位置监控项添加通知事件处理程序
            positionItem.Notification += (item, args) =>
            {
                // 遍历接收到的值
                foreach (var value in item.DequeueValues())
                {
                    // 将值转换为 double 类型
                    var position = Convert.ToDouble(value.Value);
                    // 将密度数据添加到队列中
                    if (position != 0)
                    {
                        AddPositionData(position);
                    }


                    // 触发密度数据接收事件
                    OnPositionReceived?.Invoke(this, new OpcValueEventArgs(position));
                    lock (_dataLock)
                    {
                        _latestPosition = position;
                        _latestTimestamp = value.SourceTimestamp;
                        CheckAndTriggerAllDataEvent();
                    }
                }
            };
            // 将速度监控项添加到订阅中
            _subscription.AddItem(speedItem);
            // 将密度监控项添加到订阅中
            _subscription.AddItem(densityItem);
            // 将位置监控项添加到订阅中
            _subscription.AddItem(positionItem);

            // 创建 SizeDV50 监控项
            var sizeDv50Item = new MonitoredItem(_subscription.DefaultItem)
            {
                StartNodeId = "ns=2;s=Sensor_00.PlumeStatistics.SizeDV50",
                AttributeId = Attributes.Value,
                DisplayName = "SizeDV50",
                NodeClass = NodeClass.Variable,
                SamplingInterval = _settings.SamplingInterval,
                QueueSize = _settings.QueueSize,
                DiscardOldest = true,
                Filter = new DataChangeFilter
                {
                    Trigger = DataChangeTrigger.StatusValueTimestamp,
                    DeadbandType = (uint)DeadbandType.None,
                    DeadbandValue = 0
                }
            };

            sizeDv50Item.Notification += (item, args) =>
            {
                foreach (var value in item.DequeueValues())
                {
                    try
                    {
                        var dv50 = Convert.ToDouble(value.Value);
                        OnSizeDV50Received?.Invoke(this, new OpcValueEventArgs(dv50));
                    }
                    catch { }
                }
            };

            // 创建 SizeDV90 监控项
            var sizeDv90Item = new MonitoredItem(_subscription.DefaultItem)
            {
                StartNodeId = "ns=2;s=Sensor_00.PlumeStatistics.SizeDV90",
                AttributeId = Attributes.Value,
                DisplayName = "SizeDV90",
                NodeClass = NodeClass.Variable,
                SamplingInterval = _settings.SamplingInterval,
                QueueSize = _settings.QueueSize,
                DiscardOldest = true,
                Filter = new DataChangeFilter
                {
                    Trigger = DataChangeTrigger.StatusValueTimestamp,
                    DeadbandType = (uint)DeadbandType.None,
                    DeadbandValue = 0
                }
            };

            sizeDv90Item.Notification += (item, args) =>
            {
                foreach (var value in item.DequeueValues())
                {
                    try
                    {
                        var dv90 = Convert.ToDouble(value.Value);
                        OnSizeDV90Received?.Invoke(this, new OpcValueEventArgs(dv90));
                    }
                    catch { }
                }
            };

            // 创建报警字符串监控项
            var alertItem = new MonitoredItem(_subscription.DefaultItem)
            {
                StartNodeId = "ns=2;s=Sensor_00.CurrentMean.CurrentAlertMessage",
                AttributeId = Attributes.Value,
                DisplayName = "CurrentAlertMessage",
                NodeClass = NodeClass.Variable,
                SamplingInterval = _settings.SamplingInterval,
                QueueSize = _settings.QueueSize,
                DiscardOldest = true,
                Filter = new DataChangeFilter
                {
                    Trigger = DataChangeTrigger.StatusValueTimestamp,
                    DeadbandType = (uint)DeadbandType.None,
                    DeadbandValue = 0
                }
            };

            alertItem.Notification += (item, args) =>
            {
                foreach (var value in item.DequeueValues())
                {
                    try
                    {
                        var msg = Convert.ToString(value.Value);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            OnAlertMessageReceived?.Invoke(this, new OpcStringEventArgs(msg));
                            LogService.AddLogToEnqueue($"OPC 报警消息: {msg}", EnumMsgType.Info);
                        }
                    }
                    catch { }
                }
            };

            _subscription.AddItem(sizeDv50Item);
            _subscription.AddItem(sizeDv90Item);
            _subscription.AddItem(alertItem);

        }

        // 将速度数据添加到队列中，并确保队列大小不超过最大数据点数
        private void AddSpeedData(double value)
        {
            // 将速度数据加入队列
            _speedData.Enqueue(value);
            //  LogService.AddLogToEnqueue($"速度数据已添加到队列，当前值: {value}，队列大小: {_speedData.Count}");
            // 如果队列中的数据点数超过最大数据点数，则移除最早的数据点
            while (_speedData.Count > MaxDataPoints && _speedData.TryDequeue(out _)) ;
        }

        // 将密度数据添加到队列中，并确保队列大小不超过最大数据点数
        private void AddDensityData(double value)
        {
            // 将密度数据加入队列
            _densityData.Enqueue(value);
            //  LogService.AddLogToEnqueue($"浓度数据已添加到队列，当前值: {value}，队列大小: {_densityData.Count}");
            // 如果队列中的数据点数超过最大数据点数，则移除最早的数据点
            while (_densityData.Count > MaxDataPoints && _densityData.TryDequeue(out _)) ;
        }

        private void AddPositionData(double value)
        {
            // 将密度数据加入队列
            _positionData.Enqueue(value);
            // LogService.AddLogToEnqueue($"位置数据已添加到队列，当前值: {value}，队列大小: {_positionData.Count}");
            // LogService.AddLogToEnqueue($"位置数据已添加到队列，当前值: {value}，队列大小: {_positionData.Count}");
            // 如果队列中的数据点数超过最大数据点数，则移除最早的数据点
            while (_positionData.Count > MaxDataPoints && _positionData.TryDequeue(out _)) ;
        }
        private void CheckAndTriggerAllDataEvent()
        {
            lock (_dataLock)
            {
                // 检查三种数据是否均已更新（非null）
                if (_latestSpeed.HasValue && _latestDensity.HasValue && _latestPosition.HasValue)
                {
                    // 触发事件，传递所有数据
                    OnAllDataReceived?.Invoke(this, new AllDataEventArgs(
                        _latestSpeed.Value,
                        _latestDensity.Value,
                        _latestPosition.Value,
                        _latestTimestamp
                    ));

                    // 可选：清空缓存，避免重复触发（根据需求决定是否保留最新值）
                    _latestSpeed = null;
                    _latestDensity = null;
                    _latestPosition = null;
                }
            }
        }
        // 获取当前的速度和密度数据的统计信息
        public void GetCurrentValues(TestData testData)
        {
            // 将速度队列中的数据转换为数组
            var speedValues = _speedData.ToArray();
            // 将密度队列中的数据转换为数组
            var densityValues = _densityData.ToArray();
            var positionValues = _positionData.ToArray();
            int rawSpeedCount = speedValues.Length;
            int rawDensityCount = densityValues.Length;
            int rawPositionCount = positionValues.Length;
            LogService.AddLogToEnqueue($"OPC统计->原始样本数 speed:{rawSpeedCount}, density:{rawDensityCount}, position:{rawPositionCount}", EnumMsgType.Info);

            // 定义要移除的数据点数量（前15个和后15个）
            int removeCount = 15;

            // 去掉速度数据的前15个和后15个数据点
            if (speedValues.Length > removeCount * 2)
            {
                // 跳过前15个数据，取中间的数据
                speedValues = speedValues.Skip(removeCount).Take(speedValues.Length - removeCount * 2).ToArray();
            }
            else
            {
                // 样本不足30点时不做裁剪，避免短时采集被清空后统计值全部为0。
                LogService.AddLogToEnqueue($"OPC统计->速度样本数{speedValues.Length}不足{removeCount * 2 + 1}，跳过前后裁剪", EnumMsgType.Info);
            }

            // 去掉密度数据的前15个和后15个数据点
            if (densityValues.Length > removeCount * 2)
            {
                // 跳过前15个数据，取中间的数据
                densityValues = densityValues.Skip(removeCount).Take(densityValues.Length - removeCount * 2).ToArray();
            }
            else
            {
                // 样本不足30点时不做裁剪，避免短时采集被清空后统计值全部为0。
                LogService.AddLogToEnqueue($"OPC统计->浓度样本数{densityValues.Length}不足{removeCount * 2 + 1}，跳过前后裁剪", EnumMsgType.Info);
            }

            // 去掉位置数据的前15个和后15个数据点
            if (positionValues.Length > removeCount * 2)
            {
                // 跳过前15个数据，取中间的数据
                positionValues = positionValues.Skip(removeCount).Take(positionValues.Length - removeCount * 2).ToArray();
            }
            else
            {
                // 样本不足30点时不做裁剪，避免短时采集被清空后统计值全部为0。
                LogService.AddLogToEnqueue($"OPC统计->位置样本数{positionValues.Length}不足{removeCount * 2 + 1}，跳过前后裁剪", EnumMsgType.Info);
            }

            LogService.AddLogToEnqueue($"OPC统计->裁剪后样本数 speed:{speedValues.Length}, density:{densityValues.Length}, position:{positionValues.Length}", EnumMsgType.Info);

            // 计算速度统计量
            testData.AverageSpeed = speedValues.Length > 0 ? Math.Round(speedValues.Average(), 2) : 0;
            testData.MinSpeed = speedValues.Length > 0 ? Math.Round(speedValues.Min(), 2) : 0;
            testData.MaxSpeed = speedValues.Length > 0 ? Math.Round(speedValues.Max(), 2) : 0;
            testData.StdDevSpeed = speedValues.Length > 0 ? Math.Round(CalculateStandardDeviation(speedValues), 2) : 0;

            // 计算密度统计量
            testData.AverageConcentration = densityValues.Length > 0 ? Math.Round(densityValues.Average(), 2) : 0;
            testData.MinConcentration = densityValues.Length > 0 ? Math.Round(densityValues.Min(), 2) : 0;
            testData.MaxConcentration = densityValues.Length > 0 ? Math.Round(densityValues.Max(), 2) : 0;
            testData.StdDevConcentration = densityValues.Length > 0 ? Math.Round(CalculateStandardDeviation(densityValues), 2) : 0;

            // 计算位置统计量
            testData.AveragePosition = positionValues.Length > 0 ? Math.Round(positionValues.Average(), 2) : 0;
            testData.MinPosition = positionValues.Length > 0 ? Math.Round(positionValues.Min(), 2) : 0;
            testData.MaxPosition = positionValues.Length > 0 ? Math.Round(positionValues.Max(), 2) : 0;
            testData.StdDevPosition = positionValues.Length > 0 ? Math.Round(CalculateStandardDeviation(positionValues), 2) : 0;

            if (speedValues.Length == 0 || densityValues.Length == 0 || positionValues.Length == 0)
            {
                LogService.AddLogToEnqueue("OPC统计->至少一类样本为0，统计结果可能为0；请检查MonitorIP、节点名和采样时长", EnumMsgType.Exception);
            }
        }
        // 标准差计算方法（总体标准差）
        private double CalculateStandardDeviation(double[] values)
        {
            if (values == null || values.Length == 0)
                return 0;

            double mean = values.Average();
            double sumOfSquares = values.Sum(val => Math.Pow(val - mean, 2));
            double variance = sumOfSquares / values.Length;
            return Math.Sqrt(variance);
        }

        // 如果需要样本标准差（使用n-1作为分母），可以使用这个方法
        private double CalculateSampleStandardDeviation(double[] values)
        {
            if (values == null || values.Length < 2)
                return 0;

            double mean = values.Average();
            double sumOfSquares = values.Sum(val => Math.Pow(val - mean, 2));
            double sampleVariance = sumOfSquares / (values.Length - 1);
            return Math.Sqrt(sampleVariance);
        }
        /// <summary>
        /// 清空速度和密度数据的队列
        /// </summary>
        public void ClearData()
        {
            // 清空速度数据队列
            while (_speedData.TryDequeue(out _)) ;
            // 清空密度数据队列
            while (_densityData.TryDequeue(out _)) ;
            // 清空位置数据队列
            while (_positionData.TryDequeue(out _)) ;
            LogService.AddLogToEnqueue("OPC缓存已清空(speed/density/position)", EnumMsgType.Info);
        }

        /// <summary>
        /// 向指定节点写入值（通用写入）
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteNodeValue(string nodeId, object value)
        {
            if (_session == null || !_isConnected)
                return false;
            try
            {
                var write = new WriteValue
                {
                    NodeId = NodeId.Parse(nodeId),
                    AttributeId = Attributes.Value,
                    Value = new DataValue(new Variant(value))
                };
                var nodesToWrite = new WriteValueCollection { write };
                StatusCodeCollection results;
                DiagnosticInfoCollection diag;
                _session.Write(null, nodesToWrite, out results, out diag);
                if (results != null && results.Count > 0 && StatusCode.IsGood(results[0]))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue("OPC写入失败" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                return false;
            }
        }

        /// <summary>
        /// 向 SESSION COMMAND 节点写入命令（便捷方法）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool WriteSessionCommand(string command)
        {
            return WriteNodeValue(CommandNodeId, command);
        }

        public bool TryReadNodeValueAsString(string nodeId, out string value)
        {
            value = string.Empty;
            if (_session == null || !_isConnected)
                return false;
            try
            {
                DataValue dataValue = _session.ReadValue(NodeId.Parse(nodeId));
                if (dataValue == null || StatusCode.IsBad(dataValue.StatusCode))
                    return false;

                value = Convert.ToString(dataValue.Value) ?? string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue("OPC读取字符串节点失败" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                return false;
            }
        }

        public bool TryReadNodeValueAsDouble(string nodeId, out double value)
        {
            value = 0;
            if (_session == null || !_isConnected)
                return false;
            try
            {
                DataValue dataValue = _session.ReadValue(NodeId.Parse(nodeId));
                if (dataValue == null || StatusCode.IsBad(dataValue.StatusCode) || dataValue.Value == null)
                    return false;

                value = Convert.ToDouble(dataValue.Value);
                return true;
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue("OPC读取数值节点失败" + ex.Message + ex.StackTrace, EnumMsgType.Exception);
                return false;
            }
        }

        private static bool IsStandbyState(string state)
        {
            string normalized = (state ?? string.Empty).Trim();
            return string.Equals(normalized, "0", StringComparison.OrdinalIgnoreCase)
                || string.Equals(normalized, "Standby", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsActiveState(string state)
        {
            string normalized = (state ?? string.Empty).Trim();
            return string.Equals(normalized, "1", StringComparison.OrdinalIgnoreCase)
                || string.Equals(normalized, "Active", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> WriteSessionResetAndConfirmAsync(int timeoutMs = 5000, int pollIntervalMs = 200)
        {
            double beforeLength;
            if (TryReadNodeValueAsDouble(SessionCurrentLengthNodeId, out beforeLength))
                LogService.AddLogToEnqueue($"SESSION RESET前 CurrentLength={beforeLength}", EnumMsgType.Info);

            if (!WriteSessionCommand("SESSION RESET"))
            {
                LogService.AddLogToEnqueue("向OPC写入 SESSION RESET 失败", EnumMsgType.Exception);
                return false;
            }

            DateTime deadline = DateTime.Now.AddMilliseconds(timeoutMs);
            while (DateTime.Now <= deadline)
            {
                double currentLength;
                if (TryReadNodeValueAsDouble(SessionCurrentLengthNodeId, out currentLength))
                {
                    if (Math.Abs(currentLength) < 0.000001)
                    {
                        LogService.AddLogToEnqueue("SESSION RESET确认成功，CurrentLength=0", EnumMsgType.Info);
                        return true;
                    }
                }
                await Task.Delay(pollIntervalMs);
            }

            double finalLength;
            if (TryReadNodeValueAsDouble(SessionCurrentLengthNodeId, out finalLength))
                LogService.AddLogToEnqueue($"SESSION RESET确认超时，CurrentLength={finalLength}", EnumMsgType.Exception);
            else
                LogService.AddLogToEnqueue("SESSION RESET确认超时，CurrentLength读取失败", EnumMsgType.Exception);

            return false;
        }

        public async Task<bool> WriteSessionStoreAndConfirmAsync(string code, int timeoutMs = 5000, int pollIntervalMs = 200)
        {
            string beforePath;
            TryReadNodeValueAsString(SessionLastOutputPathNodeId, out beforePath);
            LogService.AddLogToEnqueue($"SESSION STORE前 LastOutputPath={beforePath}", EnumMsgType.Info);

            // 清洗条码参数，移除文件名中不可用的字符，替换空白为下划线，限制长度
            string originalCode = code ?? string.Empty;
            string sanitizedCode = originalCode;
            try
            {
                var invalid = Path.GetInvalidFileNameChars();
                var sb = new StringBuilder();
                foreach (char c in sanitizedCode)
                {
                    if (Array.IndexOf(invalid, c) < 0)
                    {
                        sb.Append(c);
                    }
                }
                sanitizedCode = sb.ToString();
                // 将任意连续空白替换为单个下划线，避免空格作为文件名问题
                sanitizedCode = Regex.Replace(sanitizedCode, "\\s+", "_");
                // 限制文件名长度，避免过长
                const int maxLen = 60;
                if (sanitizedCode.Length > maxLen)
                    sanitizedCode = sanitizedCode.Substring(0, maxLen);
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue($"SESSION STORE 条码清洗失败: {ex.Message}", EnumMsgType.Exception);
                sanitizedCode = originalCode ?? string.Empty;
            }

            LogService.AddLogToEnqueue($"SESSION STORE 条码原始: '{originalCode}' -> 清洗后: '{sanitizedCode}'", EnumMsgType.Info);

            // 在发送 SESSION STORE 之前，先发送 SESSION FOLDER 命令，foldername = C:\Users\QR + sanitizedCode
            try
            {
                string folderName = $"C:\\Users\\QR{sanitizedCode}";
                LogService.AddLogToEnqueue($"发送 SESSION FOLDER {folderName}", EnumMsgType.Info);
                if (WriteSessionCommand($"SESSION FOLDER {folderName}"))
                {
                    LogService.AddLogToEnqueue($"SESSION FOLDER 写入成功: {folderName}", EnumMsgType.Info);
                }
                else
                {
                    LogService.AddLogToEnqueue($"SESSION FOLDER 写入失败: {folderName}", EnumMsgType.Exception);
                }
            }
            catch (Exception ex)
            {
                LogService.AddLogToEnqueue($"发送 SESSION FOLDER 失败: {ex.Message}", EnumMsgType.Exception);
            }

            if (!WriteSessionCommand($"SESSION STORE {sanitizedCode}"))
            {
                LogService.AddLogToEnqueue($"向OPC写入 SESSION STORE {sanitizedCode} 失败", EnumMsgType.Exception);
                return false;
            }

            DateTime deadline = DateTime.Now.AddMilliseconds(timeoutMs);
            while (DateTime.Now <= deadline)
            {
                string currentPath;
                if (TryReadNodeValueAsString(SessionLastOutputPathNodeId, out currentPath))
                {
                    if (!string.IsNullOrWhiteSpace(currentPath) && !string.Equals(currentPath, beforePath, StringComparison.Ordinal))
                    {
                        LogService.AddLogToEnqueue($"SESSION STORE确认成功，LastOutputPath={currentPath}", EnumMsgType.Info);
                        return true;
                    }
                }
                await Task.Delay(pollIntervalMs);
            }

            string finalPath;
            if (TryReadNodeValueAsString(SessionLastOutputPathNodeId, out finalPath))
                LogService.AddLogToEnqueue($"SESSION STORE确认超时，LastOutputPath={finalPath}", EnumMsgType.Exception);
            else
                LogService.AddLogToEnqueue("SESSION STORE确认超时，LastOutputPath读取失败", EnumMsgType.Exception);

            return false;
        }

        public async Task<bool> EnsureSensorStandbyThenActivateAsync(int timeoutMs = 5000, int pollIntervalMs = 200)
        {
            string initialState;
            if (TryReadNodeValueAsString(CameraStateNodeId, out initialState))
                LogService.AddLogToEnqueue($"SENSOR ACTIVE前 CameraState={initialState}", EnumMsgType.Info);
            else
                LogService.AddLogToEnqueue("SENSOR ACTIVE前 CameraState读取失败", EnumMsgType.Exception);

            if (!IsStandbyState(initialState))
            {
                if (!WriteSessionCommand("SENSOR IDLE"))
                {
                    LogService.AddLogToEnqueue("发送 COMMAND SENSOR IDLE 失败", EnumMsgType.Exception);
                }
                else
                {
                    LogService.AddLogToEnqueue("已发送 COMMAND SENSOR IDLE，等待进入Standby", EnumMsgType.Info);
                }

                DateTime standbyDeadline = DateTime.Now.AddMilliseconds(timeoutMs);
                bool standbyReached = false;
                while (DateTime.Now <= standbyDeadline)
                {
                    string state;
                    if (TryReadNodeValueAsString(CameraStateNodeId, out state) && IsStandbyState(state))
                    {
                        standbyReached = true;
                        LogService.AddLogToEnqueue("CameraState已进入Standby", EnumMsgType.Info);
                        break;
                    }
                    await Task.Delay(pollIntervalMs);
                }

                if (!standbyReached)
                {
                    LogService.AddLogToEnqueue("等待CameraState进入Standby超时，继续发送ACTIVE", EnumMsgType.Exception);
                }
            }

            if (!WriteSessionCommand("SENSOR ACTIVE"))
            {
                LogService.AddLogToEnqueue("向OPC写入 COMMAND SENSOR ACTIVE 失败", EnumMsgType.Exception);
                return false;
            }

            DateTime activeDeadline = DateTime.Now.AddMilliseconds(timeoutMs);
            while (DateTime.Now <= activeDeadline)
            {
                string state;
                if (TryReadNodeValueAsString(CameraStateNodeId, out state))
                {
                    if (IsActiveState(state))
                    {
                        LogService.AddLogToEnqueue($"COMMAND SENSOR ACTIVE后 CameraState={state}", EnumMsgType.Info);
                        return true;
                    }
                }
                await Task.Delay(pollIntervalMs);
            }

            string finalState;
            if (TryReadNodeValueAsString(CameraStateNodeId, out finalState))
                LogService.AddLogToEnqueue($"等待ACTIVE状态变化超时，CameraState={finalState}", EnumMsgType.Exception);
            else
                LogService.AddLogToEnqueue("等待ACTIVE状态变化超时，CameraState读取失败", EnumMsgType.Exception);

            return false;
        }

        public async Task<bool> WriteSensorIdleAndConfirmStandbyAsync(int timeoutMs = 5000, int pollIntervalMs = 200)
        {
            if (!WriteSessionCommand("SENSOR IDLE"))
            {
                LogService.AddLogToEnqueue("向OPC写入 COMMAND SENSOR IDLE 失败", EnumMsgType.Exception);
                return false;
            }

            DateTime deadline = DateTime.Now.AddMilliseconds(timeoutMs);
            while (DateTime.Now <= deadline)
            {
                string state;
                if (TryReadNodeValueAsString(CameraStateNodeId, out state) && IsStandbyState(state))
                {
                    LogService.AddLogToEnqueue("COMMAND SENSOR IDLE确认成功，CameraState=Standby", EnumMsgType.Info);
                    return true;
                }
                await Task.Delay(pollIntervalMs);
            }

            string finalState;
            if (TryReadNodeValueAsString(CameraStateNodeId, out finalState))
                LogService.AddLogToEnqueue($"COMMAND SENSOR IDLE确认超时，CameraState={finalState}", EnumMsgType.Exception);
            else
                LogService.AddLogToEnqueue("COMMAND SENSOR IDLE确认超时，CameraState读取失败", EnumMsgType.Exception);

            return false;
        }

        // 实现 IDisposable 接口，用于释放资源
        public void Dispose()
        {
            // 取消重连操作
            _reconnectCts?.Cancel();

            try
            {
                // 删除订阅
                _subscription?.Delete(true);
                // 关闭会话
                _session?.Close();
                // 释放会话资源
                _session?.Dispose();
            }
            catch (Exception ex)
            {
                // 记录释放资源时的错误日志
                LogService.AddLogToEnqueue("释放OPC资源时出错" + ex.Message + ex.StackTrace);
            }

            // 释放重连操作的令牌源
            _reconnectCts?.Dispose();
            // 标记为未连接
            _isConnected = false;
        }
    }

    // 包含连接和订阅的相关配置
    public class OpcMonitorSettings
    {
        // 发布间隔，单位为毫秒
        public int PublishingInterval { get; set; } = 200;
        // 保持活动计数
        public uint KeepAliveCount { get; set; } = 5;
        // 生命周期计数
        public uint LifetimeCount { get; set; } = 15;
        // 采样间隔，单位为毫秒
        public int SamplingInterval { get; set; } = 200;
        // 队列大小
        public uint QueueSize { get; set; } = 20;
        // 操作超时时间，单位为毫秒
        public int OperationTimeout { get; set; } = 15000;
    }

    // 事件参数类，用于在事件处理程序中传递接收到的数据值和时间戳
    public class OpcValueEventArgs : EventArgs
    {
        // 接收到的数据值
        public double Value { get; }
        // 数据的时间戳
        public DateTime Timestamp { get; }

        // 构造函数，初始化数据值和时间戳
        public OpcValueEventArgs(double value)
        {
            Value = value;
            Timestamp = DateTime.Now;
        }
    }
    // 事件参数类：字符串类型
    public class OpcStringEventArgs : EventArgs
    {
        public string Value { get; }
        public DateTime Timestamp { get; }

        public OpcStringEventArgs(string value)
        {
            Value = value;
            Timestamp = DateTime.Now;
        }
    }
    // 事件参数类，包含三种数据和时间戳
    public class AllDataEventArgs : EventArgs
    {
        public double Speed { get; }
        public double Density { get; }
        public double Position { get; }
        public DateTime Timestamp { get; }

        public AllDataEventArgs(double speed, double density, double position, DateTime timestamp)
        {
            Speed = speed;
            Density = density;
            Position = position;
            Timestamp = timestamp;
        }
    }
}