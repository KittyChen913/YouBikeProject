using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace YouBikeProject.Services
{
    public class HttpClientHelpers : IHttpClientHelpers
    {
        private readonly ILogger<HttpClientHelpers> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HttpClientHelpers(ILogger<HttpClientHelpers> logger, IHttpClientFactory _httpClientFactory, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._httpClientFactory = _httpClientFactory;
            this._logger = logger;
        }

        public async Task<string> GetAPI(string requestUrl)
        {
            string responseContent = string.Empty;
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return responseContent;
        }


    }
}