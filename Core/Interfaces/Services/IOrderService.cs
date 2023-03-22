using Core.Entities.OrderRelatedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, int deliveryMethodId, int basketId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string userId);
        Task<decimal> GetOrdersRevenueForSupplier(int supplierId);

        Task<IReadOnlyList<OrderItem>> GetItemsSoldForSupplier(int supplierId);
    }
}
