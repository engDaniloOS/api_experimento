using WebApi.Domain.Dtos;
using WebApi.Domain.Providers;
using WebApi.Domain.UseCases;

namespace WebApi.Domain
{
    public class CarService : ICarService
    {
        private readonly ICarDataProvider _provider;

        public CarService(ICarDataProvider provider) =>  _provider = provider;

        public async Task<CarBrandDto> GetCarsBrands()
        {
            try
            {
                var carBaseDto = await _provider.GetCarsBrandsData();

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
                var carBaseDto = await _provider.GetCarsModelsDataByBrandId(brandId.ToString());

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
    }
}
