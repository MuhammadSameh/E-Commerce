using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository repo;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
             return Ok(await repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            // CREATE categoryDto!!  to represent
            return Ok(await repo.GetCategoryById(id));
        }

        [HttpPost]
        [Authorize(Policy ="Admin")]
        public ActionResult<Category> AddCategory([FromBody]CategoryDto catDto)
        {
            
            Category cat = _mapper.Map<Category>(catDto);

            repo.Add(cat);
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
            repo.Update(cat);

            return CreatedAtAction("GetCategory", new { id = cat.CategoryId }, catDto);

        }

        [HttpPost("delete/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            Category cat = await repo.GetByIdAsync(id);

            if (cat is null)
                return NotFound();
            if (cat.CategoryId != id)
                return NotFound();
            

            repo.Delete(cat);

            return NoContent();
        }



        [HttpGet("parentCategories")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetParentCategories()
        {
            return Ok(await repo.GetParentCategories());
        }

        [HttpGet("subcategories/{parentName}")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetSubCategoriesByParent(string parentName)
        {
            return Ok(await repo.GetSubCategoryByParentName(parentName));
        }


    }
}
