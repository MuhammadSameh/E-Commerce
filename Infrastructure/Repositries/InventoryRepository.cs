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
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        private readonly EcommerceContext context;

        public InventoryRepository(EcommerceContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<Inventory>> GetInventoryByBrand(string brandName, string sortBy)
        {
            var query = context.Inventories.Where(i => i.Product.Brand.Name == brandName);
            return await AddSort(query, sortBy).ToListAsync();

        }

        public async Task<IReadOnlyList<Inventory>> GetInventoryByCategory(string categoryName, string sortBy)
        {
            var query = context.Inventories
                .Where(i => i.Product.Category.Name == categoryName);

            return await AddSort(query,sortBy).ToListAsync();

        }


        public async Task<IReadOnlyList<Inventory>> GetProducts(string sortBy)
        {
            var query = context.Inventories
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);

          return await AddSort(query, sortBy).ToListAsync();


        }

        private IQueryable<Inventory> AddSort(IQueryable<Inventory> query, string sortBy)
        {
            switch (sortBy)
            {
                case "priceAsc":
                    {
                        query
                  .OrderBy(inv => inv.Price);

                    }
                    break;

                case "priceDesc":
                    {
                        query.OrderByDescending(inv => inv.Price);  
                    }
                    break;

                default:
                    {
                        query.OrderByDescending(inv => inv.CreatedDate);
                    }
                    break;
            }
            return query;
        }
    }
}
