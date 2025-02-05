﻿namespace Innovators_ShareMarket.Models
{
    public class LiveData
    {
        private double _lastTradingPrice;

        public double LastTradingPrice
        {
            get 
            { 
                return _lastTradingPrice; 
            }
            set
            {
                _lastTradingPrice = value;
                LiveDataChanged?.Invoke(this, new LiveDataChangedEventArgs(Strike, _lastTradingPrice, Option));
            }
        }
        public int Strike { get; set; }
        public OptionType Option { get; set; }
        public string Exchange { get; set; }

        public int Token { get; set; }

        public DateTime DateTime { get; set; }

        public event EventHandler<LiveDataChangedEventArgs> LiveDataChanged;
    }
}
