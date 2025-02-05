﻿using System.Runtime.Serialization;

namespace Services.WebSocket.Records
{
    public class CreateWebsocketConnectionRequest
    {
        [DataMember(Name = "susertoken")]
        public string AccessToken { get; set; }
        [DataMember(Name = "t")]
        public string RequestType { get; set; } = "c";
        [DataMember(Name = "actid")]
        public string AccountId { get; set; }
        [DataMember(Name = "uid")]
        public string UserId { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; } = "API";
    }
}
