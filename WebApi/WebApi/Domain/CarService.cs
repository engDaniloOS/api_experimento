using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using WebApi.Domain.Dtos;
using WebApi.Domain.UseCases;

namespace WebApi.Domain
{
    public class CarService : ICarService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public CarService(IConfiguration configuration, 
                          IMemoryCache cache, 
                          HttpClient httpClient)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<CarBrandDto> GetCarsBrands()
        {
            try
            {
                var url = _configuration.GetRequiredSection("Http")["Cars:Urls:Brands"];
                var carBaseDto = await ExecuteHttpGetRequest<List<BrandDto>>(url);

                return new CarBrandDto { Brands = carBaseDto };
            }
            catch (Exception ex)
            {
                return new CarBrandDto
                {
                    ErrorMessage = $"Erro: {ex.Message}",
                    HasError = true
                };
            }
        }

        public async Task<CarModelDto> GetCarsModelsByBrandId(int brandId)
        {
            try
            {
                var url = _configuration.GetRequiredSection("Http")["Cars:Urls:Models"]
                                        .Replace("{brand_Id}", brandId.ToString());

                var carBaseDto = await ExecuteHttpGetRequest<CarModelDto>(url);

                return carBaseDto;
            }
            catch (Exception ex)
            {
                return new CarModelDto
                {
                    ErrorMessage = $"Erro: {ex.Message}",
                    HasError = true
                };
            }
        }

        private async Task<T> ExecuteHttpGetRequest<T>(string url)
        {
            var cacheExpiresIn = _configuration.GetRequiredSection("Http")["Cars:CachePeriodInMinutes"];
            var cacheSize = _configuration.GetRequiredSection("Http")["Cars:CacheSizeInMb"];

            var cachedResponse = _cache.GetOrCreate(url, async fun =>
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
