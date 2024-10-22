using WebApi.Domain.Dtos;

namespace WebApi.Domain.Providers
{
    public interface ICarDataProvider
    {
        Task<List<BrandDto>> GetCarsBrandsData();

        Task<CarModelDto> GetCarsModelsDataByBrandId(string brandId);
    }
}
