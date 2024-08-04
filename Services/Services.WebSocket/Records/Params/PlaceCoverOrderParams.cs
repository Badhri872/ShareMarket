using Newtonsoft.Json;

namespace Services.WebSocket.Records
{
    public class PlaceCoverOrderParams : PlaceRegularOrderParams
    {
        [JsonProperty("trailing_stop_loss")]
        public decimal TrailingStopLoss;

        [JsonProperty("stopLoss")]
        public decimal StopLoss;
    }
}
