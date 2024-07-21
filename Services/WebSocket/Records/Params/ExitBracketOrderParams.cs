using Newtonsoft.Json;

namespace Services.WebSocket
{
    public class ExitBracketOrderParams
    {
        [JsonProperty("nestOrderNumber")]
        public string OrderNumber;

        [JsonProperty("symbolOrderId")]
        public string PriceType;

        [JsonProperty("status")]
        public string OrderStatus;
    }
}
