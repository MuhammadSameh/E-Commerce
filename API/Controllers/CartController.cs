using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartRepository repository;
        private readonly IBaseRepository<Inventory> inventoryRepository;
        private readonly IBaseRepository<CartItem> itemRepository;
        private readonly IMapper mapper;

        public CartController(ICartRepository repository, IBaseRepository<Inventory> inventoryRepository,
            IBaseRepository<CartItem> itemRepository, IMapper mapper)
        {
            this.repository = repository;
            this.inventoryRepository = inventoryRepository;
            this.itemRepository = itemRepository;
            this.mapper = mapper;
        }



        [HttpGet]
        [Route("{cartId}")]
        public async Task<ActionResult> GetCart(int cartId)
        {
            var cart = await repository.GetCartItems(cartId);
            if (cart == null) return NotFound("Cart Not found");
            var cartDto = mapper.Map<CartDto>(cart);
            return Ok(cartDto);
        }
        [HttpGet]
        public async Task<ActionResult> AddCart()
        {
            var cart = new Cart();
            await repository.Add(cart);
            return CreatedAtAction(nameof(GetCart),new {cartId = cart.Id}, cart);
        }

        [HttpPost]
        [Route("{cartId}/{inventoryId}")]
        public async Task<ActionResult> AddToCart(int cartId, int inventoryId, int quantity)
        {
            var cartItem = new CartItem { Quantity = quantity, CartId = cartId, InventoryId = inventoryId };
            var cart = await repository.GetByIdAsync(cartId);
            var inventory = await inventoryRepository.GetByIdAsync(inventoryId);
            if (cart == null || inventory == null) return NotFound();
            if (cart.CartItems == null) cart.CartItems = new List<CartItem>();
            cart.CartItems.Add(cartItem);
            await repository.Save();
            return NoContent();
        }


        [HttpPost]
        [Route("removeItem/{cartId}/{itemId}")]
        public async Task<ActionResult> DeleteFromCart(int cartId, int itemId)
        {
            
            var cart = await repository.GetByIdAsync(cartId);
            var item = await itemRepository.GetByIdAsync(itemId);
           
            if (cart == null || item == null) return NotFound();
           var result =  cart.CartItems.Remove(item);
           await repository.Save();
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
