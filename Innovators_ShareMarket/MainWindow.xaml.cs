using Innovators_ShareMarket.ViewModels;
using System.Windows;

namespace Innovators_ShareMarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowVM;
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowVM = new MainWindowViewModel();
            DataContext = _mainWindowVM;
        }
    }
}