using Innovators_ShareMarket.Models;

namespace Innovators_ShareMarket.ViewModels
{
    public class SymbolDataViewModel
    {
        public SymbolDataViewModel(SpotData data) 
        {
            Symbol = data.Symbol;
            Open = data.Open;
            Close = data.Close;
            High = data.High;
            Low = data.Low;
        }
        public string Symbol { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
    }
}
