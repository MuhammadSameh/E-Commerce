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

        //public async Task<IReadOnlyList<Product>> GetProducts()
        //{
        //    return await context.Products
        //        .Include(c => c.Category)
        //        .Include(b => b.Brand)
        //        .Include(m => m.Medias).ToListAsync();
        //}

        public async Task<IReadOnlyList<Inventory>> GetProducts()
        {
            return await context.Inventories
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand)
                .ToListAsync();


        }
    }
}
