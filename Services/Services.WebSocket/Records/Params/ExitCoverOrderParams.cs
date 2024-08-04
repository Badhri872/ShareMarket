using Newtonsoft.Json;

namespace Services.WebSocket
{
    public class ExitCoverOrderParams
    {
        [JsonProperty("nestOrderNumber")]
        public string OrderNumber;
    }
}
