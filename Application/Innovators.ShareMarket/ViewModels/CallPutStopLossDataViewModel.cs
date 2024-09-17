using MoreLinq;
using Innovators_ShareMarket.Models;
using System.ComponentModel;

namespace Innovators_ShareMarket.ViewModels
{
    public class CallPutStopLossDataViewModel : INotifyPropertyChanged
    {
        private readonly OptionTradingDetails _tradingDetails;
        public CallPutStopLossDataViewModel(
                    OptionTradingDetails optionTradingDetails,
                    IEnumerable<LiveData> liveDataCollection)
        {
            _tradingDetails = optionTradingDetails;
            IntraDayDataVM = new IntraDayDataViewModel(optionTradingDetails, liveDataCollection);
            IntraDayDataVM.SelectedStrikeChanged += onSelectedStrikeChanged;
            updateCallPutData(IntraDayDataVM.SelectedStrike);
        }

        public CallPutStopLossDataViewModel(
                    OptionTradingDetails optionTradingDetails,
                    IEnumerable<LiveData> liveDataCollection,
                    int selectedStrike)
        {
            _tradingDetails = optionTradingDetails;
            IntraDayDataVM = new IntraDayDataViewModel(optionTradingDetails, liveDataCollection, selectedStrike);
            IntraDayDataVM.SelectedStrikeChanged += onSelectedStrikeChanged;
            updateCallPutData(IntraDayDataVM.SelectedStrike);
        }
        ~CallPutStopLossDataViewModel()
        {
            IntraDayDataVM.SelectedStrikeChanged -= onSelectedStrikeChanged;
        }

        public StopLossDataViewModel Call { get; set; }
        public StopLossDataViewModel Put { get; set; }
        public IntraDayDataViewModel IntraDayDataVM { get; set; }
        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void SubscribeData() => IntraDayDataVM.SubscribeLiveData();
        public void UnSubscribeData() => IntraDayDataVM.UnSubscribeLiveData();
        public void SaveData(DateTime strikeTime) => IntraDayDataVM.SaveData(strikeTime);

        private void onSelectedStrikeChanged(object? sender, SelectedStrikeChangedEventArgs e)
        {
            updateCallPutData(e.SelectedStrike);
        }

        private void updateCallPutData(int selectedStrike)
        {
            var callStrike = _tradingDetails.StopLossDataDetails.Values.First(
                                x => x.Strike == selectedStrike && x.OptionType == OptionType.Call);

            var putStrike = _tradingDetails.StopLossDataDetails.Values.First(
                                x => x.Strike == selectedStrike && x.OptionType == OptionType.Put);

            Call = new StopLossDataViewModel(callStrike);
            Put = new StopLossDataViewModel(putStrike);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Call)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Put)));
        }
    }
}
