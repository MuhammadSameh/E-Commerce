using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IInventoryRepository: IBaseRepository<Inventory>
    {
        public Task<IReadOnlyList<Inventory>> GetInventoryByCategory(string categoryName, string sortBy);
        public Task<IReadOnlyList<Inventory>> GetInventoryByBrand(string brandName, string sortBy);

        public Task<IReadOnlyList<Inventory>> GetProducts(string sortBy);
    }
}
