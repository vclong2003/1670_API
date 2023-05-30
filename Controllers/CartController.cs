using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {

            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if(accountDTO == null)
            {
                return StatusCode(401, "Unauthorized");
            }
            else
            {
                var carts = await _dataContext.CartItems.Where(c => c.CustomerId == accountDTO.Id).
                    Include(c=>c.Product)
                    .Select(c => new {
                        name = c.Product.Name,
                        quantity = c.Quantity,
                        price = c.Product.Price
                    })
                    .ToListAsync();
                return StatusCode(200, carts);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Add(CartItemDTO cartItem)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null)
            {
                return StatusCode(401, "Unauthorized");
            }
            else
            {
                var current = await _dataContext.CartItems.FindAsync(accountDTO.Id, cartItem.ProductId);
                if(current is null)
                {
                    int quantity = _dataContext.Products
                        .Where(p => p.Id == cartItem.ProductId)
                        .Select(p => p.Quantity)
                        .SingleOrDefault();
                    if (quantity < cartItem.Quantity)
                    {
                        return StatusCode(401, "Product is exceed to stock");
                    }
                    else
                    {
                        CartItem cart = new()
                        {
                            CustomerId = (int)accountDTO.Id,
                            ProductId = (int)cartItem.ProductId,
                            Quantity = (int)cartItem.Quantity
                        };

                        _dataContext.CartItems.Add(cart);
                        await _dataContext.SaveChangesAsync();
                        return StatusCode(200, "Add To Cart Successfully");
                    }
                }
                else
                {
                    return StatusCode(401, "This product has existed in your cart");
                }
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int quantityAdded, int id)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null){
                return StatusCode(401, "Unauthorized");
            }
            else{
                var current = await _dataContext.CartItems.FindAsync(accountDTO.Id, id);
                if (current is null){
                    return StatusCode(401, "This item is not exist");
                }
                else{
                    int quantity = _dataContext.Products
                        .Where(p => p.Id == id)
                        .Select(p => p.Quantity)
                        .SingleOrDefault();
                    if (quantity < quantityAdded){
                        return StatusCode(401, "Product is exceed to stock");
                    }
                    else{
                        current.Quantity = quantityAdded;
                        await _dataContext.SaveChangesAsync();
                        return StatusCode(200, "Update Cart Item Successfully");
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null)
            {
                return StatusCode(401, "Unauthorized");
            }
            else
            {
                CartItem current = await _dataContext.CartItems.FindAsync(accountDTO.Id, id);
                if (current is null)
                {
                    return StatusCode(401, "This item is not exist");
                }
                else
                {
                    _dataContext.CartItems.Remove(current);
                    await _dataContext.SaveChangesAsync();
                    return StatusCode(200, "Delete item successfully");
                }
            }
        }
    }
}
