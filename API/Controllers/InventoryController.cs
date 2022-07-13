using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository repo;

        public InventoryController(IInventoryRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("AllProducts")]
        public async Task<ActionResult<IReadOnlyList<Inventory>>> getAllProducts()
        {
            return Ok(await repo.GetAllAsync());
        }

        [HttpGet("ProductsByBrand/{brandName}")]
        public async Task<ActionResult<IReadOnlyList<Inventory>>> getProductsByBrand(string brandName)
        {
            return Ok(await repo.GetInventoryByBrand(brandName));
        }

        [HttpGet("ProductsByCategory/{categoryName}")]
        public async Task<ActionResult<IReadOnlyList<Inventory>>> getProductsByCategory(string categoryName)
        {
            return Ok(await repo.GetInventoryByCategory(categoryName));
        }
    }
}
