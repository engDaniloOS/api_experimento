using Microsoft.Extensions.Caching.Memory;
using Polly;
using System.Net;
using System.Text.Json;
using WebApi.Domain.Dtos;
using WebApi.Domain.Providers;

namespace WebApi.DataProviders
{
    public class CarDataProvider : ICarDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public CarDataProvider(IMemoryCache cache, 
                               IConfiguration configuration,
                               HttpClient httpClient)
        {
            _cache = cache;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<BrandDto>> GetCarsBrandsData()
        {
            var url = _configuration.GetRequiredSection("Http")["Cars:Urls:Brands"];
            return await ExecuteHttpGetRequest<List<BrandDto>>(url);
        }

        public async Task<CarModelDto> GetCarsModelsDataByBrandId(string brandId)
        {
            var url = _configuration.GetRequiredSection("Http")["Cars:Urls:Models"]
                        .Replace("{brand_Id}", brandId);

            return await ExecuteHttpGetRequest<CarModelDto>(url);
        }

        private async Task<T> ExecuteHttpGetRequest<T>(string url)
        {
            var retryPolicy = Policy.Handle<HttpRequestException>()
                                    .RetryAsync(3,
                                        (exception, retryCount) => Console.WriteLine($"Fail on try {retryCount}. Trying again..."));

            var circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                                              .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                                                  onBreak: (exception, breakDelay) => Console.WriteLine($"Circuit Open! Awaiting {breakDelay.TotalSeconds} seconds to try again."),
                                                  onReset: () => Console.WriteLine("Circuit closed! The operations came back to normal."),
                                                  onHalfOpen: () => Console.WriteLine("Circuit half-open. Trying one new operation..."));

            var finalPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

            var json = await finalPolicy.ExecuteAsync(async () => await GetFromCacheOrExecuteRequest(url));

            return JsonSerializer.Deserialize<T>(json);
        }

        private async Task<string> GetFromCacheOrExecuteRequest(string url)
        {
            var cacheExpiresIn = _configuration.GetRequiredSection("Http")["Cars:CachePeriodInMinutes"];
            var cacheSize = _configuration.GetRequiredSection("Http")["Cars:CacheSizeInMb"];

            var cachedResponse = _cache.GetOrCreate(url, async fun =>
            {
                fun.AbsoluteExpiration = DateTime.Now.AddMinutes(long.Parse(cacheExpiresIn));
                fun.Size = long.Parse(cacheSize);

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                    throw new HttpRequestException();

                return await response.Content.ReadAsStringAsync();
            });

            return await cachedResponse;
        }
    }
}
