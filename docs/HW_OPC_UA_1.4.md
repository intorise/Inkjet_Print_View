# HiWatch OPC UA 数据通信说明（V1.4）

- 来源文件: `docs/.archivetempHW_OPC_UA_1.4.pdf`
- 页数: 21
- 说明: 本文为 PDF 内容提取后的结构化 Markdown 版本（含关键节点与命令定义）

---

## 1. 适用范围（Scope）

本文档定义了 HiWatch 冷喷粒子传感器的 OPC UA 数据通信协议，包含：

- OPC UA 节点树结构
- 各节点数据含义
- 远程命令接口（SENSOR / PULSE / SESSION）

## 2. 总体信息（General Information）

- HiWatch 自 `SoftwareVersion 3.5` 起可选支持 OPC UA Server。
- 服务器依赖 NI 组件许可：`LabView OPC UA Toolkit deployment license (785292-35)`。
- OPC UA 服务器随 HiWatch 软件启动自动运行。
- 仅支持 `OPC.TCP` 协议。
- 若有防火墙，需开放 TCP `49580` 入站端口。
- 连接、断开传感器或修改告警限值时，TCP 连接可能中断，建议客户端自动重连。

## 3. 使用说明（Usage）

### 3.1 远程命令与控制

- 开启 Remote control 后：
	- 相机/激光控制改由远程连接独占
	- 相机切到 `Standby/Idle`
	- Laser trigger 启用
	- Analysis mode 激活

远程运行中应持续监控以下状态节点：

- `CameraState`
- `CameraTemp`
- `FrameRate`
- `SensorErrorCode`

### 3.2 安全性

- 当前版本仅明文通信，无加密。
- 无用户身份鉴别。
- 建议在隔离网络中使用。

## 4. OPC UA 节点结构

### 4.1 根节点（Root）

- `ns=2;s=SoftwareBuild`
- `ns=2;s=SoftwareClass`
- `ns=2;s=MaxSensorCount`
- `ns=2;s=SoftwareVersion`

### 4.2 传感器主目录

- `ns=2;s=Sensor_00`
	- `Location`
	- `SensorType`
	- `SensorSN`

### 4.3 数据目录说明

- `Sensor_00.CurrentMean`：最近一次均值数据
	- 典型节点：`ParticleSpeed`, `ParticleDensity`, `SprayPos`, `ParticleCount`, `SprayWidth`, `CurrentAlertMessage`
- `Sensor_00.CurrentParticles`：最近图像包中单颗粒数组数据
	- 典型节点：`ParticleAxVel`, `ParticleLatVel`, `ParticleAxPos`, `ParticleLatPos`, `ParticleDiameter`
- `Sensor_00.PlumeStatistics`：按采集窗口统计的喷雾统计量
	- 典型节点：`PlumeCount`, `PeakDensityPos`, `PeakAxVelPos`, `PeakAxVel`, `SizeCount`, `SizeDV50`, `SizeDV90`
- `Sensor_00.SensorStatus`：传感器状态
	- 典型节点：`CalibrationDate`, `HardwareProfile`, `CameraState`, `CameraTemp`, `LaserPulsing`, `CameraGain`, `FrameRate`, `SensorErrorCode`, `SensorErrorMessage`
- `Sensor_00.SessionData`：会话数据状态
	- 典型节点：`SessionName`, `AlertProfileList`, `ActiveAlertProfile`, `MaxLength`, `CurrentLength`, `OutputFolder`, `LastOutputPath`
- `Sensor_00.OPERATIONS`：命令接口
	- `ENABLED`（只读）
	- `COMMAND`（读写）
	- `RESULT`（只读）
	- `USERPROMPT`（只读）

### 4.4 数据质量状态码

- `GOOD`：正常
- `BAD`：测量失败
- `UNCERTAIN`：数据不稳定或统计量不足
- `BAD_WaitingForInitialData`：测量尚未开始

## 5. 告警对象（Alert Objects）

- 告警对象：`CurrentMeanAlertmon`
- 用途：当 CurrentMean 变量超限时触发 OPC UA Events
- 说明：修改告警限值会导致 TCP 会话断开，客户端应尽快重连并必要时刷新

## 6. 命令接口（Command Interface）

### 6.1 命令格式

```text
cmdstring {parameter}
```

- `ENABLED` 节点应先为可用状态。
- `RESULT` 仅反映命令语法/结构处理结果，不等于业务动作已完成。
- 业务成功需通过相应状态节点二次确认。

### 6.2 支持命令

#### SENSOR

```text
SENSOR [ACTIVE|IDLE]
```

- 用于切换相机状态。
- 文档强调：发送 `SENSOR ACTIVE` 前，应先检查 `SensorStatus.CameraState`。
- 当状态不满足要求时，需要先修正状态再激活。

#### PULSE

```text
PULSE [CT|DUR|INT] [value]
```

- 控制激光脉冲参数。
- 参数生效可能有最多约 1 秒延迟，需通过 `LaserPulsing` 回读确认。

#### SESSION

```text
SESSION [RESET|STORE|FOLDER|MAXLENGTH] [parameter]
```

- `SESSION RESET`：清空当前粒子数据集
- `SESSION STORE [name]`：存储当前数据到 HMI 数据表与文件
- `SESSION FOLDER [foldername]`：设置输出目录
- `SESSION MAXLENGTH [number]`：设置数据集上限

对应二次确认建议：

- `RESET`：`SessionData.CurrentLength` 归零
- `STORE`：`SessionData.SessionName` / `SessionData.LastOutputPath` 更新
- `FOLDER`：`SessionData.OutputFolder` 更新
- `MAXLENGTH`：`SessionData.MaxLength` 更新

## 7. 限制项（Limitations）

该 OPC UA 服务器不支持：

- Methods
- Views

## 8. 安装与激活摘要

- 安装后需要重启系统。
- 通过 NI License Manager 激活 `OPC UA Deployment`（示例版本：2019）。
- 首次运行可能触发防火墙放行提示，通常需要允许私有/公有网络。
- 连接传感器并加载初始配置后，Remote 页面可查看 OPC Server URL。

## 9. 与本项目直接相关的实现要点

- 监控采集核心节点（本项目已使用）：
	- `CurrentMean.ParticleSpeed`
	- `CurrentMean.ParticleDensity`
	- `CurrentMean.SprayPos`
	- `PlumeStatistics.SizeDV50`
	- `PlumeStatistics.SizeDV90`
- 冷喷启动前应读取：
	- `SensorStatus.CameraState`
	- 当状态非 Active 时再发送 `SENSOR ACTIVE`


