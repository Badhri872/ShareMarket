using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.WebSocket.Records
{
    internal class WebsocketSession
    {
        [JsonPropertyName("wsSess")]
        public string SessionId { get; set; }
    }
}
