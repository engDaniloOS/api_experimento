using WebApi.Domain.Dtos;
using WebApi.Domain.Providers;
using WebApi.Domain.UseCases;

namespace WebApi.Domain
{
    public class CarService(ICarDataProvider provider, ILogger<CarService> logger) : ICarService
    {
        public async Task<CarBrandDto> GetCarsBrands()
        {
            logger.LogInformation("Executing Get Cars Brands...");
            var carBaseDto = await provider.GetCarsBrandsData();

            return new CarBrandDto { Brands = carBaseDto };
        }

        public async Task<CarModelDto> GetCarsModelsByBrandId(int brandId)
        {
            logger.LogInformation($"Executing Get Cars Models to brand {brandId}...");
            var carBaseDto = await provider.GetCarsModelsDataByBrandId(brandId.ToString());

            return carBaseDto;
        }
    }
}
