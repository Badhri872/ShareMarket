﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Services.WebSocket.Records
{
    internal class EncryptionKeyResult : BaseResponseResult
    {
        [JsonPropertyName("userId")]
        public string UserID { get; set; }
        [JsonPropertyName("encKey")]
        public string EncryptionKey { get; set; }
    }
}