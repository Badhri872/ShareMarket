using Newtonsoft.Json;

namespace Services.WebSocket.Records
{
    public class WebsocketAccessTokenParams
    {
        [JsonProperty("loginType")]
        public string LoginType;
    }
}
