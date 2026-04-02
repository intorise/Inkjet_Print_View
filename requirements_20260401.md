# 1.上位机需要从OPC_UA 节点：SizeDV50，SizeDV90 获取数据，显示到界面上，保存在数据库
## 节点ID：ns=2;s=Sensor_00.PlumeStatistics.SizeDV50
• 浏览名称： SizeDV50 
• 数据类型: Double 
• 访问级别： 只读
• 变量描述： 喷雾羽流的颗粒收集统计：SizeDV50 
## 节点ID：ns=2;s=Sensor_00.PlumeStatistics.SizeDV90
• 浏览名称： SizeDV90 
• 数据类型: Double 
• 访问级别： 只读
• 变量描述： 喷雾羽流的颗粒收集统计：SizeDV90

# 2.上位机需要写入二维码、冷喷启动信号，冷喷结束信号到OPC UA 节点：
收到某个二维码的冷喷启动信号时往OPC UA的COMMAND节点写入SESSION RESET命令
收到某个二维码的冷喷结束信号时往OPC UA的COMMAND节点写入SESSION  STORE 【二维码】命令
## 节点
   ns=2;s=Sensor_00.OPERATIONS.COMMAND
# 3、上位机从OPC UA 节点：ns=2;s=Sensor_00.CurrentMean.CurrentAlertMessage 接收报警信号
报警信号应该是字符串，参考其他报警信息进行处理，例如：添加到日志