namespace Innovators_ShareMarket.Models
{
    public class SelectedStrikeChangedEventArgs : EventArgs
    {
        public SelectedStrikeChangedEventArgs(int selectedStrike) 
        {
            SelectedStrike = selectedStrike;
        }

        public int SelectedStrike { get; }
    }
}
