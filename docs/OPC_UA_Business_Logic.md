# OPC UA 业务逻辑说明

## 概述
本文档说明当前工程中与 OPC UA 交互的业务流程、关键节点、命令序列、确认逻辑、报警联动与测试要点，便于测试、维护与现场调试。

## 主要实现位置
- OPC UA 客户端与订阅：Inkjet_Print_View/Moudules/HiWatchOpcMonitor.cs
- 业务流程编排与 UI：Inkjet_Print_View/FrmMain.cs
- PLC 写入封装：MecPlcHelper（位于项目的 PLC 帮助模块）

## 关键 OPC 节点
- COMMAND: `ns=2;s=Sensor_00.OPERATIONS.COMMAND` — 下发命令（SESSION / COMMAND）
- CameraState: `ns=2;s=Sensor_00.SensorStatus.CameraState` — 传感器状态（支持 `0`/`1` 或 `Standby`/`Active`）
- SessionCurrentLength: `ns=2;s=Sensor_00.SessionData.CurrentLength` — 用于判断 SESSION RESET 是否归零
- SessionLastOutputPath: `ns=2;s=Sensor_00.SessionData.LastOutputPath` — 用于判断 SESSION STORE 是否生成输出路径
- 统计与报警节点（订阅）: ParticleSpeed, ParticleDensity, SprayPos, PlumeStatistics.SizeDV50, SizeDV90, CurrentMean.CurrentAlertMessage

## 命令序列与确认逻辑
- 启动（冷喷示例）
  1. 写入 `COMMAND SENSOR ACTIVE`（通过 EnsureSensorStandbyThenActivateAsync），实现细节：若当前非 Standby，先发 `COMMAND SENSOR IDLE` 并轮询 CameraState 到 Standby（有超时），然后发 ACTIVE 并轮询到 Active。确认过程结果记录为日志（默认不阻断上层流程）。
  2. 写入 `SESSION RESET`（WriteSessionResetAndConfirmAsync）：写入后轮询 `SessionCurrentLength` 直至接近 0 或超时；成功/超时写日志。

- 结束/保存
  1. 先写 `SESSION FOLDER C:\Users\\QR{条码}`（条码在写入前清洗，去除非法文件名字符、将连续空白替换为下划线并截断到安全长度）
  2. 写 `SESSION STORE {条码(清洗后)}`（WriteSessionStoreAndConfirmAsync）：写入后轮询 `SessionLastOutputPath` 是否变化以确认为成功；结果写日志。

- 写入确认策略
  - 每个写入首先通过 `Session.Write` 获取 StatusCode（直接写入结果），然后有的命令会继续通过读取特定节点轮询确认（默认超时 5000ms，轮询 200ms）。
  - 所有确认结果会记录为日志；当前实现将确认结果作为信息记录，不作为主流程的阻断分支（可按需改为阻断）。

## 订阅与事件处理
- 使用 `Subscription` + `MonitoredItem` 订阅速度/密度/位置/SizeDV50/SizeDV90/CurrentAlertMessage。
- 每个监控项在 `Notification` 中将值转换并触发事件：`OnSpeedReceived`、`OnDensityReceived`、`OnPositionReceived`、`OnSizeDV50Received`、`OnSizeDV90Received`、`OnAlertMessageReceived`。
- 当 speed/density/position 三项都有最新值时触发 `OnAllDataReceived`（用于统计与保存）。

## 报警与 PLC 联动
- 报警字符串节点 `CurrentAlertMessage` 非空时：
  - 触发 `OnAlertMessageReceived` 并写入日志
  - 调用 PLC 写函数（例：将 D5725 置 1），实现位置在 PLC helper 中（已实现写入 D5725 的方法并由 FrmMain 订阅调用）
- 未实现自动清零策略：建议在报警清除或达到某条件时写 0 清除 D5725（可由 OPC 报警清楚事件或由手工/PLC 控制）。

## CameraState 语义与处理细节
- 支持的表示：`"0"` / `"1"` 或字符串 `Standby` / `Active`。
- 判定方法在 `HiWatchOpcMonitor` 中通过 `IsStandbyState` / `IsActiveState` 实现。
- 在 EnsureSensorStandbyThenActivateAsync 中包含先 IDLE 再 ACTIVE 的保护性步骤与超时处理；超时仅记录异常日志并继续（可配置改为阻断）。

## 配置项与超时
- 可调整项位于 `OpcMonitorSettings`：`PublishingInterval`、`SamplingInterval`、`OperationTimeout` 等。
- 命令确认默认超时：5s；轮询间隔：200ms。根据现场网络性能或响应速度可适当增减。

## 测试与验证清单（建议）
1. 集成测试：在现场 OPC UA Server + PLC 环境运行完整冷喷流程，收集日志，关注 CameraState、SessionCurrentLength、SessionLastOutputPath、D5725 写入。   
2. 条码清洗测试：使用包含非法字符、空格、超长的条码，确认 `SESSION FOLDER` 与 `SESSION STORE` 实际写入的参数被正确清洗。   
3. 报警测试：人为触发 `CurrentAlertMessage`，确认日志与 PLC D5725 写入并验证 PLC 下游响应。   
4. 会话断线重连测试：断开 OPC 后观察重连、订阅恢复与事件重新到达。

## 改进建议
- 在报警清除时自动写 0 清除 D5725，并记录清除原因与时间。  
- 将命令确认是否阻断流程设为可配置（配置中心或 UI 开关）。  
- SESSION FOLDER 采用更严格的白名单或 URL/路径编码以防路径注入或逃逸。  
- 将超时/轮询等阈值暴露到现场可调参数，便于调优。

## 关联文件
- OPC 主实现: [Inkjet_Print_View/Moudules/HiWatchOpcMonitor.cs](Inkjet_Print_View/Moudules/HiWatchOpcMonitor.cs)
- 业务编排: [Inkjet_Print_View/FrmMain.cs](Inkjet_Print_View/FrmMain.cs)

---
*生成于工作区，供测试与现场调试参考。如果需要我把该文档提交到版本控制（例如创建 Git commit），或把 D5725 的清零策略实现为代码，我可以继续实现。*