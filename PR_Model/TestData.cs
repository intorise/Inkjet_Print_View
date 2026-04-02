using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Model
{
    /// <summary>
    /// 测试数据
    /// </summary>
    public class TestData
    {
        public TestData() { }

        [ExcelIgnore]
        public int ID { get; set; }


        [ExcelColumnName("条码(QR-Code)"),ExcelColumnWidth(30)]
        /// <summary>
        /// 刻印条码
        /// </summary>
        public string Code { get; set; }

        [ExcelColumnName("喷前重量(Weight Before Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷前重量
        /// </summary>
        public float PreSprayWeight { get; set; }
        [ExcelColumnName("喷前重量1S（Weight Before Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷前重量
        /// </summary>
        public float PreSprayWeight_1 { get; set; }
        [ExcelColumnName("喷前重量1.5S(Weight Before Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷前重量
        /// </summary>
        public float PreSprayWeight_1_5 { get; set; }
        [ExcelColumnName("喷前重量2S(Weight Before Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷前重量
        /// </summary>
        public float PreSprayWeight_2 { get; set; }
        [ExcelColumnName("喷前重量2.5S(Weight Before Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷前重量
        /// </summary>
        public float PreSprayWeight_2_5 { get; set; }

        [ExcelColumnName("喷后重量（Weight After Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷后重量
        /// </summary>
        public float PostSprayWeight { get; set; }
        [ExcelColumnName("喷后重量1s(Weight After Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷后重量
        /// </summary>
        public float PostSprayWeight_1 { get; set; }
        [ExcelColumnName("喷后重量1.5s(Weight After Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷后重量
        /// </summary>
        public float PostSprayWeight_1_5 { get; set; }
        [ExcelColumnName("喷后重量2s(Weight After Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷后重量
        /// </summary>
        public float PostSprayWeight_2 { get; set; }
        [ExcelColumnName("喷后重量2.5s(Weight After Spraying)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷后重量
        /// </summary>
        public float PostSprayWeight_2_5 { get; set; }

        [ExcelColumnName("喷淋位置(Spraying Position)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 喷淋位置
        /// </summary>
        public string Location { get; set; }

        [ExcelColumnName("沉积重量(Weight Gain)"), ExcelColumnWidth(10)]
        /// <summary>
        /// 沉积重量
        /// </summary>
        public float SedimentationWeight { get; set; }

        [ExcelColumnName("重量上限(Upper Limit of Weight)"), ExcelColumnWidth(10)]
        /// <summary>
        /// 重量上限
        /// </summary>
        public float WeightUpperLimit { get;set;}

        [ExcelColumnName("重量下限(Lower Limit of Weight)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 重量下限 
        /// </summary>
        public float WeightLowerLimit { get;set;}

        [ExcelColumnName("重量结果(Result of weight)(0:待检测，1:OK,2:NG)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 结果(0:待检测，1:OK,2:NG)
        /// </summary>
        public string WeightResult { get; set; }
        [ExcelColumnName("重量测试时间(Weight AddTime)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 测试时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        [ExcelColumnName("银粉利用率(Powder Utilization Rate)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 银粉利用率 
        /// </summary>
        public string UtilizationRate { get; set; }
        [ExcelColumnName("平均温度(Average Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均温度 
        /// </summary>
        public float AverageTemperature { get; set; }
        [ExcelColumnName("平均温度上限(Upper Limit of Average Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均温度上限 
        /// </summary>
        public float AverageTemperatureUpperLimit { get; set; }
        [ExcelColumnName("平均温度下限(Lower Limit of Average Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均温度下限 
        /// </summary>
        public float AverageTemperatureLowerLimit { get; set; }
        [ExcelColumnName("平均温度结果(result of Average Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均温度结果 
        /// </summary>
        public string AverageTemperatureResult { get; set; }
        [ExcelColumnName("最小温度(Min Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小温度 
        /// </summary>
        public float MinTemperature { get; set; }

        [ExcelColumnName("最小温度下限(Lower Limit of Min Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        ///最小温度下限 
        /// </summary>
        public float MinTemperatureLowerLimit { get; set; } = 300;
        [ExcelColumnName("最小温度结果(result  of Min Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小温度结果 
        /// </summary>
        public string MinTemperatureResult { get; set; }

        [ExcelColumnName("最大温度(Max Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最大温度 
        /// </summary>
        public float MaxTemperature { get; set; }

        [ExcelColumnName("温度结果(Result of Temperature)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 温度结果 
        /// </summary>
        public string TemperatureResult { get; set; }

        [ExcelColumnName("平均氮气压力(Average Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均氮气压力 
        /// </summary>
        public float AverageNitrogenPressure { get; set; }

        [ExcelColumnName("平均氮气压力上限(Upper Limit of Average Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均氮气压力上限 
        /// </summary>
        public float AverageNitrogenPressureUpperLimit { get; set; }

        [ExcelColumnName("平均氮气压力下限(Lower Limit of Average Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均氮气压力下限 
        /// </summary>
        public float AverageNitrogenPressureLowerLimit { get; set; }

        [ExcelColumnName("平均氮气压力结果(Result of Average Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均氮气压力结果 
        /// </summary>
        public string AverageNitrogenPressureResult { get; set; }
        [ExcelColumnName("最小氮气压力(Min Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小氮气压力 
        /// </summary>
        public float MinNitrogenPressure { get; set; }

        [ExcelColumnName("最小氮气压力下限(Lower Limit of Min Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小氮气压力下限 
        /// </summary>
        public float MinNitrogenPressureLowerLimit { get; set; } = 300;
        [ExcelColumnName("最小氮气压力结果(Result of Min Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小氮气压力结果 
        /// </summary>
        public string MinNitrogenPressureResult { get; set; }

        [ExcelColumnName("最大氮气压力(Max Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最大氮气压力 
        /// </summary>
        public float MaxNitrogenPressure { get; set; }

        [ExcelColumnName("氮气压力结果(Result of Nitrogen Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 氮气压力结果 
        /// </summary>
        public string NitrogenPressureResult { get; set; }

        [ExcelColumnName("供粉速度(Powder Feeding Rate)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 供粉速度 
        /// </summary>
        public float PowderSupplySpeed { get; set; }
        [ExcelColumnName("冷喷启动时间(Start Time)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 冷喷启动时间 
        /// </summary>
        public DateTime? StartTime { get; set; }
        [ExcelColumnName("冷喷停止时间(Stop Time)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 冷喷停止时间 
        /// </summary>
        public DateTime? EndTime { get; set; }
        [ExcelColumnName("冷喷节拍(Cycle Time)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 冷喷节拍 
        /// </summary>
        public float Beat { get; set; }
        [ExcelColumnName("激光清洗后扫码时间(Scanning Time After Laser Cleaning)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 激光清洗后摆放时间 
        /// </summary>
        public DateTime? PlacementTime { get; set; }
        [ExcelColumnName("螺杆转速(Auger Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 螺纹旋转 
        /// </summary>
        public float ThreadRotation { get; set; }
        [ExcelColumnName("进气压力(Inlet Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 进气压力 
        /// </summary>
        public float IntakePressure { get; set; }
        [ExcelColumnName("进气压力下限(Lower Limit of Inlet Gas Pressure)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 进气压力下限 
        /// </summary>
        public float IntakePressureLowerLimit { get; set; } = 2.15f;
        [ExcelColumnName("进气压力结果(Inlet Gas Pressure result)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 进气压力结果
        /// </summary>
        public string IntakePressureResult { get; set; }
        [ExcelColumnName("喷嘴高度(Nozzle Height)"), ExcelColumnWidth(30)]

        [ExcelColumn(Name = "进气流量", Width = 30)]
        public double IntakeFlow { get; set; }

        /// <summary>
        /// 喷嘴高度 
        /// </summary>
        public float NozzleHeight { get; set; }
        [ExcelColumnName("平均粒子速度(Average Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均速度 
        /// </summary>
        public double AverageSpeed { get; set; }

        [ExcelColumnName("平均粒子速度上限(Upper Limit of Average Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均速度上限 
        /// </summary>
        public double AverageSpeedUpperLimit { get; set; }

        [ExcelColumnName("平均粒子速度下限(Lower Limit of Average Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均速度下限 
        /// </summary>
        public double AverageSpeedLowerLimit { get; set; }

        [ExcelColumnName("平均粒子速度结果(Result of Average Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均速度结果 
        /// </summary>
        public string AverageSpeedResult { get; set; }

        [ExcelColumnName("最大粒子速度(Max Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最大速度 
        /// </summary>
        public double MaxSpeed { get; set; }
        [ExcelColumnName("最小粒子速度(Min Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小速度 
        /// </summary>
        public double MinSpeed { get; set; }

        [ExcelColumnName("最小粒子速度下限(Lower Limit of Min Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小速度下限
        /// </summary>
        public double MinSpeedLowerLimit { get; set; } = 450;

        [ExcelColumnName("最小粒子速度结果(Result of Min Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小速度结果 
        /// </summary>
        public string MinSpeedResult { get; set; }
        [ExcelColumnName("粒子速度标准偏差值(Speed Standard Deviation)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 速度偏差值
        /// </summary>
        public double StdDevSpeed { get; set; }

        [ExcelColumnName("粒子速度结果(Result of Speed)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 速度结果 
        /// </summary>
        public string SpeedResult { get; set; }
        [ExcelColumnName("平均粒子浓度(Average Concentration)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均浓度 
        /// </summary>
        public double AverageConcentration { get; set; }

        [ExcelColumnName("平均粒子浓度上限(Upper Limit of Average Concentration)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均浓度上限 
        /// </summary>
        public double AverageConcentrationUpperLimit { get; set; }

        [ExcelColumnName("平均粒子浓度下限(Lower Limit of Average Concentration)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均浓度下限 
        /// </summary>
        public double AverageConcentrationLowerLimit { get; set; }

        [ExcelColumnName("平均粒子浓度结果(Result of Average Concentration)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 平均浓度结果 
        /// </summary>
        public string AverageConcentrationResult { get; set; }
        [ExcelColumnName("最大粒子浓度(Max Concentration)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最大浓度
        /// </summary>
        public double MaxConcentration { get; set; }
        [ExcelColumnName("最小粒子浓度(Min Concentration)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 最小浓度 
        /// </summary>
        public double MinConcentration { get; set; }

        [ExcelColumnName("粒子浓度标准偏差值(Concentration Standard Deviation )"), ExcelColumnWidth(30)]
        /// <summary>
        /// 浓度偏差值
        /// </summary>
        public double StdDevConcentration { get; set; }

        [ExcelColumnName("粒子浓度结果(Result of Concentration)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 浓度结果 
        /// </summary>
        public string ConcentrationResult { get; set; }

        [ExcelColumnName("粒子位置平均值(Average Position)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 粒子束中心偏移量平均值
        /// </summary>
        public double AveragePosition { get; set; }
        [ExcelColumnName("粒子位置最大值 (Max Flux Position)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 粒子束中心偏移量最大值 
        /// </summary>
        public double MaxPosition { get; set; }
        [ExcelColumnName("粒子位置最小值(Min Flux Position )"), ExcelColumnWidth(30)]
        /// <summary>
        /// 粒子束中心偏移量最小值
        /// </summary>
        public double MinPosition { get; set; }
        [ExcelColumnName("粒子位置标准偏差值(Position Standard Deviation)"), ExcelColumnWidth(30)]
        /// <summary>
        /// 位置偏差值
        /// </summary>
        public double StdDevPosition { get; set; }
        [ExcelColumnName("SizeDV50"), ExcelColumnWidth(30)]
        /// <summary>
        /// 粒径统计 DV50
        /// </summary>
        public double SizeDV50 { get; set; }

        [ExcelColumnName("SizeDV90"), ExcelColumnWidth(30)]
        /// <summary>
        /// 粒径统计 DV90
        /// </summary>
        public double SizeDV90 { get; set; }

        [ExcelColumnName("摆放时间(取小时)(Placement Time(h))"), ExcelColumnWidth(30)]

        /// <summary>
        /// 摆放时间(取小时)
        /// </summary>
        public string PlacementHour { get; set; }

      

        public override string ToString()
        {
            return $"ID:{ID},条码：{Code},喷前重量：{PreSprayWeight},喷后重量：{PostSprayWeight},沉积重量：{SedimentationWeight},重量上限：{WeightUpperLimit},重量下限：{WeightLowerLimit},结果：{WeightResult}";
        }
    }
}
