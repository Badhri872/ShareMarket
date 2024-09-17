using System.Text.Unicode;

namespace Services.AngelWebSocket
{
    public class StrikeLTPData
    {
        public byte SubscriptionMode { get; init; }
        public byte ExchangeType { get; init; }

        public string Token { get; init; }
        public DateTime ExchangeTimestamp { get; init; }
        public Int32 LTP { get; init; }

        public override string ToString() =>
            $"SubscriptionMode:{SubscriptionMode}, ExchangeType:{ExchangeType}, Token:{Token}, ExchangeTimestamp:{ExchangeTimestamp}, LTP:{LTP}";
    }
}
