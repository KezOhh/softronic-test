using Newtonsoft.Json;

namespace Api.Domain
{
    public class BaseService
    {
        private IHttpClientFactory _httpClientFactory;
        private HttpClient _client { get => GetClient(); }
        private IConfiguration _config;

        public BaseService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        private HttpClient GetClient()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            client.BaseAddress = new Uri(_config.GetSection("ApiSettings")["Url"]);
            client.DefaultRequestHeaders.Add("X-Functions-Key", _config.GetSection("ApiSettings")["Key"]);
            return client;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string requestUri)
        {
            var response = await _client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Get request to {requestUri} failed with status code {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest request)
        {
            var content = CreateContent(request);

            var response = await _client.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Post request to {requestUri} failed with status code {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        private HttpContent CreateContent<TRequest>(TRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }
    }
}
