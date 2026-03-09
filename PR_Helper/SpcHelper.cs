using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_SPC
{
    /// <summary>
    /// SPC 统计分析帮助类;
    /// </summary>
    public class SpcHelper
    {
        /// <summary>
        /// 计算标准偏差
        /// </summary>
        /// <param name="arrData"></param>
        /// <returns></returns>
        public float StDev(float[] arrData) 
        {
            float xSum = 0F;
            float xAvg = 0F;
            float sSum = 0F;
            float tmpStDev = 0F;
            int arrNum = arrData.Length;
            for (int i = 0; i < arrNum; i++)
            {
                xSum += arrData[i];
            }
            xAvg = xSum / arrNum;
            for (int j = 0; j < arrNum; j++)
            {
                sSum += ((arrData[j] - xAvg) * (arrData[j] - xAvg));
            }
            tmpStDev = Convert.ToSingle(Math.Sqrt((sSum / (arrNum - 1))).ToString());
            return tmpStDev;
        }

        /// <summary>
        /// 获取CP值
        /// </summary>
        /// <param name="UpperLimit">上限</param>
        /// <param name="LowerLimit">下限</param>
        /// <param name="StDev">标准差</param>
        /// <returns></returns>
        public float GetCp(float UpperLimit, float LowerLimit, float StDev)//计算cp
        {
            float tmpV =  UpperLimit - LowerLimit;
            return Math.Abs(tmpV / (6 * StDev));
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="arrData">数据值</param>
        /// <returns></returns>
        public float Avage(float[] arrData)    //计算平均值
        {
            float tmpSum = 0F;
            for (int i = 0; i < arrData.Length; i++)
            {
                tmpSum += arrData[i];
            }
            return tmpSum / arrData.Length;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="arrData"></param>
        /// <returns></returns>
        public float Max(float[] arrData)   //计算最大值
        {
            float tmpMax = 0;
            for (int i = 0; i < arrData.Length; i++)
            {
                if (tmpMax < arrData[i])
                {
                    tmpMax = arrData[i];
                }
            }
            return tmpMax;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="arrData"></param>
        /// <returns></returns>
        public float Min(float[] arrData)  //计算最小值
        {
            if(arrData.Length == 0)
            {
                return 0f;
            }
            float tmpMin = arrData[0];
            for (int i = 1; i < arrData.Length; i++)
            {
                if (tmpMin > arrData[i])
                {
                    tmpMin = arrData[i];
                }
            }
            return tmpMin;
        }

        /// <summary>
        /// CPKU
        /// </summary>
        /// <param name="UpperLimit"></param>
        /// <param name="Avage"></param>
        /// <param name="StDev"></param>
        /// <returns></returns>
        public float CpkU(float UpperLimit, float Avage, float StDev)//计算CpkU
        {
            float tmpV = 0F;
            tmpV = UpperLimit - Avage;
            return tmpV / (3 * StDev);
        }

        /// <summary>
        /// 计算CpkL
        /// </summary>
        /// <param name="LowerLimit"></param>
        /// <param name="Avage"></param>
        /// <param name="StDev"></param>
        /// <returns></returns>
        public float CpkL(float LowerLimit, float Avage, float StDev) 
        {
            float tmpV = 0F;
            tmpV = Avage - LowerLimit;
            return tmpV / (3 * StDev);
        }


        private float Cpk(float CpkU, float CpkL)  //计算Cpk
        {
            return Math.Abs(Math.Min(CpkU, CpkL));
        }

        public float GetR_value(float[] k_valuesTOO)
        {
            float min = k_valuesTOO[0];
            float max = k_valuesTOO[0];
            for (int i = 0; i < k_valuesTOO.Length; i++)
            {
                if (k_valuesTOO[i] < min)
                {
                    min = k_valuesTOO[i];
                }
                if (k_valuesTOO[i] > max)
                {
                    max = k_valuesTOO[i];
                }
            }
            return max - min;
        }

        public float getCPK(float[] k, float UpperLimit, float LowerLimit) //获取CPK值
        {
            //CpkPro cpkPro = new CpkPro();
            //float[] k = { 0.03F, 0.06F, 0.05F, 0.03F, 0.04F, 0.04F, 0.03F, 0.04F, 0.04F, 0.04F, 0.04F, 0.04F, 0.04F, 0.03F, 0.01F, 0.03F, 0.01F, 0.03F, 0.04F, 0.04F, 0.04F, 0.05F, 0.02F, 0.04F, 0.05F, 0.05F, 0.05F, 0.05F, 0.03F, 0.05F, 0.05F, 0.03F, 0.02F, 0.04F, 0.04F, 0.02F, 0.06F, 0.04F, 0.02F, 0.03F, 0.04F, 0.02F, 0.05F, 0.06F, 0.07F, 0.02F, 0.04F, 0.04F, 0.03F, 0.03F };
            //float UpperLimit = 0.12F;  //上限
            //float LowerLimit = 0F; //下限
            //float result = cpkPro.getCPK(k, UpperLimit, LowerLimit);
            //Console.WriteLine(result);
            if (k.Length <= 1 || UpperLimit <= LowerLimit)
            {
                return -1;
            }
            float cpk = Cpk(CpkU(UpperLimit, Avage(k), StDev(k)), CpkL(LowerLimit, Avage(k), StDev(k)));
            return cpk;
        }
    }
}
