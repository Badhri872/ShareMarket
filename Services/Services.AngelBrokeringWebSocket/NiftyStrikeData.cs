using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AngelWebSocket
{
    public record OptionData(
    Dictionary<string, string> DateInfo, // Dictionary to store dynamic date keys
    string InstrumentType,
    string ExpiryDate,
    string OptionType,
    double StrikePrice,
    double OpenPrice,
    double HighPrice,
    double LowPrice,
    double ClosePrice,
    double PrevClose,
    double LastPrice,
    double Change,
    double PChange,
    int NumberOfContractsTraded,
    double TotalTurnover,
    string NseIdentifier,
    double? Support,
    double? Resistance
);

    public record SummaryData(
        double Open,
        double High,
        double Low,
        double Close
    );

    public record NiftyStrikeData(
        List<OptionData> Active,
        SummaryData Summary
    );
}
