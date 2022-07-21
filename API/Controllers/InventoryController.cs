using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IReadOnlyList<InventoryDto>>> GetProducts(string sortBy,
            int currentPage = 1, int pageSize=20)
        {
            IReadOnlyList<Inventory> invens = await repo.GetProducts(sortBy, pageSize,currentPage);

            return Ok(
                this._mapper.Map<IReadOnlyList<InventoryDto>>(invens)
                );
        }

        

        [HttpGet("ProductsByBrand/{brandName}")]
        public async Task<ActionResult<IReadOnlyList<Inventory>>> getProductsByBrand(string brandName, string sortBy,
            int currentPage = 1, int pageSize = 20)
        {
            return Ok(await repo.GetInventoryByBrand(brandName, sortBy,pageSize,currentPage));
        }

        [HttpGet("ProductsByCategory/{categoryName}")]
        public async Task<ActionResult<IReadOnlyList<Inventory>>> getProductsByCategory(string categoryName, string sortBy
            , int pageSize=20, int currentPage = 1)
        {
            return Ok(await repo.GetInventoryByCategory(categoryName, sortBy,pageSize,currentPage));
        }

        // CRUD

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetInventory(int id)
        {

            return Ok(await repo.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Policy ="Supplier")]
        public ActionResult<Category> AddInventory([FromBody] Inventory inventory)
        {
            repo.Add(inventory);
            return CreatedAtAction("GetInventory", new { id = inventory.InventoryId }, inventory);
        }

        [HttpPost("{id}")]
        [Authorize(Policy = "Supplier")]
        public ActionResult<Category> UpdateInventory(int id, [FromBody] InventoryDto inventoryDto)
        {
            if (id != inventoryDto.InventoryId)
            {
                return BadRequest();
            }
            else if (inventoryDto is null)
            {
                return NotFound();
            }

            Inventory inventory = _mapper.Map<Inventory>(inventoryDto);
            repo.Update(inventory);

            return CreatedAtAction("GetInventory", new { id = inventory.InventoryId }, inventory);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy ="Supplier")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            Inventory inventory = await repo.GetByIdAsync(id);

            if (inventory is null)
                return NotFound();
            if (inventory.InventoryId != id)
                return NotFound();


            repo.Delete(inventory);

            return NoContent();
        }




    }
}
