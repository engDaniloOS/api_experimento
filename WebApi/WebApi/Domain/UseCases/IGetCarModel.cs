using WebApi.Domain.Dtos;

namespace WebApi.Domain.UseCases
{
    public interface IGetCarModel
    {
        Task<CarModelDto> GetCarsModelsByBrandId(int brandId);
    }
}
