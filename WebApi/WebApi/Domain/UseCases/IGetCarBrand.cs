using WebApi.Domain.Dtos;

namespace WebApi.Domain.UseCases
{
    public interface IGetCarBrand
    {
        Task<CarBrandDto> GetCarsBrands();
    }
}
