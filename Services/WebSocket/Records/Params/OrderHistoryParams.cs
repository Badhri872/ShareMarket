using Newtonsoft.Json;

namespace Services.WebSocket.Records
{
    public class OrderHistoryParams
    {
        [JsonProperty("nestOrderNumber")]
        public string OrderNumber;       
    }
}
