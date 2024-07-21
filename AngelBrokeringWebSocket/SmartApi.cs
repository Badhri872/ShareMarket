using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Services.AngelWebSocket.Helpers;

namespace Services.AngelWebSocket
{
    public class SmartApi
    {
        private readonly string _privateKey;
        private const string PUBLISHERLOGIN = "https://smartapi.angelbroking.com/publisher-login?api_key=";
        private const string APIURL = "https://apiconnect.angelbroking.com";
        public SmartApi(string apiKey) 
        {
            _privateKey = apiKey;
            ClientPublicAddress = GetPublicIPAddress();
            ClientLocalAddress = GetLocalIPAddress();
            MACAddress = GetMacAddress();
        }

        public string ClientPublicAddress { get; }
        public string ClientLocalAddress { get; }
        public string MACAddress { get; }
        public AngelToken Token { get; private set; }

        public OutputResponse GenerateSession(string clientCode, string password, string totp)
        {
            var response = new OutputResponse();
            try
            {
                var url = APIURL + "/rest/auth/angelbroking/user/v1/loginByPassword";
                var data = "{\"clientcode\":\"" + clientCode + "\",\"password\":\"" + password + "\",\"totp\":\""+totp+"\"}";
                var json = postWebRequest(null, url, data);
                if (json.Contains("PostError:"))
                {
                    response.Status = false;
                    response.HttpCode = "404";
                    response.HttpError = json.Replace("PostError:", "");
                }
                else
                {
                    var angelResponse = JsonConvert.DeserializeObject<AngelTokenResponse>(json);
                    response.TokenResponse = angelResponse.data;
                    response.Status = angelResponse.status;
                    response.HttpCode = angelResponse.errorcode;
                    response.HttpError = angelResponse.message;
                    Token = angelResponse.data;
                }
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.HttpCode = "404";
                response.HttpError = ex.Message;
            }
            return response;
        }

        public OutputResponse GenerateToken()
        {
            var response = new OutputResponse();
            try
            {
                if (validateToken())
                {

                    var url = APIURL + "/rest/auth/angelbroking/jwt/v1/generateTokens";
                    var data = "{\"refreshToken\":\"" + Token.refreshToken + "\"}";

                    var json = postWebRequest(Token, url, data);
                    if (json.Contains("PostError:"))
                    {
                        response.Status = false;
                        response.HttpCode = "404";
                        response.HttpError = json.Replace("PostError:", "");
                    }
                    else
                    {
                        var angelResponse = JsonConvert.DeserializeObject<AngelTokenResponse>(json);
                        response.TokenResponse = angelResponse.data;
                        response.Status = angelResponse.status;
                        response.HttpCode = angelResponse.errorcode;
                        response.HttpError = angelResponse.message;
                        Token = angelResponse.data;
                    }
                }
                else
                {
                    response.Status = false;
                    response.HttpCode = "404";
                    response.HttpError = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.HttpCode = "404";
                response.HttpError = ex.Message;
            }
            return response;
        }

        private string postWebRequest(AngelToken token, string url, string data)
        {
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                
                setHttpRequestProperties(token, httpRequest);

                var byteArray = Encoding.UTF8.GetBytes(data);
                httpRequest.ContentLength = byteArray.Length;

                using var dataStream = httpRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                return getResponse(httpRequest);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private string getWebRequest(AngelToken token, string url)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            setHttpRequestProperties(token, httpRequest);
            StreamReader stringReader;
            return getResponse(httpRequest);
        }

        private static string getResponse(HttpWebRequest httpRequest)
        {
            using var streamReader = new StreamReader(httpRequest.GetResponse().GetResponseStream());
            return streamReader.ReadToEnd();
        }

        private void setHttpRequestProperties(AngelToken token, HttpWebRequest httpRequest)
        {
            if (token is not null) 
                httpRequest.Headers.Add("Authorization", "Bearer " + token.jwtToken);
            httpRequest.Headers.Add("X-Content-Type-Options", "nosniff");
            httpRequest.Headers.Add("X-UserType", "USER");
            httpRequest.Headers.Add("X-SourceID", "WEB");
            httpRequest.Headers.Add("X-ClientLocalIP", ClientLocalAddress);
            httpRequest.Headers.Add("X-ClientPublicIP", ClientPublicAddress);
            httpRequest.Headers.Add("X-MACAddress", MACAddress);
            httpRequest.Headers.Add("X-PrivateKey", _privateKey);

            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
        }

        private bool validateToken()
            => Token != null &&
            !string.IsNullOrEmpty(Token.jwtToken) &&
            !string.IsNullOrEmpty(Token.jwtToken);
    }
}
