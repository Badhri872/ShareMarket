using Newtonsoft.Json;

namespace Services.WebSocket
{
    public class StrikeData
    {
        [JsonProperty("optionType")]
        public string OptionType { get; set; }

        [JsonProperty("strikePrice")]
        public string StrikePrice { get; set; }

        [JsonProperty("openPrice")]
        public string Open { get; set; }

        [JsonProperty("highPrice")]
        public string High { get; set; }

        [JsonProperty("lowPrice")]
        public string Low { get; set; }

        [JsonProperty("closePrice")]
        public string Close { get; set; }

        [JsonProperty("prevClose")]
        public string PreviousClose { get; set; }
    }
}
