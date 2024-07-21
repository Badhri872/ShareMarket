using Newtonsoft.Json;

namespace Services.ServerRequest
{
    public class HttpRequestService : IDisposable
    {
        private const string USERAGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
        private readonly HttpClient _httpClient;

        public HttpRequestService()
        {
            _httpClient = new HttpClient();
            updateDefaultHeaderDetails();
        }

        public async Task<T> GetMasterContracts<T>(string url) where T : class
        {
            T content = null;
            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                content = JsonConvert.DeserializeObject<T>(jsonString);
            }
            return content;
        }

        public async Task<List<T>> GetMasterContractDetails<T>(string url) where T : class
        {
            List<T> content = new List<T>();

            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                content = JsonConvert.DeserializeObject<List<T>>(
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString)["active"].ToString());
                
            }
            return content;
        }

        public async 

        private void updateDefaultHeaderDetails()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", USERAGENT);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
