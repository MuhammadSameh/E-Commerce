using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly IBaseRepository<Category> _baseRepository;

        public CategoryService(IBaseRepository<Category> baseRepository): base(baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var result = await _baseRepository.FindByCondition(c => c.CategoryId == id, query => query.Include(c => c.ParentCategory).Include(c => c.Brands));
            return result.FirstOrDefault();
        }

        public async Task<IReadOnlyList<Category>> GetParentCategories()
        {
           return await _baseRepository.FindByCondition(c => c.ParentCategory == null);
        }

        public async Task<IReadOnlyList<Category>> GetSubCategoryByParentId(int parentId)
        {
            return await _baseRepository.FindByCondition(c => c.ParentCategory.CategoryId == parentId);
        }

        public async Task<IReadOnlyList<Category>> GetSubCategoryByParentName(string parentName)
        {
            return await _baseRepository.FindByCondition(c => c.ParentCategory.Name == parentName);
        }
    }
}
