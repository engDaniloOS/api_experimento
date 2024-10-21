namespace WebApi.Domain.Dtos
{
    public class CarBrandDto: BaseDto
    {
        public List<BrandDto> Brands { get; set; }
    }
}
