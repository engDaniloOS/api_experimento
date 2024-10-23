using WebApi.Domain.Dtos;
using WebApi.Domain.Providers;
using WebApi.Domain.UseCases;

namespace WebApi.Domain
{
    public class CarService : ICarService
    {
        private readonly ICarDataProvider _provider;
        private readonly ILogger<CarService> _logger;

        public CarService(ICarDataProvider provider, ILogger<CarService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public async Task<CarBrandDto> GetCarsBrands()
        {
            try
            {
                _logger.LogInformation("Executing Get Cars Brands...");
                var carBaseDto = await _provider.GetCarsBrandsData();

                return new CarBrandDto { Brands = carBaseDto };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when try to get cars brands. Error: {ex.Message}");

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
                _logger.LogInformation($"Executing Get Cars Models to brand {brandId}...");
                var carBaseDto = await _provider.GetCarsModelsDataByBrandId(brandId.ToString());

                return carBaseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when try to get cars model to brand id {brandId}. Error: {ex.Message}");

                return new CarModelDto
                {
                    ErrorMessage = $"Erro: {ex.Message}",
                    HasError = true
                };
            }
        }
    }
}
