// See https://aka.ms/new-console-template for more information

using Services.AngelWebSocket;
using OathNet;

string Client_code = "A1208575"; 
string Password = "3005"; 
string api_key = "lr4liHKm";
int increment = 0;

// Establish connection with angel broker
var connect = new SmartApi(api_key);

// Get totp
var base32SecretKey = "SOQFEBU7TRR3YJUXDFIF7WESJU";
var tOtp = new TimeBasedOtpGenerator(new Key(base32SecretKey), 6);
var currentOTP = tOtp.GenerateOtp(DateTime.Now);

// Login the angel broker
var session = connect.GenerateSession(Client_code, Password, currentOTP);
var token = session.TokenResponse;

// Generate and get the token
session = connect.GenerateToken();
token = session.TokenResponse;

// Create WebSocket
var webSocket = new WebSocket();
var exitEvent = new ManualResetEvent(false);

webSocket.ConnectforStockQuote(token.feedToken, Client_code, api_key, token.jwtToken);
var ltpReq =
    new LTPRequest(
        correlationID: "sampleSubscription",
        action: 1,
        new LTPRequestParameters(
            mode: 1,
            tokenList: new List<ExchangeSubscriptionDetailsAngel>
                        {
                            new ExchangeSubscriptionDetailsAngel(
                                exchangeType: 2,
                                tokens: new List<string>
                                {
                                    "51794",
                                    "51795",
                                }),
                        }));
webSocket.MessageReceived += writeResult;
webSocket.Send(ltpReq);

void writeResult(object? sender, MessageEventArgs e)
{
    Console.WriteLine("Tick Received :" + e.Message);
    if(increment > 10)
    {
        webSocket.Close();
        exitEvent.Set();
    }
    else
    {
        increment++;
    }
}
exitEvent.WaitOne();
Console.WriteLine("Socket Communication Successful");
