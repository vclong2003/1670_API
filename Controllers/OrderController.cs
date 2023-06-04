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

        // POST: api/order
        // body params: shippingAddressId, paymentMethod
        // return id of the new order
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDTO orderDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role != "CUSTOMER") { return StatusCode(401, "Unauthorized"); }

            var cartItems = _dataContext.CartItems.Where(a => a.CustomerId == accountDTO.Id).ToList();
            if (cartItems.Count == 0) { return StatusCode(400, "Cart is empty!"); }

            var newOrderId = Guid.NewGuid().ToString();
            Order newOrder = new()
            {
                Id = newOrderId,
                CustomerId = (int)accountDTO.Id,
                ShippingAddressId = orderDTO.ShippingAddressId,
                StaffId = null,
                Date = DateTime.Now,
                PaymentMethod = orderDTO.PaymentMethod,
                Status = "Pending"
            };
            _dataContext.Orders.Add(newOrder);

            cartItems.ForEach(cartItems =>
            {
                OrderItem item = new()
                {
                    ProductId = cartItems.ProductId,
                    OrderId = newOrderId,
                    Quantity = cartItems.Quantity,
                };
                _dataContext.OrderItems.Add(item);
            });
            _dataContext.CartItems.RemoveRange(cartItems);

            await _dataContext.SaveChangesAsync();

            return StatusCode(200, newOrder.Id);
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
                var orderItems = _dataContext.OrderItems.Where(oi => oi.OrderId == id).ToList();
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
                    paymentMethod = o.PaymentMethod,
                    status = o.Status,
                })
                .ToListAsync();
            return StatusCode(200, orders);
        }
    }
}
