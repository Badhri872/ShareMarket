using Innovators_ShareMarket.Views;
using Newtonsoft.Json;
using Services.AngelWebSocket;
using Services.Database;
using Services.ServerRequest;
using System.ComponentModel;
using OathNet;
using System.Globalization;
using MoreLinq;

namespace Innovators_ShareMarket.Models
{
    public class ShareMarketConnection
    {
        private const string CONTRACTMASTER = 
            "https://margincalculator.angelbroking.com/OpenAPI_File/files/OpenAPIScripMaster.json";
        private const string APIKEY = "lr4liHKm";
        private const string USERID = "A1208575";
        private const string Password = "3005";
        private string _cachedTokenFile = $"cached_token_{DateTime.Now.ToString("dd_mmm_yyyy")}.txt";
        private OptionTradingDetails _optionTrading;
        private readonly List<LiveData> _liveDataCollection;
        private readonly Action<OptionTradingDetails, List<LiveData>> _updateStrikeDetails;
        private readonly Action<DateTime> _displayData;
        private readonly ContractExpiryView _contractExpiry;
        private readonly SmartApi _smartApi;
        private readonly WebSocket _webSocket;
        private int _increment = 0;

        public ShareMarketConnection(
                Action<OptionTradingDetails, List<LiveData>> updateStrikeDetails,
                ContractExpiryView contractExpiry,
                Action<DateTime> displayData)
        {
            _updateStrikeDetails = updateStrikeDetails;
            _displayData = displayData;
            _contractExpiry = contractExpiry;
            _liveDataCollection = new List<LiveData>();
            _smartApi = new SmartApi(APIKEY);
            _webSocket = new WebSocket();
            _optionTrading = new OptionTradingDetails();
        }

