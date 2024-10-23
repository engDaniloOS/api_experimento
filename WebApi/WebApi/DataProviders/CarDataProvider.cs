using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;
using WebApi.Configurations;
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
                               IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient(HttpClientServiceConfig.HTTP_CLIENT_DEFAULT);
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
            var cacheExpiresIn = _configuration.GetRequiredSection("Http")["Cars:CachePeriodInMinutes"];
            var cacheSize = _configuration.GetRequiredSection("Http")["Cars:CacheSizeInMb"];

            var cachedResponse = _cache.GetOrCreateAsync(url, async fun =>
            {
                fun.AbsoluteExpiration = DateTime.Now.AddMinutes(long.Parse(cacheExpiresIn));
                fun.Size = long.Parse(cacheSize);

                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(json);
            });

            return await cachedResponse;
        }
    }
}
