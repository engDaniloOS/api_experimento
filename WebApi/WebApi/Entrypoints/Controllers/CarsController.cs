using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.UseCases;

namespace WebApi.Entrypoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController(ICarService carService) : ControllerBase
    {
        [HttpGet]
        [Route("brands")]
        public async Task<IActionResult> GetAllCarsBrands() 
            => Ok(await carService.GetCarsBrands());

        [HttpGet]
        [Route("brands/{brand_id}")]
        public async Task<IActionResult> GetCarById([FromRoute(Name = "brand_id")] int brandId)
            => Ok(await carService.GetCarsModelsByBrandId(brandId));
    }
}
