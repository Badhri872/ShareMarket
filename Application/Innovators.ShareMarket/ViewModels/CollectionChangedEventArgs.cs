namespace Innovators_ShareMarket.ViewModels
{
    public class CollectionChangedEventArgs : EventArgs
    {
        public CollectionChangedEventArgs(List<CallPutStopLossDataViewModel> callPutData)
        {
            CallPutStopLossData = callPutData;
        }
        public IEnumerable<CallPutStopLossDataViewModel> CallPutStopLossData { get; }
    }
}
