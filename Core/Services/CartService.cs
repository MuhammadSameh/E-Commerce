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
    public class CartService : ICartService
    {
        private readonly IBaseRepository<Cart> _baseRepository;

        public CartService(IBaseRepository<Cart> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<Cart> GetCartItems(int id)
        {
            var result =  await _baseRepository.FindByCondition(cart => cart.Id == id, query => query.Include(cart => cart.CartItems)
                                  .ThenInclude(i => i.Inventory)
                                  .ThenInclude(p => p.Product));
            return result.FirstOrDefault();
        }
    }
}
