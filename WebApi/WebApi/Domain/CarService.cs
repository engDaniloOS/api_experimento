using System.Text.Json;
using WebApi.Domain.Dtos;
using WebApi.Domain.UseCases;

namespace WebApi.Domain
{
    public class CarService : ICarService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CarService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<CarBrandDto> GetCarsBrands()
        {
            try
            {
                var url = _configuration.GetRequiredSection("Http")["Cars:Brands"];
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
                var url = _configuration.GetRequiredSection("Http")["Cars:Models"]
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
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
