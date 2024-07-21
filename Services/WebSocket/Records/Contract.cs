using Newtonsoft.Json;
using System;

namespace Services.WebSocket.Records
{
    public class Contract
    {
        [JsonProperty("exch")]
        public string Exchange { get; set; }

        [JsonProperty("exchange_segment")]
        public string ExchangeSegment { get; set; }
        public DateTime? Expiry { get; set; }
        public string InstrumentName { get; set; }

        [JsonProperty("formatted_ins_name")]
        public string FormattedInstrumentName { get; set; }

        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        [JsonProperty("instrument_type")]
        public string InstrumentType { get; set; }

        [JsonProperty("lot_size")]
        public string LotSize { get; set; }

        [JsonProperty("option_type")]
        public string OptionType { get; set; }
        public decimal Strike { get; set; }
        public string InstrumentSymbol { get; set; }

        [JsonProperty("tick_size")]
        public string TickSize { get; set; }

        [JsonProperty("token")]
        public string InstrumentToken { get; set; }

        [JsonProperty("trading_symbol")]
        public string TradingSymbol { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("expiry_date")]
        public string ExpiryUnixTimer { get; set; }

        [JsonProperty("strike_price")]
        public string StrikePrice { get; set; }

        public DateTime GetTradeExpiry() => DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(ExpiryUnixTimer)).LocalDateTime;

    }
}
