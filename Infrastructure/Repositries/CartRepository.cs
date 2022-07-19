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
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly EcommerceContext context;

        public CartRepository(EcommerceContext context): base(context)
        {
            this.context = context;
        }
        public async Task<Cart> GetCartItems(int id)
        {
            return await context.Carts.Include(c => c.CartItems).ThenInclude(i => i.Inventory).ThenInclude(p => p.Product)
                .Where(c=> c.Id == id).FirstOrDefaultAsync();
        }
    }
}
