using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IBaseService<Inventory> _inventoryService;
        private readonly IBaseService<CartItem> _itemService;
        private readonly IMapper mapper;

        public CartController(IBaseRepository<Inventory> inventoryRepository,
            IBaseRepository<CartItem> itemRepository, IMapper mapper, ICartService cartService,
            IBaseService<Inventory> inventoryService, IBaseService<CartItem> itemService)
        {
            this.mapper = mapper;
            _cartService = cartService;
            _inventoryService = inventoryService;
            _itemService = itemService;
        }



        [HttpGet]
        [Route("{cartId}")]
        public async Task<ActionResult> GetCart(int cartId)
        {
            var cart = await _cartService.GetCartItems(cartId);
            if (cart == null) return NotFound("Cart Not found");
            var cartDto = mapper.Map<CartDto>(cart);
            return Ok(cartDto);
        }
        [HttpGet]
        public ActionResult AddCart()
        {
            var cart = new Cart();
            var result = _cartService.Add(cart);
            return CreatedAtAction(nameof(GetCart),new {cartId = cart.Id}, cart);
        }

        [HttpPost]
        [Route("{cartId}/{inventoryId}/{quantity}")]
        public async Task<ActionResult> AddToCart(int cartId, int inventoryId, int quantity)
        {
            var cartItem = new CartItem { Quantity = quantity, CartId = cartId, InventoryId = inventoryId };
            var cart = await _cartService.GetByIdAsync(cartId);
            var inventory = await _inventoryService.GetByIdAsync(inventoryId);
            if (cart == null || inventory == null) return NotFound();
            if (cart.CartItems == null) cart.CartItems = new List<CartItem>();
            cart.CartItems.Add(cartItem);
            await _cartService.Save();
            return NoContent();
        }


        [HttpPost]
        [Route("removeItem/{cartId}/{itemId}")]
        public async Task<ActionResult> DeleteFromCart(int cartId, int itemId)
        {
            
            var cart = await _cartService.GetByIdAsync(cartId);
            var item = await _itemService.GetByIdAsync(itemId);
           
            if (cart == null || item == null) return NotFound();
           var result =  cart.CartItems.Remove(item);
           await _cartService.Save();
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
