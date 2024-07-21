using Newtonsoft.Json;

namespace Services.WebSocket
{
    public class CancelOrderParams
    {
        [JsonProperty("nestOrderNumber")]
        public string OrderNumber;
    }
}
