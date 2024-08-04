using Innovators_ShareMarket.Models;

namespace Innovators_ShareMarket.ViewModels
{
    public class StopLossDataViewModel
    {
        private readonly StopLossData _model;
        public StopLossDataViewModel(StopLossData model) 
        {
            _model = model;
            Strike = _model.Strike.ToString() + _model.OptionType.ToString();
            HundredPercent = _model.HundredPercent;
            FiftyPercent = _model.FiftyPercent;
            ThirtyPercent = _model.ThirtyPercent;
            NegThirtyPercent = _model.NegThirtyPercent;
            NegFiftyPercent = _model.NegFiftyPercent;
            NegSixtyOnePercent = _model.NegSixtyOnePercent;
            PreviousClose = _model.PreviousClose;
            SelfPreviousHigh = _model.SelfPreviousHigh;
        }

        public string Strike { get; set; }
        public double HundredPercent { get; set; }
        public double FiftyPercent { get; set; }
        public double ThirtyPercent { get; set; }
        public double NegThirtyPercent { get; set; }
        public double NegFiftyPercent { get; set; }
        public double NegSixtyOnePercent { get; set; }
        public double PreviousClose { get; set; }
        public double SelfPreviousHigh { get; set; }
    }
}
