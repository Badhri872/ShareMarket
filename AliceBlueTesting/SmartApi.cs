using System;
using System.IO;
using System.Net;
using System.Text;

namespace AliceBlueTesting
{
    public class SmartApi
    {
        private readonly string _jwtToken, _refreshToken;
        protected string USER = "USER", SourceID = "WEB", PrivateKey = "";
        static string ClientPublicIP = "", ClientLocalIP = "", MACAddress = "";

        //protected string APIURL = "https://openapisuat.angelbroking.com";
        protected string APIURL = "https://apiconnect.angelbroking.com"; //prod endpoint

        //AngelToken Token { get; set; }

        /*Constructors*/
        public SmartApi(string _PrivateKey)
        {
            PrivateKey = _PrivateKey;
            ClientPublicIP = Helpers.GetPublicIPAddress();
            if (ClientPublicIP == "")
                ClientPublicIP = Helpers.GetPublicIPAddress();

            if (ClientPublicIP == "")
                ClientPublicIP = "106.193.147.98";

            ClientLocalIP = Helpers.GetLocalIPAddress();

            if (ClientLocalIP == "")
                ClientLocalIP = "127.0.0.1";

            if (Helpers.GetMacAddress() != null)
                MACAddress = Helpers.GetMacAddress().ToString();
            else
                MACAddress = "fe80::216e:6507:4b90:3719";

        }
        public SmartApi(string _PrivateKey, string jwtToken = "", string refreshToken = "")
        {
            PrivateKey = _PrivateKey;

            _jwtToken = jwtToken;
            _refreshToken = refreshToken;
        }

        /*Historical Data*/
        public string GetCandleData()
        {
            string json = string.Empty;
            try
            {
                string URL = APIURL + "/rest/secure/angelbroking/historical/v1/getCandleData";

                string PostData = "{\r\n \"exchange\": \"NSE\",\r\n \"symboltoken\": \"3045\",\r\n \"interval\": \"ONE_MINUTE\",\r\n \"fromdate\": \"2021-02-08 09:00\",\r\n \"todate\": \"2021-02-08 09:16\"\r\n}";

                json = POSTWebRequest(URL, PostData);
                if (!json.Contains("PostError:"))
                {
                    return json;
                }
            }
            catch (Exception ex)
            {
                
            }
            return json;
        }

        /* Makes a POST request */
        private string POSTWebRequest(string URL, string Data)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                HttpWebRequest httpWebRequest = null;
                httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + _jwtToken);
                httpWebRequest.Headers.Add("X-Content-Type-Options", "nosniff");
                httpWebRequest.Headers.Add("X-UserType", USER);
                httpWebRequest.Headers.Add("X-SourceID", SourceID);
                httpWebRequest.Headers.Add("X-ClientLocalIP", ClientLocalIP);
                httpWebRequest.Headers.Add("X-ClientPublicIP", ClientPublicIP);
                httpWebRequest.Headers.Add("X-MACAddress", MACAddress);
                httpWebRequest.Headers.Add("X-PrivateKey", PrivateKey);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                byte[] byteArray = Encoding.UTF8.GetBytes(Data);
                httpWebRequest.ContentLength = byteArray.Length;
                string Json = "";

                Stream dataStream = httpWebRequest.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                WebResponse response = httpWebRequest.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    Json = reader.ReadToEnd();
                }
                return Json;
            }
            catch (Exception ex)
            {
                return "PostError:" + ex.Message;
            }
        }
    }
}