using Newtonsoft.Json;

namespace SmartOvenV2.Models
{
    public class ElementsStatusInfo
    {
        public double TopTemperature { get; set; }

        public double BottomTemperature { get; set; }

        public double TopDesiredTemperature { get; set; }

        public double BottomDesiredTemperature { get; set; }

        public double TopPower { get; set; }

        public double BottomPower { get; set; }

        public double TopMaxPower { get; set; }

        public double BottomMaxPower { get; set; }

        public int TopElementStatus { get; set; }

        public int BottomElementStatus { get; set; }

        public static ElementsStatusInfo FromCommand(ElementsStatusInfoCommand command)
        {
            return new ElementsStatusInfo()
            {
                TopTemperature = command.Current[0],
                BottomTemperature = command.Current[1],
                TopDesiredTemperature = command.Desired[0],
                BottomDesiredTemperature = command.Desired[1],
                TopElementStatus = command.Status[0],
                BottomElementStatus = command.Status[1],
                TopPower = command.Power[0],
                BottomPower = command.Power[1],
                TopMaxPower = command.MaxPower[0],
                BottomMaxPower = command.MaxPower[1]
            };
        }
    }

    public class ElementsStatusInfoCommand
    {
        [JsonProperty("current")]
        public double[] Current { get; set; }

        [JsonProperty("desired")]
        public double[] Desired { get; set; }

        [JsonProperty("power")]
        public double[] Power { get; set; }

        [JsonProperty("maxpower")]
        public double[] MaxPower { get; set; }

        [JsonProperty("status")]
        public int[] Status { get; set; }
    }
}
