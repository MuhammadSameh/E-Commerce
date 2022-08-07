using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderRelatedEntities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepo;
        private readonly IMapper mapper;

        public OrderController(IOrderRepository orderRepo, IMapper mapper)
        {
            this.orderRepo = orderRepo;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("{cartId}/{DeliveryMethodId}")]
        public async Task<IActionResult> CreateOrder(int cartId, Address shippingAddress, int DeliveryMethodId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await orderRepo.CreateOrderAsync(userId,DeliveryMethodId,cartId,shippingAddress);
            if (order == null)
            {
                return BadRequest("There's a problem occurred when creating order");
            }
            return Ok(mapper.Map<OrderDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult> GetOrdersForUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await orderRepo.GetOrdersForUserAsync(userId);
            return Ok(mapper.Map<List<OrderDto>>(orders));
        }

        [HttpGet]
        [Route("OrdersForSupplier/{supplierId}")]
        public async Task<ActionResult<IReadOnlyList<InventoryDto>>> GetItemsSoldForSupplier(int supplierId)
        {
            var orderItems = await orderRepo.GetItemsSoldForSupplier(supplierId);
            var inventories = new List<Inventory>();
            foreach (var item in orderItems)
            {
                inventories.Add(item.Inventory);
            }
           var invensDto = mapper.Map<List<InventoryDto>>(inventories);
            return invensDto;
        }

        [HttpGet]
        [Route("{supplierId}")]
        public async Task<ActionResult> GetOrderRevenueForSupplier(int supplierId)
        {
            return Ok(await orderRepo.GetOrdersRevenueForSupplier(supplierId));
        }

    }
}
