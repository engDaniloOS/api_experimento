using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.UseCases;

namespace WebApi.Entrypoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _service;

        public CarsController(ICarService carService) => _service = carService;

        [HttpGet]
        [Route("brands")]
        public async Task<IActionResult> GetAllCarsBrands()
        {
            var brandsDto = await _service.GetCarsBrands();

            if (brandsDto.HasError)
                return BadRequest(brandsDto.ErrorMessage);

            return Ok(brandsDto);
        }

        [HttpGet]
        [Route("brands/{brand_id}")]
        public async Task<IActionResult> GetCarById([FromRoute(Name = "brand_id")] int brandId)
        {
            var modelsDto = await _service.GetCarsModelsByBrandId(brandId);

            if(modelsDto.HasError)
                return BadRequest(modelsDto.ErrorMessage);

            return Ok(modelsDto);
        }
    }
}
