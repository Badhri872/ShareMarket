using Newtonsoft.Json;

namespace Services.WebSocket.Records
{
    public class PositionBookParams
    {
        [JsonProperty("ret")]
        public string RetentionType;
    }
}
