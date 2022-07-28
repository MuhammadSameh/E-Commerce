using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositries
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly EcommerceContext context;

        public CategoryRepository(EcommerceContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await context.Categories.Include(p => p.ParentCategory).Include(c => c.Brands).FirstOrDefaultAsync(i => i.CategoryId == id);
           
        }

        public async Task<IReadOnlyList<Category>> GetParentCategories()
        {
            return await context.Categories.Where(c => c.ParentId == null).ToListAsync();
        }

        public async Task<IReadOnlyList<Category>> GetSubCategoryByParentId(int parentId)
        {
            return await context.Categories.Where(c => c.ParentCategory.CategoryId == parentId).ToListAsync();
        }

        public async Task<IReadOnlyList<Category>> GetSubCategoryByParentName(string parentName)
        {
            return await context.Categories.Where(c => c.ParentCategory.Name == parentName).ToListAsync();
        }
    }
}
