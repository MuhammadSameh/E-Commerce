using Core.Entities;
using Core.Interfaces;
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
        private readonly IBaseRepository<Brand> _repo;
        public BrandController(IBaseRepository<Brand> repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            return Ok(await _repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            return Ok(await _repo.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> AddBrand([FromBody] Brand brand)
        {


            await _repo.Add(brand);
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

            _repo.Update(brand);

            return CreatedAtAction("GetBrand", new { id = brand.BrandId }, brand);

        }

        [HttpPost("delete/{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            Brand brand = await _repo.GetByIdAsync(id);

            if (brand is null)
                return NotFound();
            if (brand.BrandId != id)
                return NotFound();


            _repo.Delete(brand);

            return NoContent();
        }

    }
}
