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

        public async Task<IReadOnlyList<Inventory>> GetInventoryByBrand(string brandName)
        {
            return await context.Inventories.Where(i => i.Product.Brand.Name == brandName)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Inventory>> GetInventoryByCategory(string categoryName)
        {
            return await context.Inventories
                .Where(i => i.Product.Category.Name == categoryName).ToListAsync();
        }
    }
}
