using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository repo;

        public CategoryController(ICategoryRepository repo)
        {
            this.repo = repo;
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
