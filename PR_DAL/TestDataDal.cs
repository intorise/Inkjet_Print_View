using PR_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_DAL
{
    /// <summary>
    /// 测试数据
    /// </summary>
    public class TestDataDal
    {
        static object insertLocker = new object();

        public TestDataDal() { }

        /// <summary>
        /// 修改冷喷数据
        /// </summary>
        /// <returns></returns>
        public bool UpColdSpraydate(TestData model)
        {
            string sql = $"update test_data set StartTime ='{model.StartTime}',EndTime='{model.EndTime}'," +
                $"Beat='{model.Beat}',PlacementTime ='{model.PlacementTime}'," +
                $"ThreadRotation='{model.ThreadRotation}',IntakePressure ='{model.IntakePressure}'," +
                $"IntakePressureLowerLimit='{model.IntakePressureLowerLimit}'," +
                $"IntakePressureResult='{model.IntakePressureResult}',PlacementHour ='{model.PlacementHour}'," +
                $"IntakeFlow='{model.IntakeFlow}' " +
                $"where ID='{model.ID}' ";
            int ret = DapperHelper.Execute(sql);
            return ret > 0;
        }
        /// <summary>
        /// 修改称重数据
        /// </summary>
        /// <returns></returns>
        public bool UpHeavydate(TestData model)
        {
            string sql = $"update test_data set PreSprayWeight='{model.PreSprayWeight}',PreSprayWeight_1='{model.PreSprayWeight_1}',PreSprayWeight_1_5='{model.PreSprayWeight_1_5}',PreSprayWeight_2='{model.PreSprayWeight_2}',PreSprayWeight_2_5='{model.PreSprayWeight_2_5}',PostSprayWeight ='{model.PostSprayWeight}',PostSprayWeight_1 ='{model.PostSprayWeight_1}',PostSprayWeight_1_5 ='{model.PostSprayWeight_1_5}',PostSprayWeight_2 ='{model.PostSprayWeight_2}',PostSprayWeight_2_5 ='{model.PostSprayWeight_2_5}',SedimentationWeight='{model.SedimentationWeight}',WeightUpperLimit ='{model.WeightUpperLimit}',WeightLowerLimit='{model.WeightLowerLimit}',WeightResult ='{model.WeightResult}',AddTime='{model.AddTime}',UtilizationRate='{model.UtilizationRate}',PowderSupplySpeed='{model.PowderSupplySpeed}',Location='{model.Location}',NozzleHeight='{model.NozzleHeight}' where ID='{model.ID}'";
            int ret = DapperHelper.Execute(sql);
            return ret > 0;
        }
        /// <summary>
        /// 修改监控数据
        /// </summary>
        /// <returns></returns>
        public bool UpMonitordate(TestData model)
        {
            string sql = $"update test_data set AverageSpeed='{model.AverageSpeed}',AverageSpeedUpperLimit='{model.AverageSpeedUpperLimit}',AverageSpeedLowerLimit='{model.AverageSpeedLowerLimit}',AverageSpeedResult='{model.AverageSpeedResult}',MaxSpeed='{model.MaxSpeed}',MinSpeed ='{model.MinSpeed}',MinSpeedLowerLimit='{model.MinSpeedLowerLimit}',MinSpeedResult='{model.MinSpeedResult}',StdDevSpeed ='{model.StdDevSpeed}',SpeedResult='{model.SpeedResult}',AverageConcentration='{model.AverageConcentration}',AverageConcentrationUpperLimit='{model.AverageConcentrationUpperLimit}',AverageConcentrationLowerLimit='{model.AverageConcentrationLowerLimit}',AverageConcentrationResult='{model.AverageConcentrationResult}',MaxConcentration='{model.MaxConcentration}',MinConcentration ='{model.MinConcentration}',StdDevConcentration ='{model.StdDevConcentration}',ConcentrationResult='{model.ConcentrationResult}',AveragePosition='{model.AveragePosition}',MaxPosition='{model.MaxPosition}',MinPosition ='{model.MinPosition}',StdDevPosition ='{model.StdDevPosition}',AverageTemperature='{model.AverageTemperature}',AverageTemperatureUpperLimit='{model.AverageTemperatureUpperLimit}',AverageTemperatureLowerLimit='{model.AverageTemperatureLowerLimit}',AverageTemperatureResult='{model.AverageTemperatureResult}',MinTemperature='{model.MinTemperature}',MaxTemperature='{model.MaxTemperature}',MinTemperatureLowerLimit='{model.MinTemperatureLowerLimit}',MinTemperatureResult='{model.MinSpeedResult}',TemperatureResult='{model.TemperatureResult}',AverageNitrogenPressure='{model.AverageNitrogenPressure}',AverageNitrogenPressureUpperLimit='{model.AverageNitrogenPressureUpperLimit}',AverageNitrogenPressureLowerLimit='{model.AverageNitrogenPressureLowerLimit}',AverageNitrogenPressureResult='{model.AverageNitrogenPressureResult}',MinNitrogenPressure='{model.MinNitrogenPressure}',MaxNitrogenPressure='{model.MaxNitrogenPressure}',MinNitrogenPressureLowerLimit='{model.MinNitrogenPressureLowerLimit}',MinNitrogenPressureResult='{model.MinNitrogenPressureResult}',NitrogenPressureResult='{model.NitrogenPressureResult}' where ID='{model.ID}'";
            int ret = DapperHelper.Execute(sql);
            return ret > 0;
        }
        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(TestData model)
        {
            model.AddTime = DateTime.Now;
            string sql = "insert into test_data(Code,PreSprayWeight,PostSprayWeight,SedimentationWeight,UpperLimit,LowerLimit,Result,AddTime)";
            sql += " values(@Code,@PreSprayWeight,@PostSprayWeight,@SedimentationWeight,@UpperLimit,@LowerLimit,@Result," +
                "@AddTime)";
            return DapperHelper.Execute(sql, model) > 0;
        }

        private string GetCondition(TestDataQuery param)
        {
            string condition = " where 1=1";
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                condition += $" and Code like '%{param.Code}%'";
            }
            if (!string.IsNullOrWhiteSpace(param.StartTime))
            {
                condition += $" and AddTime>='{param.StartTime}'";
            }
            if (!string.IsNullOrWhiteSpace(param.EndTime))
            {
                condition += $" and AddTime<='{param.EndTime}'";
            }
            return condition;
        }

        /// <summary>
        /// 获取分页数据;
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<TestData> GetList(TestDataQuery param)
        {
            int startIndex = (param.PageNum - 1) * param.PageSize;
            string sql = "select * from test_data";
            sql += GetCondition(param);
            sql += " order by ID desc";
            sql += $" limit {startIndex},{param.PageSize}";
            List<TestData> list = DapperHelper.Query<TestData>(sql);
            return list;
        }

        public int GetTotalCount()
        {
            string sql = "select count(*) from test_data";
            return Convert.ToInt32(DapperHelper.ExecuteScalar(sql));
        }
        public bool DeleteByWhere()
        {
            string sql = $"DELETE FROM test_data WHERE AddTime < DATE_SUB(NOW(), INTERVAL 60 DAY) LIMIT 1000";
            int result = DapperHelper.Execute(sql);
            return result > 0;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetCount(TestDataQuery param)
        {
            string sql = "select count(*) from test_data ";
            sql += GetCondition(param);
            return Convert.ToInt32(DapperHelper.ExecuteScalar(sql));
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<TestData> GetExportData(TestDataQuery param)
        {
            string sql = "select * from test_data";
            sql += GetCondition(param);
            sql += " order by ID asc";
            return DapperHelper.Query<TestData>(sql);
        }
        /// <summary>
        /// 根据最新ID获取一个最新数据;
        /// </summary>
        /// <param name="lastID">上一次最新ID</param>
        /// <returns></returns>
        public TestData GetLastDataById(int lastID)
        {
            string sql = $"select * from test_data where ID>{lastID} order by ID desc limit 1";
            return DapperHelper.QueryFirstOrDefault<TestData>(sql);
        }
        /// <summary>
        /// 根据最新ID获取一个最新数据;
        /// </summary>
        /// <param name="lastID">上一次最新ID</param>
        /// <returns></returns>
        public TestData GetLastDataByCode(string code)
        {
            string sql = $"select * from test_data where Code='{code}' ORDER BY ID DESC LIMIT 1 ";
            return DapperHelper.QueryFirstOrDefault<TestData>(sql);
        }
        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool addNew(TestData model)
        {
            lock (insertLocker)
            {
                var sql = "insert into test_data(Code,PreSprayWeight,PreSprayWeight_1,PreSprayWeight_1_5,PreSprayWeight_2,PreSprayWeight_2_5,PostSprayWeight,PostSprayWeight_1,PostSprayWeight_1_5,PostSprayWeight_2,PostSprayWeight_2_5,SedimentationWeight,WeightUpperLimit,WeightLowerLimit,WeightResult,AddTime,UtilizationRate,AverageTemperature,AverageTemperatureUpperLimit,AverageTemperatureLowerLimit,AverageTemperatureResult,MinTemperature,MinTemperatureLowerLimit,MinTemperatureResult,MaxTemperature,TemperatureResult,AverageNitrogenPressure,AverageNitrogenPressureUpperLimit,AverageNitrogenPressureLowerLimit,AverageNitrogenPressureResult,MinNitrogenPressure,MinNitrogenPressureLowerLimit,MinNitrogenPressureResult,MaxNitrogenPressure,NitrogenPressureResult,PowderSupplySpeed,StartTime,EndTime,Beat,PlacementTime,Location,ThreadRotation,IntakePressure,IntakePressureLowerLimit,IntakePressureResult,NozzleHeight,AverageSpeed,AverageSpeedUpperLimit,AverageSpeedLowerLimit,AverageSpeedResult,MaxSpeed,MinSpeed,MinSpeedLowerLimit,MinSpeedResult,StdDevSpeed,SpeedResult,AverageConcentration,AverageConcentrationUpperLimit,AverageConcentrationLowerLimit,AverageConcentrationResult,MaxConcentration,MinConcentration,StdDevConcentration,ConcentrationResult,AveragePosition,MaxPosition,MinPosition,StdDevPosition,PlacementHour)";
                sql += " values(@Code,@PreSprayWeight,@PreSprayWeight_1,@PreSprayWeight_1_5,@PreSprayWeight_2,@PreSprayWeight_2_5,@PostSprayWeight,@PostSprayWeight_1,@PostSprayWeight_1_5,@PostSprayWeight_2,@PostSprayWeight_2_5,@SedimentationWeight,@WeightUpperLimit,@WeightLowerLimit,@WeightResult," +
                    "@AddTime,@UtilizationRate,@AverageTemperature,@AverageTemperatureUpperLimit,@AverageTemperatureLowerLimit,@AverageTemperatureResult,@MinTemperature,@MinTemperatureLowerLimit,@MinTemperatureResult,@MaxTemperature,@TemperatureResult,@AverageNitrogenPressure,@AverageNitrogenPressureUpperLimit,@AverageNitrogenPressureLowerLimit,@AverageNitrogenPressureResult,@MinNitrogenPressure,@MinNitrogenPressureLowerLimit,@MinNitrogenPressureResult,@MaxNitrogenPressure,@NitrogenPressureResult,@PowderSupplySpeed,@StartTime,@EndTime,@Beat,@PlacementTime,@Location,@ThreadRotation,@IntakePressure,@IntakePressureLowerLimit,@IntakePressureResult,@NozzleHeight,@AverageSpeed,@AverageSpeedUpperLimit,@AverageSpeedLowerLimit,@AverageSpeedResult,@MaxSpeed,@MinSpeed,@MinSpeedLowerLimit,@MinSpeedResult,@StdDevSpeed,@SpeedResult,@AverageConcentration,@AverageConcentrationUpperLimit,@AverageConcentrationLowerLimit,@AverageConcentrationResult,@MaxConcentration,@MinConcentration,@StdDevConcentration,@ConcentrationResult,@AveragePosition,@MaxPosition,@MinPosition,@StdDevPosition,@PlacementHour)";
                return DapperHelper.Execute(sql, model) > 0;
            }
        }
    }
}
