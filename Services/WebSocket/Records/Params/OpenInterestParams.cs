using Newtonsoft.Json;
using System.Linq;

namespace Services.WebSocket.Records
{
    public class OpenInterestParams
    {
        public OpenInterestToken[] OpenInterestTokens { get; set; }
    }
}
