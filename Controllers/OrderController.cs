using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null)
            {
                return StatusCode(401, "Unauthorized");
            }

            var items = await _dataContext.Orders.Where(o => o.CustomerId == accountDTO.Id).
                Include(o => o.ShippingAddress)
                .Select(o => new
                {
                    id = o.Id,
                    name = o.ShippingAddress.Name,
                    address = o.ShippingAddress.Address,
                    phone = o.ShippingAddress.Phone,
                    city = o.ShippingAddress.City,
                    country = o.ShippingAddress.Country,
                })
                .ToListAsync();

            return StatusCode(200, items);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderItems(string id)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null)
            {
                return StatusCode(401, "Unauthorized");
            }

            var items = await _dataContext.OrderItems.Where(o => o.OrderId == id).
                Include(o => o.Product)
                .Select(o => new
                {
                    productName = o.Product.Name,
                    price = o.Product.Price,
                    quantity = o.Quantity,
                    total = o.Product.Price * o.Quantity
                })
                .ToListAsync();

            return StatusCode(200, items);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDTO orderDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role != "CUSTOMER") { return StatusCode(401, "Unauthorized"); }



            try
            {
                var guid = Guid.NewGuid().ToString();
                Order order = new()
                {
                    Id = guid,
                    CustomerId = (int)accountDTO.Id,
                    ShippingAddressId = addressId,
                    StaffId = null,
                    Date = DateTime.Now,
                    ShippingFee = shippingFee,
                    Status = "Pending"
                };
                _dataContext.Orders.Add(order);
                await _dataContext.SaveChangesAsync();
                var cartItems = _dataContext.CartItems.Where(a => a.CustomerId == accountDTO.Id).ToList();
                foreach (var cartItem in cartItems)
                {
                    OrderItem item = new()
                    {
                        ProductId = cartItem.ProductId,
                        OrderId = guid,
                        Quantity = cartItem.Quantity,
                    };

                    _dataContext.OrderItems.Add(item);
                    await _dataContext.SaveChangesAsync();
                }
                _dataContext.CartItems.
                    RemoveRange(_dataContext.CartItems.
                    Where(a => a.CustomerId == accountDTO.Id)
                );
                await _dataContext.SaveChangesAsync();
                return StatusCode(200, "Create Order Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderStatus(string id, OrderStatusDTO orderStatus)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null)
            {
                return StatusCode(401, "Unauthorized");
            }

            var order = await _dataContext.Orders.FindAsync(id);
            order.StaffId = accountDTO.Id;
            order.Status = orderStatus.Status;
            if (orderStatus.Status == "Completed")
            {
                var orderItems = _dataContext.OrderItems.Where(oi=> oi.OrderId == id).ToList();
                foreach (var item in orderItems)
                {
                    var product = await _dataContext.Products.FindAsync(item.ProductId);
                    product.Quantity = product.Quantity - item.Quantity;
                    await _dataContext.SaveChangesAsync();
                }
            }
            await _dataContext.SaveChangesAsync();

            return StatusCode(200, "Update Order Status Successfully");
        }

        //Get All Order Of The System
        [HttpGet("all")]
        public async Task<ActionResult> AllOrders()
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null)
            {
                return StatusCode(401, "Unauthorized");
            }
            var orders = await _dataContext.Orders.Include(o => o.ShippingAddress)
                .Select(o => new
                {
                    id = o.Id,
                    name = o.ShippingAddress.Name,
                    phone = o.ShippingAddress.Phone,
                    address = o.ShippingAddress.Address,
                    city = o.ShippingAddress.City,
                    country = o.ShippingAddress.Country,
                    date = o.Date,
                    shippingFee = o.ShippingFee,
                    status = o.Status,
                })
                .ToListAsync();
            return StatusCode(200, orders);
        }
    }
}
