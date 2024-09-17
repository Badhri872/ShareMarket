using Newtonsoft.Json;

namespace Innovators_ShareMarket.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FifteenMinutesData
    {
        private double _close;
        public EventHandler OpenData, HighData, LowData, CloseData;

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
                    CloseData?.Invoke(this, EventArgs.Empty);
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
                if (Close < Low && Close != 0)
                {
                    Low = Close;
                    LowData?.Invoke(this, EventArgs.Empty);
                }
                if (Close > High && Close != 0)
                {
                    High = Close;
                    HighData?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void initializePositions(double position = 0)
        {
            Open = position;
            High = position;
            Low = position;

            OpenData?.Invoke(this, EventArgs.Empty);
            LowData?.Invoke(this, EventArgs.Empty);
            HighData?.Invoke(this, EventArgs.Empty);
        }
    }
}
