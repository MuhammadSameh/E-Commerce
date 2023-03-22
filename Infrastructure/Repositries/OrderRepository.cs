using Core.Entities.OrderRelatedEntities;
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
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly EcommerceContext context;

        public OrderRepository(EcommerceContext context) : base(context)
        {
            this.context = context;
        }

        //public async Task<Order> CreateOrderAsync(string userId, int deliveryMethodId, int basketId, Address shippingAddress)
        //{
        //    var cart = await context.Carts.Where(c => c.Id == basketId).Include(c => c.CartItems).ThenInclude(i => i.Inventory)
        //        .FirstOrDefaultAsync();
        //    var items = new List<OrderItem>();
        //    if (cart == null || !cart.CartItems.Any())
        //        return null;
        //    foreach (var item in cart.CartItems)
        //    {
        //        var inventory = await context.Inventories.Where(i => i.InventoryId == item.InventoryId).FirstOrDefaultAsync();
        //        if(inventory == null || inventory.Quantity < item.Quantity)
        //        {
        //            return null;
        //        }
        //        inventory.Quantity -= item.Quantity;
        //        context.Entry(inventory).State = EntityState.Modified;
        //        var orderItem = new OrderItem { InventoryId = item.InventoryId, Price= item.Inventory.Price, Quantity = item.Quantity};
        //        cart.CartItems.Remove(item);
        //        context.Entry(cart).State =EntityState.Modified;
        //        items.Add(orderItem);
        //    }
        //    var deliveryMethod = await context.DeliveryMethods.Where(c => c.Id == deliveryMethodId).FirstOrDefaultAsync();
        //    var total = items.Sum(item => item.Price * item.Quantity);
        //    var order = new Order { DeliveryMethod = deliveryMethod, Total=total, UserId = userId, OrderItems = items
        //    , ShippingAddress=shippingAddress};
        //   await context.Orders.AddAsync(order);
        //   await context.SaveChangesAsync();
        //    return order;
        //}

        public async Task<Order> CreateOrderAsync(string userId, int deliveryMethodId, int basketId, Address shippingAddress)
        {
            var cart = await context.Carts
        .Include(c => c.CartItems)
        .ThenInclude(i => i.Inventory)
        .FirstOrDefaultAsync(c => c.Id == basketId);

            if (cart == null || !cart.CartItems.Any())
                return null;

            var orderItems = cart.CartItems
                .Select(item =>
                {
                    var inventory = item.Inventory;
                    if (inventory?.Quantity < item.Quantity)
                        return null;

                    inventory.Quantity -= item.Quantity;
                    var orderItem = new OrderItem
                    {
                        InventoryId = item.InventoryId,
                        Price = item.Inventory.Price,
                        Quantity = item.Quantity
                    };

                    return orderItem;
                })
                .Where(item => item != null)
                .ToList();

            var deliveryMethod = await context.DeliveryMethods
                .FirstOrDefaultAsync(c => c.Id == deliveryMethodId);

            var total = orderItems.Sum(item => item.Price * item.Quantity);

            var order = new Order
            {
                DeliveryMethod = deliveryMethod,
                Total = total,
                UserId = userId,
                OrderItems = orderItems,
                ShippingAddress = shippingAddress
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            return order;
        }

        public async Task<IReadOnlyList<OrderItem>> GetItemsSoldForSupplier(int supplierId)
        {
            return await context.OrderItems.Include(oi => oi.Inventory).Include(oi => oi.Inventory.Medias).Include(oi => oi.Inventory.Product)
                .Where(o => o.Inventory.Product.SupplierInfoId == supplierId).ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string userId)
        {
            return await context.Orders.Include(o => o.DeliveryMethod)
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<decimal> GetOrdersRevenueForSupplier(int supplierId)
        {
            return await context.OrderItems
                .Where(o => o.Inventory.Product.SupplierInfoId == supplierId)
                .SumAsync(oi => oi.Quantity*oi.Price);
        }
    }
}
