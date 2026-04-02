集成与验收测试计划

前提条件：
- 已部署或可访问的 OPC UA 测试/仿真服务器（可用 Prosys、Kepware、open62541 或其他）。
- 数据库备份已完成（对 `test_data` 表做变更前必须备份）。
- 应用配置中的 `MonitorIP` 指向 OPC UA 端点。

手动验收用例：

用例 A — SizeDV 读取与写入数据库（高优先级）
1. 确保 OPC 仿真器已存在节点：
   - `ns=2;s=Sensor_00.PlumeStatistics.SizeDV50`
   - `ns=2;s=Sensor_00.PlumeStatistics.SizeDV90`
2. 在 OPC 仿真器写入测试值（例如 12.34、56.78）。
3. 在应用中触发对应条码的监控流程（或启动监控线程），确认 UI 显示最新值。
4. 在数据库中检索对应 `test_data` 记录（按条码或 ID），确认 `SizeDV50` 与 `SizeDV90` 字段已被写入且值正确。

用例 B — 写入 SESSION 命令（冷喷开始/结束）（高优先级）
1. 确保 OPC 仿真器存在可写节点：`ns=2;s=Sensor_00.OPERATIONS.COMMAND`（字符串类型）。
2. 在冷喷触发点（PLC 写 D5670=1 或手动触发相应代码路径）启动冷喷流程。
3. 检查 OPC 仿真器是否接收到 `SESSION RESET`（启动时）和 `SESSION STORE {code}`（结束时）字符串。
4. 若仿真器支持回读或有设备响应，确认设备侧行为（或在日志中判断写入成功/失败）。

用例 C — 报警字符串订阅（中优先级）
1. 在 OPC 仿真器向 `ns=2;s=Sensor_00.CurrentMean.CurrentAlertMessage` 写入示例报警文本。
2. 在应用中观察日志（或报警表），确认报警文本被记录并带有时间戳。

用例 D — 异常与容错
1. 在应用运行时关闭 OPC 仿真器，验证应用是否能记录连接失败并重试。
2. 对 `WriteSessionCommand` 强制返回失败或拒绝写入，验证上位机的异常日志与重试逻辑。

自动化测试建议：
- 单元测试：
  - Mock `Session` 或抽象一个 `IOpcSession` 接口，针对 `HiWatchOpcMonitor` 的 `WriteNodeValue`、订阅解析（数值/字符串）进行单元测试。
  - 测试 `GetCurrentValues` 的统计逻辑（采样裁剪、标准差计算）。
- 集成测试：
  - 使用 open62541 或其他可编程的 OPC UA 模拟器，在 CI 或本地运行脚本来推送 SizeDV 值并读取上位机写入。可用简单 Python 脚本或 NodeJS 客户端完成操作。

数据库变更（迁移脚本）：
- 在执行前请备份 `test_data` 表。
- 变更 SQL 示例见 `sql/add_sizedv_columns.sql`。

日志与验证：
- 在测试执行时，开启上位机日志并记录每次写入/订阅的成功或失败信息（`LogService` 输出）。

后续步骤：
- 若需要，我可以生成一个简单的 Python 脚本示例，作为 OPC 仿真器的替代（基于 open62541 或 opcua 库），用于自动化推送 SizeDV 与验证 SESSION 命令回写。