using System.Text.Json;
using WebApi.Configurations;
using WebApi.DataProviders.Commons;
using WebApi.Domain.Dtos;
using WebApi.Domain.Providers;

namespace WebApi.DataProviders
{
    public class CarDataProvider(HttpCacheProvider cache,
                                 IConfiguration configuration,
                                 IHttpClientFactory httpClientFactory) : ICarDataProvider
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(HttpClientServiceConfig.HTTP_CLIENT_DEFAULT);

        public async Task<List<BrandDto>> GetCarsBrandsData()
        {
            var url = configuration.GetRequiredSection("Http")["Cars:Urls:Brands"];
            return await FetchAndDeserialize<List<BrandDto>>(url);
        }

        public async Task<CarModelDto> GetCarsModelsDataByBrandId(string brandId)
        {
            var url = configuration.GetRequiredSection("Http")["Cars:Urls:Models"]
                                    .Replace("{brand_Id}", brandId);

            return await FetchAndDeserialize<CarModelDto>(url);
        }

        private async Task<T> FetchAndDeserialize<T>(string url)
        {
            async Task<T> fun()
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(json);
            }

            return await cache.GetCacheOrExecuteFunction(url, fun);
        }
    }
}
