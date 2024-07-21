using System.Linq;
using System.Runtime.Serialization;

namespace Services.WebSocket.Records
{
    public class SubscribeFeedDataRequest
    {
        [DataMember(Name = "t")]
        public string RequestType { get; set; } = "t";

        [DataMember(Name = "k")]
        public string Tokens { get
            {
                return SubscriptionTokens
                    .Select(x => $"{x.Exchange}|{x.Token}")
                    .Aggregate((left, right) => $"{left}#{right}");
            }
        }

        [IgnoreDataMember]
        public SubscriptionToken[] SubscriptionTokens { get; set; }

    }
}
