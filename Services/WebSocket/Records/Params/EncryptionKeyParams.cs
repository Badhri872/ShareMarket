using Newtonsoft.Json;

namespace Services.WebSocket.Records
{
    public class EncryptionKeyParams
    {
        [JsonProperty("userId")]
        public string UserId;
    }
}
