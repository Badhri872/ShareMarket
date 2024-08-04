using System;
using Utf8Json;

namespace Services.WebSocket.Records
{
    public class Tick
    {
        public Tick(dynamic data)
        {
            try
            {
                var responsType = data["t"];

                Exchange = data["e"];
                Token = Utilities.ParseToInt(data["tk"]);
                FeedTime = Utilities.IsPropertyExist(data, "ft") ? Utilities.ToDateTimeFromUnixTimeSeconds(data["ft"]) : null;

                if (responsType == Constants.SOCKET_RESPONSE_TYPE_TICK_ACKNOWLEDGEMENT)
                    TickType = TICK_TYPE.Tick_Ack;
                else if (responsType == Constants.SOCKET_RESPONSE_TYPE_TICK_DEPTH_ACKNOWLEDGEMENT)
                    TickType = TICK_TYPE.Tick_Depth_Ack;
                else if (responsType == Constants.SOCKET_RESPONSE_TYPE_TICK)
                    TickType = TICK_TYPE.Tick;
                else if (responsType == Constants.SOCKET_RESPONSE_TYPE_TICK_DEPTH)
                    TickType = TICK_TYPE.Tick_Depth;

                TradingSymbol = Utilities.IsPropertyExist(data, "ts") ? data["ts"] : null;

                LotSize = Utilities.IsPropertyExist(data, "ls") ? Utilities.ParseToInt(data["ls"]) : 0;
                TickSize = Utilities.IsPropertyExist(data, "ti") ? Utilities.ParseToDouble(data["ti"]) : 0;
                Close = Utilities.IsPropertyExist(data, "c") ? Utilities.ParseToDouble(data["c"]) : 0;
                LastTradedPrice = Utilities.IsPropertyExist(data, "lp") ? Utilities.ParseToDouble(data["lp"]) : 0;
                PercentageChange = Utilities.IsPropertyExist(data, "pc") ? Utilities.ParseToDouble(data["pc"]) : 0;
                Open = Utilities.IsPropertyExist(data, "o") ? Utilities.ParseToDouble(data["o"]) : 0;
                High = Utilities.IsPropertyExist(data, "h") ? Utilities.ParseToDouble(data["h"]) : 0;
                Low = Utilities.IsPropertyExist(data, "l") ? Utilities.ParseToDouble(data["l"]) : 0;

                AveragePrice = Utilities.IsPropertyExist(data, "ap") ? Utilities.ParseToDouble(data["ap"]) : 0;
                Volume = Utilities.IsPropertyExist(data, "v") ? Utilities.ParseToInt(data["v"]) : 0;

                BuyPrice1 = Utilities.IsPropertyExist(data, "bp1") ? Utilities.ParseToDouble(data["bp1"]) : 0;
                SellPrice1 = Utilities.IsPropertyExist(data, "sp1") ? Utilities.ParseToDouble(data["sp1"]) : 0;
                BuyQty1 = Utilities.IsPropertyExist(data, "bq1") ? Utilities.ParseToInt(data["bq1"]) : 0;
                SellQty1 = Utilities.IsPropertyExist(data, "sq1") ? Utilities.ParseToInt(data["sq1"]) : 0;

                TotalOpenInterest = Utilities.IsPropertyExist(data, "toi") ? Utilities.ParseToInt(data["toi"]) : 0;

                LastTradedQty = Utilities.IsPropertyExist(data, "ltq") ? Utilities.ParseToInt(data["ltq"]) : 0;
                TotalBuyQty = Utilities.IsPropertyExist(data, "tbq") ? Utilities.ParseToInt(data["tbq"]) : 0;
                TotalSellQty = Utilities.IsPropertyExist(data, "tsq") ? Utilities.ParseToInt(data["tsq"]) : 0;

                BuyPrice2 = Utilities.IsPropertyExist(data, "bp2") ? Utilities.ParseToDouble(data["bp2"]) : 0;
                SellPrice2 = Utilities.IsPropertyExist(data, "sp2") ? Utilities.ParseToDouble(data["sp2"]) : 0;

                BuyPrice3 = Utilities.IsPropertyExist(data, "bp3") ? Utilities.ParseToDouble(data["bp3"]) : 0;
                SellPrice3 = Utilities.IsPropertyExist(data, "sp3") ? Utilities.ParseToDouble(data["sp3"]) : 0;

                BuyPrice4 = Utilities.IsPropertyExist(data, "bp4") ? Utilities.ParseToDouble(data["bp4"]) : 0;
                SellPrice4 = Utilities.IsPropertyExist(data, "sp4") ? Utilities.ParseToDouble(data["sp4"]) : 0;

                BuyPrice5 = Utilities.IsPropertyExist(data, "bp5") ? Utilities.ParseToDouble(data["bp5"]) : 0;
                SellPrice5 = Utilities.IsPropertyExist(data, "sp5") ? Utilities.ParseToDouble(data["sp5"]) : 0;

                BuyQty2 = Utilities.IsPropertyExist(data, "bq2") ? Utilities.ParseToInt(data["bq2"]) : 0;
                SellQty2 = Utilities.IsPropertyExist(data, "sq2") ? Utilities.ParseToInt(data["sq2"]) : 0;

                BuyQty3 = Utilities.IsPropertyExist(data, "bq3") ? Utilities.ParseToInt(data["bq3"]) : 0;
                SellQty3 = Utilities.IsPropertyExist(data, "sq3") ? Utilities.ParseToInt(data["sq3"]) : 0;

                BuyQty4 = Utilities.IsPropertyExist(data, "bq4") ? Utilities.ParseToInt(data["bq4"]) : 0;
                SellQty4 = Utilities.IsPropertyExist(data, "sq4") ? Utilities.ParseToInt(data["sq4"]) : 0;

                BuyQty5 = Utilities.IsPropertyExist(data, "bq5") ? Utilities.ParseToInt(data["bq5"]) : 0;
                SellQty5 = Utilities.IsPropertyExist(data, "sq5") ? Utilities.ParseToInt(data["sq5"]) : 0;

                UpperCircuit = Utilities.IsPropertyExist(data, "uc") ? Utilities.ParseToDouble(data["uc"]) : 0;
                LowerCircuit = Utilities.IsPropertyExist(data, "lc") ? Utilities.ParseToDouble(data["lc"]) : 0;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        public TICK_TYPE TickType { get; }
        public string Exchange { get;  }

        public int? Token { get;  }

        public string TradingSymbol { get; }
        public int LotSize { get; }

        public decimal TickSize { get; set; }

        public decimal? LastTradedPrice { get; set; }

        public DateTime? FeedTime { get; set; }

        public decimal? PercentageChange { get; set; }
        
        public decimal PreviousDayClose { get; set; }

        [JsonFormatter(typeof(decimal))]
        public decimal? ChangeValue { get; set; }

        public int? Volume { get; set; }

        public decimal? Open { get; set; }

        public decimal? High { get; set; }

        public decimal? Low { get; set; }

        public decimal? Close { get; set; }

        public decimal? AveragePrice { get; set; }

        public decimal OpenInterest { get; set; }

        public decimal BuyPrice1 { get; set; }

        public decimal BuyQty1 { get; set; }

        public decimal SellPrice1 { get; set; }

        public decimal SellQty1 { get; set; }

        public int LastTradedQty { get; set; }

        public int TotalBuyQty { get; set; }
        public int TotalSellQty { get; set; }
        public int TotalOpenInterest { get; set; }


        public decimal BuyPrice2 { get; set; }

        public decimal BuyQty2 { get; set; }

        public decimal SellPrice2 { get; set; }

        public decimal SellQty2 { get; set; }



        public decimal BuyPrice3 { get; set; }

        public decimal BuyQty3 { get; set; }

        public decimal SellPrice3 { get; set; }

        public decimal SellQty3 { get; set; }



        public decimal BuyPrice4 { get; set; }

        public decimal BuyQty4 { get; set; }

        public decimal SellPrice4 { get; set; }

        public decimal SellQty4 { get; set; }



        public decimal BuyPrice5 { get; set; }

        public decimal BuyQty5 { get; set; }

        public decimal SellPrice5 { get; set; }

        public decimal SellQty5 { get; set; }

        public decimal UpperCircuit { get; set; }
        public decimal LowerCircuit { get; set; }
    }
}
