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

        public Task<IReadOnlyList<Inventory>> GetInventoryForSupplier(int supplierId);
        public Task<Inventory> GetProduct(int id);
        public Task<IReadOnlyList<Inventory>> FiltrationByColor(string categoryName, string sortBy, int pageSize, int currentPage, string color);
        public Task<IReadOnlyList<Inventory>> FiltrationByBrand(string categoryName, string sortBy, int pageSize, int currentPage, int brandId);
        public Task<IReadOnlyList<Inventory>> FiltrationByPrice(string categoryName, string sortBy, int pageSize, int currentPage, decimal PriceMin, decimal PriceMax);
        public Task<IReadOnlyList<Inventory>> GetInventoriesByProduct(int productId);

    }
}
