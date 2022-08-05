using API.DTOs;
using API.Responses;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository repo;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Product> productRepo;
        private readonly IBaseRepository<Media> mediaRepo;

        public InventoryController(IInventoryRepository repo, IMapper mapper, IBaseRepository<Product> productRepo)
        {
            this.repo = repo;
            this._mapper = mapper;
            this.productRepo = productRepo;
            this.mediaRepo = mediaRepo;
        }


        [HttpGet("AllProducts")]
        public async Task<ActionResult<IReadOnlyList<InventoryDto>>> GetProducts(string sortBy,
            int currentPage = 1, int pageSize = 20)
        {
            IReadOnlyList<Inventory> invens = await repo.GetProducts(sortBy, pageSize, currentPage);

            return Ok(
                this._mapper.Map<IReadOnlyList<InventoryDto>>(invens)
                );
        }



        [HttpGet("ProductsByBrand/{brandName}")]
        public async Task<ActionResult<PaginationResponse<IReadOnlyList<InventoryDto>>>> getProductsByBrand(string brandName, string sortBy,
            int currentPage = 1, int pageSize = 20)
        {
            var invens = await repo.GetInventoryByBrand(brandName, sortBy, pageSize, currentPage);
            var data = _mapper.Map<IReadOnlyList<InventoryDto>>(invens);
            int count = await repo.GetCount(inv => inv.Product.Brand.Name == brandName);
            int totalPages = (count / pageSize);
            if ((count % pageSize) > 0)
                totalPages = totalPages + 1;

            var paginationResponse = new PaginationResponse<IReadOnlyList<InventoryDto>>
            {
                Data = data,
                PageIndex = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return Ok(paginationResponse);
        }

        [HttpGet("ProductsByCategory/{categoryName}")]
        public async Task<ActionResult<PaginationResponse<IReadOnlyList<InventoryDto>>>> getProductsByCategory(string categoryName, string sortBy
            , int pageSize = 20, int currentPage = 1)
        {
            var invens = await repo.GetInventoryByCategory(categoryName, sortBy, pageSize, currentPage);
            var data = _mapper.Map<IReadOnlyList<InventoryDto>>(invens);
            int count = await repo.GetCount(inv => inv.Product.Category.Name == categoryName);
            int totalPages = (count / pageSize);
            if ((count % pageSize) > 0)
                totalPages = totalPages + 1;
            var paginationResponse = new PaginationResponse<IReadOnlyList<InventoryDto>>
            {
                Data = data,
                PageIndex = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return Ok(paginationResponse);
        }

        // CRUD

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryDto>> GetInventory(int id)
        {
            var inventory = await repo.GetProduct(id);
            if (inventory == null) { return NotFound("Invalid Id"); }
            var inventoryDto = _mapper.Map<InventoryDto>(inventory);
            return Ok(inventoryDto);
        }

        [HttpPost]
        [Route("addInventory")]
        [Authorize(Policy = "Supplier")]
        public async Task<ActionResult<InventoryDto>> AddInventory([FromBody] Inventory inventory)
        {
            if (inventory == null)
            {
                return BadRequest("Please Enter required information");
            }
            var product = await productRepo.GetByIdAsync(inventory.ProductId);
            if (product == null) { return BadRequest("This product Id is invalid"); }
            await repo.Add(inventory);
            var inventoryDto = _mapper.Map<InventoryDto>(inventory);
            return CreatedAtAction("GetInventory", new { id = inventory.InventoryId }, inventoryDto);
        }



        [HttpPost]
        [Route("addProduct")]
        [Authorize(Policy = "Supplier")]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Please Enter Required Information");
            }
            await productRepo.Add(product);
            return Ok(new { id = product.ProductId });
        }

        [HttpPost("{id}")]
        [Authorize(Policy = "Supplier")]
        public async Task<ActionResult<InventoryDto>> UpdateInventory(int id, [FromBody] Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return BadRequest();
            }
            else if (inventory is null)
            {
                return NotFound();
            }


            var product = await productRepo.GetByIdAsync(inventory.ProductId);
            if (product == null) { return BadRequest("This product Id is invalid"); }

            await repo.Update(inventory);
            var inventoryDto = _mapper.Map<InventoryDto>(inventory);

            return CreatedAtAction("GetInventory", new { id = inventory.InventoryId }, inventoryDto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "Supplier")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            Inventory inventory = await repo.GetProduct(id);

            if (inventory is null)
                return NotFound();
            if (inventory.InventoryId != id)
                return NotFound();

            foreach (var media in inventory.Medias)
            {
                var fileUrl = media.PicUrl;
                string[] seperators = new string[] { "//", "/" };
                var arr = fileUrl.Split(seperators, System.StringSplitOptions.None);
                var fileName = arr[arr.Length - 1];
                var fullFilePath = Directory.GetCurrentDirectory() + @"\Assets\Images\" + fileName;
                System.IO.File.Delete(fullFilePath);

            }

            repo.Delete(inventory);

            return NoContent();
        }

        [HttpGet]
        [Route("productsForSupplier/{supplierInfoId}")]
        public async Task<ActionResult<IReadOnlyList<InventoryDto>>> GetInventoryForSupplier(int supplierInfoId)
        {
            var invens = await repo.GetInventoryForSupplier(supplierInfoId);
            var invensDto = _mapper.Map<IReadOnlyList<InventoryDto>>(invens);
            return Ok(invensDto);
        }

        ///Filtration 
        ///
        [HttpGet("Filtration")]
        public async Task<ActionResult<PaginationResponse<IReadOnlyList<InventoryDto>>>> Filtration(string categoryName, string sortBy, string color,
            int brandId, decimal PriceMin, decimal PriceMax, int pageSize = 20, int currentPage = 1)
        {
            var invens = await repo.Filtration(categoryName,sortBy, pageSize,currentPage,color,brandId,PriceMin,PriceMax);
            var data = _mapper.Map<IReadOnlyList<InventoryDto>>(invens);
            //int count = await repo.GetCount(inv => inv.Product.Category.Name == categoryName);
            //if (!string.IsNullOrEmpty(color)) { count = await repo.GetCount(inv => inv.Color == color); }
            //if (!(decimal.ToDouble(PriceMin) == 0) || !(decimal.ToDouble(PriceMax) == 0)) { count = await repo.GetCount(inv => inv.Price >PriceMin&&inv.Price<PriceMax); }
            //if (!(brandId == 0)) { count = await repo.GetCount(inv => inv.Product.BrandId==brandId); }

            int count = await repo.GetCount(inv =>
            (string.IsNullOrEmpty(color) || inv.Color == color) &&
            (((decimal.ToDouble(PriceMin) == 0) || (decimal.ToDouble(PriceMax) == 0)) || inv.Price > PriceMin && inv.Price < PriceMax) &&
            ((brandId == 0) || inv.Product.BrandId == brandId) && inv.Product.Category.Name == categoryName);

            int totalPages = (count / pageSize);
            if ((count % pageSize) > 0)
                totalPages = totalPages + 1;
            var paginationResponse = new PaginationResponse<IReadOnlyList<InventoryDto>>
            {
                Data = data,
                PageIndex = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return Ok(paginationResponse);
        }


        [HttpGet("GetInventoriesByProduct/{productId}")]
        public async Task<ActionResult<InventoryDto>> GetInventoriesByProduct(int productId)
        {
            var inventory = await repo.GetInventoriesByProduct(productId);
            if (inventory == null) { return NotFound("Invalid Id"); }
            var inventoryDto = _mapper.Map<List<InventoryDto>>(inventory);
            return Ok(inventoryDto);
        }
    }
}
