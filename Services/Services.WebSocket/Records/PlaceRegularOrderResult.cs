using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.WebSocket.Records
{
    public class PlaceRegularOrderResult : BaseResponseResult
    {
        [JsonPropertyName("NOrdNo")]
        public string OrderNumber { get; set; }       
    }
}
