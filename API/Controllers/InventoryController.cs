using API.DTOs;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public InventoryController(IInventoryRepository repo,IMapper mapper)
        {
            this.repo = repo;
            this._mapper = mapper;
        }

        [HttpGet("AllProducts")]
        public async Task<ActionResult<IReadOnlyList<Inventory>>> getAllProducts()
        {
            return Ok(await repo.GetAllAsync());
        }

        [HttpGet("AllProducts/test")]
        public async Task<ActionResult<IReadOnlyList<InventoryDto>>> GetProducts()
        {
            IReadOnlyList<Inventory> invens = await repo.GetProducts();

            return Ok(
                this._mapper.Map<IReadOnlyList<InventoryDto>>(invens)
                );
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
