using System.Data;

namespace Innovators_ShareMarket.Models
{
    public class SpotData
    {
        private LiveData _liveData;
        public SpotData(LiveData liveData)
        {
            _liveData = liveData;
            Symbol = liveData.Exchange;
            liveData.LiveDataChanged += onLiveDataChanged;
        }

        private void onLiveDataChanged(object? sender, EventArgs e)
        {
            updatePositions();
        }

        public string Symbol { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }

        private void updatePositions()
        {
            if (Open == 0)
            {
                initializePositions(_liveData.LastTradingPrice);
            }
            else
            {
                Close = _liveData.LastTradingPrice;
                if (Close < Low) Low = Close;
                if (Close > High) High = Close;
            }
        }

        private void initializePositions(double position = 0)
        {
            Open = position;
            Close = position;
            High = position;
            Low = position;
        }
    }
}
