using Newtonsoft.Json;

namespace Innovators_ShareMarket.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FifteenMinutesData
    {
        private double _close;

        [JsonProperty]
        public int Strike { get; set; }
        [JsonProperty]
        public OptionType Option { get; set; }
        [JsonProperty]
        public double Open { get; set; }
        [JsonProperty]
        public double Close
        {
            get 
            { 
                return _close; 
            }
            set
            {
                if (value != 0)
                {
                    _close = value;
                    updatePositions();
                }
            }
        }
        [JsonProperty]
        public double High { get; set; }
        [JsonProperty]
        public double Low { get; set; }

        private void updatePositions()
        {
            if (Open == 0)
            {
                initializePositions(_close);
            }
            else
            {
                if (Close < Low && Close != 0) Low = Close;
                if (Close > High && Close != 0) High = Close;
            }
        }

        private void initializePositions(double position = 0)
        {
            Open = position;
            High = position;
            Low = position;
        }
    }
}
