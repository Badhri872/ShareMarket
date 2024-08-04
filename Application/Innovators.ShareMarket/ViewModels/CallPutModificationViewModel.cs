using Innovators_ShareMarket.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Innovators_ShareMarket.ViewModels
{
    public class CallPutModificationViewModel : INotifyPropertyChanged
    {
        private int _selectedItem;
        private Action<int> _addItem;
        public CallPutModificationViewModel(
                    OptionTradingDetails tradeDetails, 
                    Action<int> add, 
                    Action remove)
        {
            _addItem = add;
            AddCommand = new SimpleCommand(addItem);
            DeleteCommand = new SimpleCommand(remove);
            var strikeCollection = new HashSet<int>(tradeDetails.StopLossDataDetails.Values.Select(x => x.Strike));
            StrikeCollection = new ObservableCollection<int>(strikeCollection);
            _selectedItem = StrikeCollection[0];
        }
        public ObservableCollection<int> StrikeCollection { get; set; }
        public int SelectedStrike
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrike)));
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void addItem()
        {
            _addItem(_selectedItem);
        }
    }
}
