using Newtonsoft.Json;

namespace SmartOvenV2.Models
{
    public class OvenStatus
    {
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}