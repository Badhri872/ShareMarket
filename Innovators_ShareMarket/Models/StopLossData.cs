namespace Innovators_ShareMarket.Models
{
    public class StopLossData
    {
        public OptionType OptionType { get; set; }
        public int Strike { get; set; }
        public double SelfPreviousHigh { get; set; }
        public double HundredPercent => PreviousClose * 2;
        public double FiftyPercent => PreviousClose * 1.5;
        public double ThirtyPercent => PreviousClose * 1.3;
        public double PreviousClose { get; set; }
        public double NegThirtyPercent => PreviousClose * 0.7;
        public double NegFiftyPercent => PreviousClose * 0.5;
        public double NegSixtyOnePercent => PreviousClose * 0.385;
    }
}
