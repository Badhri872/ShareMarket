using Innovators_ShareMarket.Models;
using Innovators_ShareMarket.Views;
using MoreLinq;
using Services.Database;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Innovators_ShareMarket.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ShareMarketConnection _connection;
        private readonly DateTime _currentDateTime = DateTime.Now;
        private readonly ContractExpiryView _contractExpiryView;


        public MainWindowViewModel()
        {
            _contractExpiryView = new ContractExpiryView();
            _connection = new ShareMarketConnection(
                                updateStrikeDetails, 
                                _contractExpiryView,
                                displayData);
            StartConnection = 
                new SimpleCommand(
                    _connection.StartSocketConnection);
            SaveShareMarketDetails = 
                new SimpleCommand(
                    _connection.GetAndSaveShareMarketDetails);

            if (Database.Present("basedata"))
            {
                var json = Database.ReadJson("basedata");
                _connection.GetAndSaveShareMarketDetails(json); 
            }
            else
            {

                _contractExpiryView.ShowAndUpdateStrikeData(() =>
                {
                    _connection.GetAndSaveShareMarketDetails();
                    _contractExpiryView.Close();
                });
            }
        }

        public ICommand StartConnection { get; }
        public ICommand SaveShareMarketDetails { get; }
        public ObservableCollection<StrikeDataViewModel> StrikeCollection { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;



        private void updateStrikeDetails(
                        OptionTradingDetails optionTrading,
                        IEnumerable<LiveData> liveDataCollection)
        {
            var strikeTime = new List<DateTime>
            {
                createDateTime(9,15,"AM"),  //"9:15AM",
                createDateTime(9,30,"AM"),  //"9:30AM",
                createDateTime(9,45,"AM"),  //"9:45AM",
                createDateTime(10,00,"AM"), //"10:00AM",
                createDateTime(10,15,"AM"), //"10:15AM",
                createDateTime(10,30,"AM"), //"10:30AM",
                createDateTime(10,45,"AM"), //"10:45AM",
                createDateTime(11,00,"AM"), //"11:00AM",
                createDateTime(11,15,"AM"), //"11:15AM",
                createDateTime(11,30,"AM"), //"11:30AM",
                createDateTime(11,45,"AM"), //"11:45AM",
                createDateTime(12,00,"PM"), //"12:00PM",
                createDateTime(12,15,"PM"), //"12:15PM",
                createDateTime(12,30,"PM"), //"12:30PM",
                createDateTime(12,45,"PM"), //"12:45PM",
                createDateTime(01,00,"PM"), //"01:00PM",
                createDateTime(01,15,"PM"), //"01:15PM",
                createDateTime(01,30,"PM"), //"01:30PM",
                createDateTime(01,45,"PM"), //"01:45PM",
                createDateTime(02,00,"PM"), //"02:00PM",
                createDateTime(02,15,"PM"), //"02:15PM",
                createDateTime(02,30,"PM"), //"02:30PM",
                createDateTime(02,45,"PM"), //"02:45PM",
                createDateTime(03,00,"PM"), //"03:00PM",
                createDateTime(03,15,"PM"), //"03:15PM",
            };

            StrikeCollection = new ObservableCollection<StrikeDataViewModel>(
                                    createStrikeData(
                                        strikeTime,
                                        optionTrading,
                                        liveDataCollection));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrikeCollection)));
        }

        private DateTime createDateTime(int hours, int minutes, string meridiem)
            => new DateTime(
                        _currentDateTime.Year,
                        _currentDateTime.Month,
                        _currentDateTime.Day,
                        meridiem == "PM" && hours != 12 ? hours + 12 : hours,
                        minutes,
                        0);
        private IEnumerable<StrikeDataViewModel> createStrikeData(
            IEnumerable<DateTime> strikeTime,
            OptionTradingDetails optionTrading,
            IEnumerable<LiveData> liveDataCollection)
        {
            var strikeDataCollection = new LinkedList<StrikeDataViewModel>();
            foreach (var time in strikeTime)
            {
                var data = new StrikeDataViewModel(
                                    optionTrading,
                                    liveDataCollection)
                {
                    StrikeTime = time,
                };
                if (strikeDataCollection.Count == 0)
                {
                    strikeDataCollection.AddFirst(data);
                }
                else
                {
                    var previousNode = strikeDataCollection.Last.Value;
                    previousNode.CollectionChanged += (sender, args) =>
                    {
                        data.UpdateCollection(args.CallPutStopLossData);
                    };
                    _ = strikeDataCollection.AddAfter(strikeDataCollection.Last, data);

                }
            }
            return strikeDataCollection;
        }

        private void displayData(DateTime dateTime) 
            => StrikeCollection.ForEach(item => item.DisplayData(dateTime));
    }
}
