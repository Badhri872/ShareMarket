using Innovators_ShareMarket.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Innovators_ShareMarket.Views
{
    /// <summary>
    /// Interaction logic for ContractExpiryView.xaml
    /// </summary>
    public partial class ContractExpiryView : Window, INotifyPropertyChanged
    {
        private DateTime? _selectedDate;
        private int _initialStrike;

        public ContractExpiryView()
        {
            InitializeComponent();
            DataContext = this;
        }
        public DateTime? SelectedDate 
        { 
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    onPropertyChanged();
                }
            }
        }
        public ICommand UpdateStrikeData { get; private set; }

        public int InitialStrike
        {
            get => _initialStrike;
            set
            {
                if (value != _initialStrike)
                {
                    _initialStrike = value; 
                    onPropertyChanged();
                }
            }
        }
        public bool IsOk => InitialStrike != 0 && SelectedDate.HasValue;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void ShowAndUpdateStrikeData(Action getAndUpdateStrikeData)
        {
            UpdateStrikeData = new SimpleCommand(getAndUpdateStrikeData);
            ShowDialog();
        }
        private void onPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOk)));
        }
    }
}
