using MoreLinq;
using Innovators_ShareMarket.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Newtonsoft.Json;
using Services.Database;

namespace Innovators_ShareMarket.ViewModels
{
    public class IntraDayDataViewModel : INotifyPropertyChanged
    {
        private FifteenMinutesData _call;
        private FifteenMinutesData _put;
        private int _selectedStrike;
        private readonly IEnumerable<LiveData> _liveDataCollection;
        private readonly List<FifteenMinutesData> _fifteenMinutesDataCollection;
        private bool _isDataSaved;
        public IntraDayDataViewModel(
                        OptionTradingDetails optionTradingDetails,
                        IEnumerable<LiveData> liveDataCollection)
        {
            _liveDataCollection = liveDataCollection;
            _fifteenMinutesDataCollection = new List<FifteenMinutesData>();

            var strikes = new HashSet<int>(optionTradingDetails.StopLossDataDetails.Values
                                            .Select(item => item.Strike));
            StrikeCollection = new ObservableCollection<int>(strikes);
            SelectedStrike = StrikeCollection[0];
            //subscribeLiveData();
            updateLiveDataPositions();
            updateStrikes();
            CallOpen = _call.Open;
            CallHigh = _call.High;
            CallLow = _call.Low;
            CallClose = _call.Close;
            CallStrike = _call.Option.ToString();

            PutOpen = _put.Open;
            PutHigh = _put.High;
            PutLow = _put.Low;
            PutClose = _put.Close;
            PutStrike = _put.Option.ToString();
        }

        public IntraDayDataViewModel(
                       OptionTradingDetails optionTradingDetails,
                       IEnumerable<LiveData> liveDataCollection,
                       int selectedStrike)
        {
            _liveDataCollection = liveDataCollection;
            var strikes = new HashSet<int>(optionTradingDetails.StopLossDataDetails.Values
                                            .Select(item => item.Strike));
            StrikeCollection = new ObservableCollection<int>(strikes);
            SelectedStrike = selectedStrike;
            updateLiveDataPositions();
            updateStrikes();
            CallOpen = _call.Open;
            CallHigh = _call.High;
            CallLow = _call.Low;
            CallClose = _call.Close;
            CallStrike = _call.Option.ToString();

            PutOpen = _put.Open;
            PutHigh = _put.High;
            PutLow = _put.Low;
            PutClose = _put.Close;
            PutStrike = _put.Option.ToString();
        }

        public double CallOpen { get => _call.Open; set => _call.Open = value; }
        public double CallHigh { get => _call.High; set => _call.High = value; }
        public double CallLow { get => _call.Low; set => _call.Low = value; }
        public double CallClose { get => _call.Close; set => _call.Close = value; }
        public string CallStrike { get => _call.Option.ToString(); set => _call.Option = (OptionType)Enum.Parse(typeof(OptionType), value); }
        public double PutOpen { get => _put.Open; set => _put.Open = value; }
        public double PutHigh { get => _put.High; set => _put.High = value; }
        public double PutLow { get => _put.Low; set => _put.Low = value; }
        public double PutClose { get => _put.Close; set => _put.Close = value; }
        public string PutStrike { get => _put.Option.ToString(); set => _put.Option = (OptionType)Enum.Parse(typeof(OptionType), value); }

        public ObservableCollection<int> StrikeCollection { get; set; }
        public int SelectedStrike
        {
            get
            {
                return _selectedStrike;
            }
            set
            {
                if (_selectedStrike != value)
                {
                    _selectedStrike = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrike)));
                    SelectedStrikeChanged?.Invoke(this, new SelectedStrikeChangedEventArgs(value));
                    updateStrikes();
                    notifyPropertyChange();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<SelectedStrikeChangedEventArgs> SelectedStrikeChanged;

        public void SubscribeLiveData()
        {
            _liveDataCollection.ForEach(liveData =>
            {
                if (string.IsNullOrEmpty(liveData.Exchange))
                {
                    liveData.LiveDataChanged += onLiveDataChanged;
                }
            });
            notifyPropertyChange();
        }
        public void UnSubscribeLiveData()
        {
            _liveDataCollection.ForEach(liveData => liveData.LiveDataChanged -= onLiveDataChanged);
            notifyPropertyChange();
        }

        public void SaveData(DateTime strikeTime)
        {
            string time = strikeTime.ToShortTimeString().Replace(":", "_").Replace(" ", "");
            if (!_isDataSaved && _fifteenMinutesDataCollection.Any())
            {
                var jsonString = JsonConvert.SerializeObject(_fifteenMinutesDataCollection);
                Database.SaveData(time, jsonString);
                _isDataSaved = true;
            }
        }

        private void onLiveDataChanged(object? sender, LiveDataChangedEventArgs e)
        {
            var changedLiveData = _fifteenMinutesDataCollection.First(item
                                        => item.Strike == e.Strike && item.Option == e.Option);
            changedLiveData.Close = e.LTP;

            if (changedLiveData.Strike == SelectedStrike)
            {
                notifyPropertyChange();
            }
        }

        private void updateLiveDataPositions()
        {
            _liveDataCollection.ForEach(liveData =>
            {
                if (liveData.Exchange == null)
                {
                    var fifteenMinutesData = new FifteenMinutesData
                    {
                        Close = liveData.LastTradingPrice,
                        Strike = liveData.Strike,
                        Option = liveData.Option,
                    };
                    _fifteenMinutesDataCollection.Add(fifteenMinutesData);
                }
            });
        }

        private void updateStrikes()
        {
            if (_fifteenMinutesDataCollection.Count > 0)
            {

                _call = _fifteenMinutesDataCollection.First(data
                                            => data.Strike == SelectedStrike && data.Option == OptionType.Call);
                _put = _fifteenMinutesDataCollection.First(data
                                            => data.Strike == SelectedStrike && data.Option == OptionType.Put);
                notifyPropertyChange();
            }
        }

        private void notifyPropertyChange()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallOpen)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallHigh)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallLow)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallClose)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PutOpen)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PutHigh)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PutLow)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PutClose)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallStrike)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PutStrike)));
            });
        }
    }
}
