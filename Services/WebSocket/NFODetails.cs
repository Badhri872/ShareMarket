using Newtonsoft.Json;
using Services.WebSocket.Records;
using System.Collections.Generic;

namespace Services.WebSocket
{
    public class NFODetails
    {
        [JsonProperty("NFO")]
        public List<Contract> NFO { get; set; }
        public string contract_date { get; set; }
    }
}
