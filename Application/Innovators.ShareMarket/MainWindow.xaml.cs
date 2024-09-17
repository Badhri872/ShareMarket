using Innovators_ShareMarket.ViewModels;
using System.Runtime.InteropServices;
using System.Windows;

namespace Innovators_ShareMarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Import the SetThreadExecutionState function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint SetThreadExecutionState(uint esFlags);

        // Execution state flags
        const uint ES_CONTINUOUS = 0x80000000;
        const uint ES_SYSTEM_REQUIRED = 0x00000001;
        const uint ES_AWAYMODE_REQUIRED = 0x00000040; // Optional

        private readonly MainWindowViewModel _mainWindowVM;
        public MainWindow()
        {
            // Prevent the system from sleeping while the task is running
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED);

            InitializeComponent();
            _mainWindowVM = new MainWindowViewModel();
            DataContext = _mainWindowVM;
            Closed += onWindowClose;
        }

        private void onWindowClose(object? sender, EventArgs e)
        {
            // Allow the system to sleep again
            SetThreadExecutionState(ES_CONTINUOUS);
        }
    }
}