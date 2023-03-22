using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetSubCategoryByParentName(string parentName);
        Task<IReadOnlyList<Category>> GetSubCategoryByParentId(int parentId);
        Task<IReadOnlyList<Category>> GetParentCategories();

        Task<Category> GetCategoryById(int id);
    }
}
