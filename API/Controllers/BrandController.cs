using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBaseService<Brand> _brandService;
        public BrandController(IBaseService<Brand> brandService)
        {
            
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            return Ok(await _brandService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            return Ok(await _brandService.GetByIdAsync(id));
        }

        [HttpPost]
        public ActionResult<Brand> AddBrand([FromBody] Brand brand)
        {


            _brandService.Add(brand);
            return CreatedAtAction("GetBrand", new { id = brand.BrandId }, brand);
        }

        [HttpPost("{id}")]
        public ActionResult<Brand> UpdateBrand(int id, [FromBody] Brand brand)
        {
            if (id != brand.BrandId)
            {
                return BadRequest();
            }
            else if (brand is null)
            {
                return NotFound();
            }

            _brandService.Update(brand);

            return CreatedAtAction("GetBrand", new { id = brand.BrandId }, brand);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            Brand brand = await _brandService.GetByIdAsync(id);

            if (brand is null)
                return NotFound();
            if (brand.BrandId != id)
                return NotFound();


            _brandService.Delete(brand);

            return NoContent();
        }

    }
}
