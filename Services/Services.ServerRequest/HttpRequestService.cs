using Newtonsoft.Json;

namespace Services.ServerRequest
{
    public class HttpRequestService : IDisposable
    {
        private const string USERAGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
        private static readonly HttpClient _httpClient;

        static HttpRequestService()
        {
            _httpClient = new HttpClient();
            updateDefaultHeaderDetails();
        }
        ~HttpRequestService()
        {
            _httpClient?.Dispose();
        }

        public static async Task<T?> GetMasterContracts<T>(string url) where T : class
        {
            using var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonString);

        }

        private static void updateDefaultHeaderDetails()
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