        ~ShareMarketConnection()
        {
            if (_webSocket != null)
            {
                _webSocket.MessageReceived -= writeResult;
                _webSocket.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void StartSocketConnection()
        {
            // Get totp
            var base32SecretKey = "SOQFEBU7TRR3YJUXDFIF7WESJU";
            var tOtp = new TimeBasedOtpGenerator(new Key(base32SecretKey), 6);
            var currentOTP = tOtp.GenerateOtp(DateTime.Now);

            // Login the angel broker
            var session = 
                _smartApi.GenerateSession(
                    USERID, 
                    Password, 
                    currentOTP);
            var token = session.TokenResponse;

            // Generate and get the token
            session = _smartApi.GenerateToken();
            token = session.TokenResponse;

            _webSocket.ConnectforStockQuote(
                token.feedToken, 
                USERID, 
                APIKEY, 
                token.jwtToken);
            var ltpReq =
                new LTPRequest(
                    correlationID: "sampleSubscription",
                    action: 1,
                    new LTPRequestParameters(
                        mode: 1,
                        tokenList: 
                            new List<ExchangeSubscriptionDetailsAngel>
                                {
                                    new ExchangeSubscriptionDetailsAngel(
                                        exchangeType: 2,
                                        tokens:
                                            _optionTrading
                                                .StrikeTokenDetails
                                                .Values
                                                .ToList()),
                                    new ExchangeSubscriptionDetailsAngel(
                                        exchangeType: 1,
                                        tokens: new List<string>
                                        {
                                            _optionTrading
                                                .NiftyToken,
                                            _optionTrading
                                                .FuturesToken,
                                        }),
                                }));
            _webSocket.MessageReceived += writeResult;
            _webSocket.Send(ltpReq);
        }

        public async void GetAndSaveShareMarketDetails()
        {
            var masterData = 
                await HttpRequestService
                    .GetMasterContracts<List<TokenData>>(
                        CONTRACTMASTER);
            
            var contractDetails = 
                await HttpRequestService
                    .GetMasterContracts<NiftyStrikeData>(
                        "https://alagu-chandran.github.io/nifty/NIFTY.json");
            createAndSaveJson(masterData, contractDetails);
        }

        public void GetAndSaveShareMarketDetails(string baseData)
        {
            _optionTrading =
                JsonConvert.DeserializeObject<OptionTradingDetails>(baseData) ?? 
                new OptionTradingDetails();
            _liveDataCollection.Clear();

            // Adding Nifty and Futures live data.
            _liveDataCollection.Add(new LiveData
            {
                Exchange = "Nifty",
                Token = int.Parse(_optionTrading.NiftyToken),
            });
            _liveDataCollection.Add(new LiveData
            {
                Exchange = "Futures",
                Token = int.Parse(_optionTrading.FuturesToken),
            });

            //Add Strike Details
            _optionTrading.StrikeTokenDetails.ForEach(
                strike => _liveDataCollection.Add(
                    new LiveData(strike.Key, strike.Value)));

            _updateStrikeDetails(_optionTrading, _liveDataCollection);

        }

        private async void writeResult(
            object? sender, 
            MessageEventArgs e)
        {
            await Task.Run(() =>
            {
                _displayData(e.Message.ExchangeTimestamp);
                _liveDataCollection.ForEach(item =>
                {
                    if (item.Token == int.Parse(e.Message.Token))
                    {
                        item.DateTime = e.Message.ExchangeTimestamp;
                        item.LastTradingPrice = double.Parse(e.Message.LTP.ToString());
                    }
                });
            });
        }

        private void createAndSaveJson(
                        List<TokenData> masterData,
                        NiftyStrikeData contractDetails)
        {

            _optionTrading = new OptionTradingDetails();

            var contractExpiryDate = 
                _contractExpiry.SelectedDate.GetValueOrDefault();
            _optionTrading.NiftyToken =
                masterData.FirstOrDefault(item =>
                    item.Symbol.Contains("Nifty 50"))
                .Token;
            _optionTrading.FuturesToken =
                masterData.FirstOrDefault(item =>
                    item.Symbol.Contains("NIFTYFUTURESMIDMONTH"))
                .Token;
            _liveDataCollection.Add(new LiveData
            {
                Exchange = "Nifty",
                Token = int.Parse(_optionTrading.NiftyToken),
            });
            _liveDataCollection.Add(new LiveData
            {
                Exchange = "Futures",
                Token = int.Parse(_optionTrading.FuturesToken),
            });
            var nfoContracts = masterData.Where(item => 
                item.Symbol.Contains("NIFTY" +
                contractExpiryDate.Day.ToString() +
                contractExpiryDate
                    .ToString("MMM", CultureInfo.InvariantCulture)
                    .ToUpper() +
                contractExpiryDate.ToString("yy")))
            .ToList();
            nfoContracts.ForEach(item =>
            {
                var strikePrice = 
                    (int)Math.Round(double.Parse(item.Strike)) / 100;
                if (strikePrice <= _contractExpiry.InitialStrike + 350 &&
                    strikePrice >= _contractExpiry.InitialStrike - 350)
                {
                    string strike = string.Empty;
                    if (item.Symbol.Contains("CE"))
                    {
                        strike = "Call" + strikePrice;
                    }
                    else
                    {
                        strike = "Put" + strikePrice;
                    }

                    var token = item.Token;
                    _optionTrading.StrikeTokenDetails.Add(strike, token);
                }
            });
            var tokenStrikes = 
                _optionTrading.StrikeTokenDetails.Keys.ToList();

            contractDetails.Active.ForEach(item =>
            {
                if (tokenStrikes.Contains(item.OptionType + item.StrikePrice))
                {
                    _optionTrading.StopLossDataDetails.Add(item.OptionType + item.StrikePrice,
                            new StopLossData
                            {
                                Strike = int.Parse(item.StrikePrice.ToString()),
                                OptionType = (OptionType)Enum.Parse(typeof(OptionType), item.OptionType),
                                PreviousClose = item.PrevClose,
                            });
                    _liveDataCollection.Add(new LiveData
                    {
                        Strike = int.Parse(item.StrikePrice.ToString()),
                        Option = (OptionType)Enum.Parse(typeof(OptionType), item.OptionType),
                        Token = int.Parse(_optionTrading
                                            .StrikeTokenDetails[item.OptionType + item.StrikePrice]),
                    });
                }
            });
            var jsonString = JsonConvert.SerializeObject(_optionTrading);
            Database.SaveData("basedata", jsonString);

            _updateStrikeDetails(_optionTrading, _liveDataCollection);
        }
    }
}
