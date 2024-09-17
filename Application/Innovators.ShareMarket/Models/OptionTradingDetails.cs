namespace Innovators_ShareMarket.Models
{
    public class OptionTradingDetails
    {
        public OptionTradingDetails() 
        {
            StrikeTokenDetails = new Dictionary<string, string>();
            StopLossDataDetails = new Dictionary<string, StopLossData>();
        }
        public string NiftyToken { get; set; }
        public string FuturesToken { get; set; }

        public Dictionary<string,string> StrikeTokenDetails { get; set; }
        public Dictionary<string,StopLossData> StopLossDataDetails { get; set; }
        
    }
}
