using Innovators_ShareMarket.Views;
using Newtonsoft.Json;
using Services.Database;
using Services.ServerRequest;
using Services.WebSocket;
using Services.WebSocket.Records;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace Innovators_ShareMarket.Models
{
    public class ShareMarketConnection
    {
        private const string CONTRACTMASTER = "https://v2api.aliceblueonline.com/restpy/contract_master?exch={EXCHANGE}";
        private const string APIKEY = "RzN6qDharOE9Ang24i35TLdKE8E9jVaWUrcW926fxzNzbWC5KUbljCpr1NuaCgNaQmZUCM62MtkFsSzvb7Fzez8gqxrtpmjAnIVVspSnpeDPVL6AqZ65z6XEAe7cfVg0"; //Environment.GetEnvironmentVariable("apiKey");
        private const string USERID = "1008015";
        private AliceBlue _aliceBlue;
        private Ticker _ticker;
        private string _cachedTokenFile = $"cached_token_{DateTime.Now.ToString("dd_mmm_yyyy")}.txt";
        private OptionTradingDetails _optionTrading;
        private readonly List<LiveData> _liveDataCollection;
        private readonly HttpRequestService _httpService;
        private readonly Action<OptionTradingDetails, List<LiveData>> _updateStrikeDetails;
        private readonly Action<DateTime> _displayData;
        private readonly ContractExpiryView _contractExpiry;
        private readonly List<Ticker> _tickerCollection;
        System.Timers.Timer _timer;

        public ShareMarketConnection(
                Action<OptionTradingDetails, List<LiveData>> updateStrikeDetails,
                ContractExpiryView contractExpiry,
                Action<DateTime> displayData)
        {
            _updateStrikeDetails = updateStrikeDetails;
            _displayData = displayData;
            _contractExpiry = contractExpiry;
            _liveDataCollection = new List<LiveData>();
            _httpService = new HttpRequestService();
            _tickerCollection = new List<Ticker>();

            _timer = new System.Timers.Timer();
            _timer.Elapsed += _onTimerTick;
            _timer.Interval = 5000;
        }

        private void _onTimerTick(object? sender, ElapsedEventArgs e)
        {
            _ticker.ReconnectTicker();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void StartSocketConnection()
        {
            try
            {
                _aliceBlue = new AliceBlue(USERID, APIKEY, enableLogging: true,
                    onAccessTokenGenerated: (accessToken) =>
                    {

                        File.WriteAllText(_cachedTokenFile, accessToken);
                    }, cachedAccessTokenProvider: () =>
                    {
                        return File.Exists(_cachedTokenFile) ? File.ReadAllText(_cachedTokenFile) : null;
                    });

                Task.Run(() =>
                {
                    _ticker.Close();
                    /*var tickerCollection = new List<Ticker>();
                    int i = 0;
                    while (i < 8)
                    {
                        tickerCollection.Add(_aliceBlue.CreateTicker());
                        i++;
                    }
                    return tickerCollection;*/
                    return _aliceBlue.CreateTicker();
                })
                    .ContinueWith(t =>
                    {
                        _ticker = t.Result;
                        // Setup event handlers
                        _ticker.OnTick += ticker_OnTick;
                        _ticker.OnConnect += ticker_OnConnect;
                        _ticker.OnClose += ticker_OnClose;
                        _ticker.OnError += ticker_OnError;
                        _ticker.OnNoReconnect += ticker_OnNoReconnect;
                        _ticker.OnReconnect += ticker_OnReconnect;
                        _ticker.OnReady += ticker_OnReady;

                        _ticker.EnableReconnect();

                        _ticker.Connect();
                        _timer.Start();

                        /*_tickerCollection.AddRange(t.Result);
                        // Setup event handlers
                        _tickerCollection.ForEach(ticker =>
                        {
                            ticker.OnTick += ticker_OnTick;
                            ticker.OnConnect += ticker_OnConnect;
                            ticker.OnClose += ticker_OnClose;
                            ticker.OnError += ticker_OnError;
                            ticker.OnNoReconnect += ticker_OnNoReconnect;
                            ticker.OnReconnect += ticker_OnReconnect;
                            ticker.OnReady += ticker_OnReady;

                            ticker.EnableReconnect();

                            ticker.Connect();
                        });*/
                    });

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        public async void GetAndSaveShareMarketDetails()
        {
            var nseDetails = await _httpService.GetMasterContracts<NSEDetails>(CONTRACTMASTER.Replace("{EXCHANGE}", "NSE"));
            var nfoDetails = await _httpService.GetMasterContracts<NFODetails>(CONTRACTMASTER.Replace("{EXCHANGE}", "NFO"));
            var contractDetails = await _httpService.GetMasterContractDetails<StrikeData>("https://alagu-chandran.github.io/nifty/NIFTY.json");
            createAndSaveJson(nseDetails, nfoDetails, contractDetails);
        }

        private void ticker_OnReady()
        {
            var spotSubscriptionList = new SubscriptionToken[2];
            spotSubscriptionList[0] = new SubscriptionToken
            {
                Token = int.Parse(_optionTrading.NiftyToken),
                Exchange = Constants.EXCHANGE_INDICES,
            };
            spotSubscriptionList[1] = new SubscriptionToken
            {
                Token = int.Parse(_optionTrading.FuturesToken),
                Exchange = Constants.EXCHANGE_NFO,
            };

            var subscriptionList = _optionTrading.StrikeTokenDetails.Values.ToList().Select(
                token => new SubscriptionToken
                {
                    Token = int.Parse(token),
                    Exchange = Constants.EXCHANGE_NFO,
                }).Concat(spotSubscriptionList).ToArray();
            _ticker.Subscribe(Constants.TICK_MODE_FULL,
                subscriptionList);
            /*var subscriptionList = _optionTrading.StrikeTokenDetails.Values.ToList().Select(
                token => new SubscriptionToken
                {
                    Token = int.Parse(token),
                    Exchange = Constants.EXCHANGE_NFO,
                }).ToArray();
            int tickerCount = 0;
            for (int i = 0; i < subscriptionList.Length; i += 5)
            {
                _tickerCollection[tickerCount].Subscribe(
                    Constants.TICK_MODE_FULL,
                    new SubscriptionToken[]
                    {
                        subscriptionList[i],
                        subscriptionList[i + 1]
                    });
                tickerCount++;
            }

            subscriptionList = new SubscriptionToken[2];
            subscriptionList[0] = new SubscriptionToken
            {
                Token = int.Parse(_optionTrading.NiftyToken),
                Exchange = Constants.EXCHANGE_INDICES,
            };
            subscriptionList[1] = new SubscriptionToken
            {
                Token = int.Parse(_optionTrading.FuturesToken),
                Exchange = Constants.EXCHANGE_NFO,
            };

            _tickerCollection[tickerCount].Subscribe(Constants.TICK_MODE_FULL,
                subscriptionList);*/

        }

        private async void ticker_OnTick(Tick TickData)
        {
            Debug.WriteLine(TickData.FeedTime.ToString());
            await Task.Run(() =>
            {
                _displayData(TickData.FeedTime.Value);
                _liveDataCollection.ForEach(item =>
                {
                    if (item.Token == TickData.Token)
                    {
                        item.DateTime = TickData.FeedTime.Value;
                        item.LastTradingPrice = double.Parse(TickData.LastTradedPrice.ToString());
                    }
                });
            });
        }

        private void ticker_OnReconnect()
        {
            //Console.WriteLine("Ticker reconnecting.");
        }

        private void ticker_OnNoReconnect()
        {
            //Console.WriteLine("Ticker not reconnected.");
        }

        private void ticker_OnError(string Message)
        {
            //Console.WriteLine("Ticker error." + Message);
        }

        private void ticker_OnClose()
        {
            //Console.WriteLine("Ticker closed.");
        }

        private void ticker_OnConnect()
        {
            //Console.WriteLine("Ticker connected.");
        }

        private void createAndSaveJson(
                        NSEDetails nseDetails,
                        NFODetails nfoDetails,
                        List<StrikeData> contractDetails)
        {

            _optionTrading = new OptionTradingDetails();

            var contractExpiryDate = _contractExpiry.SelectedDate.GetValueOrDefault();
            _optionTrading.NiftyToken = nseDetails.NSE.FirstOrDefault(item =>
                                            item.Symbol.Contains("NIFTY 50")).InstrumentToken;
            _optionTrading.FuturesToken = nfoDetails.NFO.FirstOrDefault(item
                                                    => item.InstrumentType == "FUTIDX" &&
                                                       item.Symbol == "NIFTY" &&
                                                       item.FormattedInstrumentName.Contains(contractExpiryDate.ToString("MMM").ToUpper()))
                                            .InstrumentToken;
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
            var nfoContracts = nfoDetails.NFO.Where(item => item.Symbol == "NIFTY" &&
                                                            item.GetTradeExpiry().Date.Day == contractExpiryDate.Day &&
                                                            item.GetTradeExpiry().Date.Month == contractExpiryDate.Month).ToList();
            nfoContracts.ForEach(item =>
            {
                var strikePrice = (int)Math.Round(double.Parse(item.StrikePrice));
                if (strikePrice <= _contractExpiry.InitialStrike + 350 &&
                    strikePrice >= _contractExpiry.InitialStrike - 350)
                {
                    string strike = string.Empty;
                    if (item.OptionType == "CE")
                    {
                        strike = "Call" + item.StrikePrice;
                    }
                    else
                    {
                        strike = "Put" + item.StrikePrice;
                    }

                    var token = item.InstrumentToken;
                    _optionTrading.StrikeTokenDetails.Add(strike, token);
                }
            });
            var tokenStrikes = _optionTrading.StrikeTokenDetails.Keys.ToList();

            contractDetails.ForEach(item =>
            {
                if (tokenStrikes.Contains(item.OptionType + item.StrikePrice))
                {
                    _optionTrading.StopLossDataDetails.Add(item.OptionType + item.StrikePrice,
                            new StopLossData
                            {
                                Strike = int.Parse(item.StrikePrice),
                                OptionType = (OptionType)Enum.Parse(typeof(OptionType), item.OptionType),
                                PreviousClose = double.Parse(item.PreviousClose),
                            });
                    _liveDataCollection.Add(new LiveData
                    {
                        Strike = int.Parse(item.StrikePrice),
                        Option = (OptionType)Enum.Parse(typeof(OptionType), item.OptionType),
                        Token = int.Parse(_optionTrading
                                            .StrikeTokenDetails[item.OptionType + item.StrikePrice]),
                    });
                }
            });
            var jsonString = JsonConvert.SerializeObject(_optionTrading);
            Database.SaveData("basedata", jsonString);
            //File.WriteAllText(@"E:\Badhri\Projects\ShareMarket\ShareMarket\Data\basedata.json", jsonString);

            _updateStrikeDetails(_optionTrading, _liveDataCollection);
        }
    }
}
