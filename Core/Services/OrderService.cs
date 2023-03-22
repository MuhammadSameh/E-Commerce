using Core.Entities;
using Core.Entities.OrderRelatedEntities;
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
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<Cart> _cartRepository;
        private readonly IBaseRepository<DeliveryMethod> _deliveryRepository;
        private readonly IBaseRepository<OrderItem> _orderItem;



        public OrderService(IBaseRepository<Order> orderRepository, IBaseRepository<Cart> cartRepository, IBaseRepository<DeliveryMethod> deliveryRepository, IBaseRepository<OrderItem> orderItem)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _deliveryRepository = deliveryRepository;
            _orderItem = orderItem;
        }

        public async Task<Order> CreateOrderAsync(string userId, int deliveryMethodId, int basketId, Address shippingAddress)
        {
            var result = await _cartRepository.FindByCondition(cart => cart.Id == basketId,
                                                             query => query.Include(c => c.CartItems)
                                                                           .ThenInclude(i => i.Inventory));
            var cart = result.FirstOrDefault();
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
            var deliveryMethod = await _deliveryRepository.GetByIdAsync(deliveryMethodId);
            var total = orderItems.Sum(item => item.Price * item.Quantity);
            var order = new Order
            {
                DeliveryMethod = deliveryMethod,
                Total = total,
                UserId = userId,
                OrderItems = orderItems,
                ShippingAddress = shippingAddress
            };
            await _orderRepository.Add(order);
            await _orderRepository.Save();
            return order;
        }

        public async Task<IReadOnlyList<OrderItem>> GetItemsSoldForSupplier(int supplierId)
        {
            var result = await _orderItem.FindByCondition(order => order.Inventory.Product.SupplierInfoId == supplierId, query => query
                                                                                                                   .Include(oi => oi.Inventory)
                                                                                                                   .Include(oi => oi.Inventory.Medias)
                                                                                                                   .Include(oi => oi.Inventory.Product));
            return result.ToList();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string userId)
        {
            var result = await _orderRepository.FindByCondition(order => order.UserId == userId, query => query.Include(o => o.DeliveryMethod)
                                                                                                         .Include(o => o.OrderItems));
            return result.ToList();
        }

        public async Task<decimal> GetOrdersRevenueForSupplier(int supplierId)
        {
            var result = await _orderItem.FindByCondition(o => o.Inventory.Product.SupplierInfoId == supplierId);
            return result.Sum(oi => oi.Quantity * oi.Price);
        }
    }
}
