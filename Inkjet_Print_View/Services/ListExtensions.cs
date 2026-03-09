using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Spc_Tester.Services
{
    public static class ListExtensions
    {
        public static float MinExcludingZero(this List<double> list)
        {
            var filtered = list.Where(x => x != 0).ToList();
            return (float)(filtered.Count > 0 ? filtered.Min() : 0);
        }
    }
}
