using Innovators_ShareMarket.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MoreLinq;
using System.Windows;

namespace Innovators_ShareMarket.ViewModels
{
    public class StrikeDataViewModel : INotifyPropertyChanged
    {
        private readonly OptionTradingDetails _optionTradingDetails;
        private readonly IEnumerable<LiveData> _liveDataCollection;
        private readonly object _lockObject = new object();
        private bool _isSubscribed, _isUnsubscribed, _canSave;

        public StrikeDataViewModel(
                    OptionTradingDetails optionTradingDetails,
                    IEnumerable<LiveData> liveDataCollection)
        {
            _optionTradingDetails = optionTradingDetails;
            _liveDataCollection = liveDataCollection;

            CallPutCollection = new ObservableCollection<CallPutStopLossDataViewModel>
            {
                new CallPutStopLossDataViewModel(optionTradingDetails, liveDataCollection),
            };
            var niftyData = new SpotData(liveDataCollection.First(item
                                    => item.Exchange.ToLower().Contains("nifty")));
            var futuresData = new SpotData(liveDataCollection.First(item
                                    => item.Exchange.ToLower().Contains("futures")));

            NiftyData = new SymbolDataViewModel(niftyData);
            FuturesData = new SymbolDataViewModel(futuresData);
            CallPutModificationVM = new CallPutModificationViewModel(
                                            _optionTradingDetails,
                                            addCallPutData,
                                            removeCallPutData);
        }

        public bool IsExpanded { get; set; }
        public DateTime StrikeTime { get; init; }
        public DateTime MaxStrikeTime => StrikeTime.AddMinutes(15);
        public ObservableCollection<CallPutStopLossDataViewModel> CallPutCollection { get; set; }
        public CallPutModificationViewModel CallPutModificationVM { get; set; }
        public SymbolDataViewModel NiftyData { get; set; }
        public SymbolDataViewModel FuturesData { get; set; }
        public event EventHandler<CollectionChangedEventArgs> CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void DisplayData(DateTime dateTime)
        {
            lock (_lockObject)
            {
                /* if (!_isUnsubscribed)
                 {
                     if (!_isSubscribed && dateTime >= StrikeTime && dateTime < MaxStrikeTime)
                     {
                         CallPutCollection.ForEach(item => item.SubscribeData());
                         _isSubscribed = true;
                         _canSave = true;
                         IsExpanded = true;
                         Application.Current.Dispatcher.Invoke(()
                         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded))));
                     }
                     else if (_isSubscribed)
                     {
                         CallPutCollection.ForEach(item => item.UnSubscribeData());
                         _isUnsubscribed = true;
                     }
                     else
                     {
                         _isUnsubscribed = true;
                     }
                 }
                 else if (_canSave)
                 {
                     CallPutCollection[0].SaveData(StrikeTime);
                     _canSave = false;
                 }*/
                if (dateTime >= StrikeTime && dateTime < MaxStrikeTime)
                {
                    CallPutCollection.ForEach(item => item.SubscribeData());
                    _isSubscribed = true;
                    IsExpanded = true;
                    Application.Current.Dispatcher.Invoke(()
                    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded))));
                }
                else if (_isSubscribed)
                {
                    CallPutCollection.ForEach(item => item.UnSubscribeData());
                    CallPutCollection[0].SaveData(StrikeTime);
                }

            }
        }
        public void UpdateCollection(IEnumerable<CallPutStopLossDataViewModel> callPutCollection)
        {
            CallPutCollection = new ObservableCollection<CallPutStopLossDataViewModel>(callPutCollection);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallPutCollection)));
        }

        private void addCallPutData(int selectedStrike)
        {
            CallPutCollection.Add(new CallPutStopLossDataViewModel(_optionTradingDetails, _liveDataCollection, selectedStrike));
            CollectionChanged?.Invoke(this, new CollectionChangedEventArgs(CallPutCollection.ToList()));
        }

        private void removeCallPutData()
        {
            var selectedData = CallPutCollection.FirstOrDefault(item => item.IsSelected);
            if (selectedData != null)
            {
                CallPutCollection.Remove(selectedData);
                CollectionChanged?.Invoke(this, new CollectionChangedEventArgs(CallPutCollection.ToList()));
            }
        }
    }
}
