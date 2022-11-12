using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace SmartOvenV2.Models
{
    public class CpuStatusInfo
    {
        public double TopCpuTemperature { get; set; }
        public double BottomCpuTemperature { get; set; }

        public static CpuStatusInfo FromDouble(double[] values)
        {
            var ret = new CpuStatusInfo();
            ret.TopCpuTemperature = values[0];
            ret.BottomCpuTemperature = values[1];

            return ret;
        }
    }
}
