using Innovators_ShareMarket.Models;
using System.ComponentModel;
using System.Windows;

namespace Innovators_ShareMarket.ViewModels
{
    public class SymbolDataViewModel : INotifyPropertyChanged
    {
        public SymbolDataViewModel(SpotData data) 
        {
            Symbol = data.Symbol;
            Open = data.Open;
            Close = data.Close;
            High = data.High;
            Low = data.Low;

            data.OpenChanged += onOpenChanged;
            data.HighChanged += onHighChanged;
            data.LowChanged += onLowChanged;
            data.CloseChanged += onCloseChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void onCloseChanged(object? sender, EventArgs e) =>
            PropertyChanged?
            .Invoke(this, new PropertyChangedEventArgs(nameof(Close)));

        private void onLowChanged(object? sender, EventArgs e) =>
            PropertyChanged?
            .Invoke(this, new PropertyChangedEventArgs(nameof(Low)));

        private void onHighChanged(object? sender, EventArgs e) =>
            PropertyChanged?
            .Invoke(this, new PropertyChangedEventArgs(nameof(High)));

        private void onOpenChanged(object? sender, EventArgs e) =>
            PropertyChanged?
            .Invoke(this, new PropertyChangedEventArgs(nameof(Open)));

        public string Symbol { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
    }
}
