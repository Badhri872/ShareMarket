using System.Text;

namespace Services.AngelWebSocket
{
    public static class ByteArrayExtensions
    {
        public static StrikeLTPData GetLTPData(this byte[] data)
        {
            return new StrikeLTPData
            {
                SubscriptionMode = data[0],
                ExchangeType = data[1],
                Token = Encoding.UTF8.GetString(data.Skip(2).Take(25).ToArray()),
                ExchangeTimestamp = DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(data.Skip(35).Take(8).ToArray(),0)).LocalDateTime,
                LTP = BitConverter.ToInt32(data.Skip(43).Take(8).ToArray(),0)/100,
            };
        }
    }
}
