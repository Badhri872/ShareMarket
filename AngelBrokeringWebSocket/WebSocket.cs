using System.Net.WebSockets;
using System.Text.Json;
using Websocket.Client;

namespace Services.AngelWebSocket
{
    public class WebSocket
    {
        WebsocketClient _ws;
        string _url = "wss://smartapisocket.angelone.in/smart-stream?";

        public event EventHandler<MessageEventArgs> MessageReceived;

        public WebSocket()
        {

        }
        public bool IsConnected()
        {
            if (_ws is null)
                return false;

            return _ws.IsStarted;
        }
        public void ConnectforStockQuote(string feedtoken, string clientcode, string apiKey, string authToken)
        {
            try
            {
                if (feedtoken != "" && clientcode != "")
                {
                    var url = new Uri(_url + "clientCode=" + clientcode + "feedToken=" + feedtoken + "apiKey=" + apiKey);
                    _ws = new WebsocketClient(url, () => 
                                getClientWebSocket(clientcode, feedtoken, apiKey, authToken));
                    _ws.MessageReceived.Subscribe(msg => 
                        MessageReceived?.Invoke(
                            this, new MessageEventArgs(msg.Binary.GetLTPData().ToString())));
                    _ws.Start();
                    int i = 0;
                    do
                    {
                        HeartBeat(feedtoken, clientcode);
                        Thread.Sleep(60);
                        i++;
                    } while (i < 10);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ClientWebSocket getClientWebSocket(string clientCode, string feedToken, string apiKey, string authToken)
        {           
            var client = new ClientWebSocket();
            client.Options.SetRequestHeader("Authorization", authToken);
            client.Options.SetRequestHeader("x-api-key", apiKey);
            client.Options.SetRequestHeader("x-client-code", clientCode);
            client.Options.SetRequestHeader("x-feed-token", feedToken);
            return client;
        }

        public void Send(LTPRequest request)
        {

            if (_ws.IsStarted)
            {
                try
                {
                    var json = 
                        JsonSerializer
                        .Serialize(
                            request,
                            new JsonSerializerOptions
                            {
                                WriteIndented = true,
                            });
                    var send = _ws.Send(json);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void HeartBeat(string feedtoken, string clientcode)
        {
            string hbmsg = "{\"task\":\"hb\",\"channel\":\"\",\"token\":\"" + feedtoken + "\",\"user\": \"" + clientcode + "\",\"acctid\":\"" + clientcode + "\"}";
            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(hbmsg);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Close(bool Abort = false)
        {
            if (_ws.IsRunning)
            {
                if (Abort)
                    _ws.Stop(WebSocketCloseStatus.NormalClosure, "Close");
                else
                {
                    _ws.Dispose();
                }
            }
        }
    }
}