namespace Innovators_ShareMarket.Models
{
    public class LiveDataChangedEventArgs : EventArgs
    {
        public LiveDataChangedEventArgs(int strike, double ltp, OptionType option) 
        {
            Strike = strike;
            LTP = ltp;
            Option = option;
        }
        public int Strike { get; }
        public OptionType Option { get; }
        public double LTP { get; }
    }
}
