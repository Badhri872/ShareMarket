using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.WebSocket.Records
{
    internal class WebsocketSessionResult : BaseResponseResult
    {
        [JsonPropertyName("result")]
        public WebsocketSession Result { get; set; }
    }
}
