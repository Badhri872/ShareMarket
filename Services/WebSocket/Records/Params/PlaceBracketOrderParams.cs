using Newtonsoft.Json;

namespace Services.WebSocket.Records
{
    public class PlaceBracketOrderParams : PlaceCoverOrderParams
    {
        [JsonProperty("target")]
        public decimal Target;
    }
}
