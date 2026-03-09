using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_SPC
{
    /// <summary>
    /// SPC 常数查表
    /// </summary>
    public class SpcConstant
    {
        /// <summary>
        /// d2
        /// </summary>
        /// <param name="groupCount">分组大小</param>
        /// <returns></returns>
        public double Get_Table_d2(int groupCount)
        {
            double d2Val = 0;
            switch (groupCount)
            {
                case 2:
                    d2Val = 1.128;
                    break;
                case 3:
                    d2Val = 1.693;
                    break;
                case 4:
                    d2Val = 2.059;
                    break;
                case 5:
                    d2Val = 2.326;
                    break;
                case 6:
                    d2Val = 2.534;
                    break;
                case 7:
                    d2Val = 2.704;
                    break;
                case 8:
                    d2Val = 2.847;
                    break;
                case 9:
                    d2Val = 2.970;
                    break;
                case 10:
                    d2Val = 3.078;
                    break;
                default:
                    d2Val = 0;
                    break;
            }
            return d2Val;
        }

        /// <summary>
        /// A2 查表
        /// </summary>
        /// <param name="groupCount">分组</param>
        /// <returns></returns>
        public double Get_Table_A2(int groupCount)
        {
            double A2Val = 0;
            switch (groupCount)
            {
                case 2:
                    A2Val = 1.880;
                    break;
                case 3:
                    A2Val = 1.023;
                    break;
                case 4:
                    A2Val = 0.729;
                    break;
                case 5:
                    A2Val = 0.577;
                    break;
                case 6:
                    A2Val = 0.483;
                    break;
                case 7:
                    A2Val = 0.419;
                    break;
                case 8:
                    A2Val = 0.373;
                    break;
                case 9:
                    A2Val = 0.337;
                    break;
                case 10:
                    A2Val = 0.308;
                    break;
                default:
                    A2Val = 0;
                    break;
            }
            return A2Val;
        }

        /// <summary>
        /// D3查表
        /// </summary>
        /// <param name="groupCount">分组大小</param>
        /// <returns></returns>
        public double Get_Table_D3(int groupCount)
        {
            double D3val = 0;
            switch (groupCount)
            {

                case 2:
                    D3val = 0;
                    break;
                case 3:
                    D3val = 0;
                    break;
                case 4:
                    D3val = 0;
                    break;
                case 5:
                    D3val = 0;
                    break;
                case 6:
                    D3val = 0;
                    break;
                case 7:
                    D3val = 0.076;
                    break;
                case 8:
                    D3val = 0.136;
                    break;
                case 9:
                    D3val = 0.184;
                    break;
                case 10:
                    D3val = 0.223;
                    break;
                default: 
                    D3val = 0;
                    break;
            }
            return D3val;
        }

        /// <summary>
        /// D4查表
        /// </summary>
        /// <param name="groupCount">分组大小</param>
        /// <returns></returns>
        public double Get_Table_D4(int groupCount)
        {
            double D4val = 0;
            switch (groupCount)
            {

                case 2:
                    D4val = 3.268;
                    break;
                case 3:
                    D4val = 2.574;
                    break;
                case 4:
                    D4val = 2.282;
                    break;
                case 5:
                    D4val = 2.114;
                    break;
                case 6:
                    D4val = 2.004;
                    break;
                case 7:
                    D4val = 1.924;
                    break;
                case 8:
                    D4val = 1.864;
                    break;
                case 9:
                    D4val = 1.816;
                    break;
                case 10:
                    D4val = 1.777;
                    break;
                default:
                    D4val = 0;
                    break;
            }
            return D4val;
        }
    }
}
