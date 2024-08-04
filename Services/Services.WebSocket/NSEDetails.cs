using Newtonsoft.Json;
using Services.WebSocket.Records;
using System.Collections.Generic;

namespace Services.WebSocket
{
    public class NSEDetails
    {
        [JsonProperty("NSE")]
        public List<Contract> NSE { get; set; }
        public string contract_date { get; set; }
    }
}
