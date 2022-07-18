using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICategoryRepository: IBaseRepository<Category>
    {
        Task<IReadOnlyList<Category>> GetSubCategoryByParentName(string parentName);
        Task<IReadOnlyList<Category>> GetParentCategories();

        Task<Category> GetCategoryById(int id);


    }
}
