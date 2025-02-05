﻿using System.Net.WebSockets;
using System.Text;
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
        public void ConnectforStockQuote(string feedtoken, string clientcode, string apiKey = null)
        {
            try
            {
                if (feedtoken != "" && clientcode != "")
                {
                    var url = new Uri(_url + "clientCode=" + clientcode + "feedToken=" + feedtoken + "apiKey=" + apiKey);
                    _ws = new WebsocketClient(url);
                    _ws.MessageReceived.Subscribe(msg => 
                        MessageReceived?.Invoke(
                            this, new MessageEventArgs(msg.Text)));
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
        public void Send(LTPRequest request)
        {

            if (_ws.IsStarted)
            {
                try
                {
                    var json = JsonSerializer.Serialize(request);
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