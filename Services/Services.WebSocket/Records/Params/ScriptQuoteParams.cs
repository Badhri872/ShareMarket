using Newtonsoft.Json;

namespace Services.WebSocket.Records
{
    public class ScriptQuoteParams
    {
        [JsonProperty("exch")]
        public string Exchange;

        [JsonProperty("symbol")]
        public int InstrumentToken;
    }
}
