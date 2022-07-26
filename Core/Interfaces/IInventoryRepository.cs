using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IInventoryRepository: IBaseRepository<Inventory>
    {
        public Task<IReadOnlyList<Inventory>> GetInventoryByCategory(string categoryName, string sortBy
            , int pageSize,int currentPage);
        public Task<IReadOnlyList<Inventory>> GetInventoryByBrand(string brandName, string sortBy, int pageSize, int currentPage);

        public Task<IReadOnlyList<Inventory>> GetProducts(string sortBy, int pageSize, int currentPage);
        public Task<int> GetCount(Expression<Func<Inventory, bool>> whereClause);
    }
}
