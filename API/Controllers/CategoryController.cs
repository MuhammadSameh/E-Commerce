using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            this._mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
             return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            
            return Ok(await _categoryService.GetCategoryById(id));
        }

        [HttpGet]
        [Route("topbrands/{categoryId}")]
        public async Task<ActionResult<Brand>> GetTopBrands(int categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);
            if(category == null) return NotFound("Wrong Category Id");
            if(category.Brands == null)
            {
                return NotFound("No Top Brands in this category");
            }
           var brands = _mapper.Map<List<BrandDto>>(category.Brands);
            return Ok(brands);
        }

        [HttpPost]
        [Authorize(Policy ="Admin")]
        public ActionResult<Category> AddCategory([FromBody]CategoryDto catDto)
        {
            
            Category cat = _mapper.Map<Category>(catDto);

            _categoryService.Add(cat);
            return CreatedAtAction("GetCategory", new { id = cat.CategoryId }, cat);
        }

        [HttpPost("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult<Category> UpdateCategory(int id, [FromBody]CategoryDto catDto)
        {
            if(id != catDto.CategoryId)
            {
                return BadRequest();
            }else if(catDto is null)
            {
                return NotFound();
            }

            Category cat = _mapper.Map<Category>(catDto);
            _categoryService.Update(cat);

            return CreatedAtAction("GetCategory", new { id = cat.CategoryId }, catDto);

        }

        [HttpPost("delete/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            Category cat = await _categoryService.GetByIdAsync(id);

            if (cat is null)
                return NotFound();
            if (cat.CategoryId != id)
                return NotFound();
            

            _categoryService.Delete(cat);

            return NoContent();
        }



        [HttpGet("parentCategories")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetParentCategories()
        {
            return Ok(await _categoryService.GetParentCategories());
        }

        [HttpGet("subcategories/{parentName}")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetSubCategoriesByParent(string parentName)
        {
            return Ok(await _categoryService.GetSubCategoryByParentName(parentName));
        }

        [HttpGet("subcategoriesByparent/{parentId}")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetSubCategoriesByParent(int parentId)
        {
            return Ok(await _categoryService.GetSubCategoryByParentId(parentId));
        }

    }
}
