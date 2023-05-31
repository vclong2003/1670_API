using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCartItems()
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

            var items = await _dataContext.CartItems.Where(c => c.CustomerId == accountDTO.Id).
                Include(c => c.Product)
                .Select(c => new
                {
                    id = c.Product.Id,
                    name = c.Product.Name,
                    price = c.Product.Price,
                    quantity = c.Quantity,
                })
                .ToListAsync();

            return StatusCode(200, items);

        }

        [HttpPost]
        public async Task<ActionResult> AddItemToCart(CartItemDTO cartItemDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

            var existedItem = await _dataContext.CartItems.FindAsync(accountDTO.Id, cartItemDTO.ProductId);
            if (existedItem != null) { return StatusCode(400, "Product already existed in cart!"); }

            var product = await _dataContext.Products.FindAsync(cartItemDTO.ProductId);
            if (product.Quantity < cartItemDTO.Quantity) { return StatusCode(400, "Exceed products available!"); }

            CartItem newItem = new()
            {
                CustomerId = (int)accountDTO.Id,
                ProductId = (int)cartItemDTO.ProductId,
                Quantity = (int)cartItemDTO.Quantity
            };

            _dataContext.CartItems.Add(newItem);
            await _dataContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemQuantity(int id, CartItemDTO cartItemDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

            var item = await _dataContext.CartItems.FindAsync(accountDTO.Id, id);
            if (item == null) { return StatusCode(404, "Item not existed!"); }

            var product = await _dataContext.Products.FindAsync(id);
            if (product.Quantity < cartItemDTO.Quantity) { return StatusCode(400, "Exceed products available!"); }

            item.Quantity = (int)cartItemDTO.Quantity;
            await _dataContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

            CartItem item = await _dataContext.CartItems.FindAsync(accountDTO.Id, id);
            if (item == null) { return StatusCode(404, "Item not existed!"); }

            _dataContext.CartItems.Remove(item);
            await _dataContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
