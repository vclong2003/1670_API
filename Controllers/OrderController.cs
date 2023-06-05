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

        // GET: api/order
        // return all orders of the current customer
        [HttpGet]
        public async Task<ActionResult> GetCustomerOrders()
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

            var items = await _dataContext.Orders
                .Where(o => o.CustomerId == accountDTO.Id)
                .Include(o => o.ShippingAddress)
                .Select(o => new
                {
                    id = o.Id,
                    date = o.Date,
                    status = o.Status,
                    name = o.ShippingAddress.Name,
                    phone = o.ShippingAddress.Phone,
                    address = o.ShippingAddress.Address,
                    city = o.ShippingAddress.City,
                    country = o.ShippingAddress.Country,
                })
                .OrderByDescending(o => o.date)
                .ToListAsync();

            return StatusCode(200, items);
        }

        // GET: api/order/all
        [HttpGet("all")]
        public async Task<ActionResult> GetAllOrders()
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

            var items = await _dataContext.Orders
               .Include(o => o.ShippingAddress)
               .Select(o => new
               {
                   id = o.Id,
                   date = o.Date,
                   status = o.Status,
                   paymentMethod = o.PaymentMethod,
                   name = o.ShippingAddress.Name,
                   phone = o.ShippingAddress.Phone,
                   address = o.ShippingAddress.Address,
                   city = o.ShippingAddress.City,
                   country = o.ShippingAddress.Country,
               })
               .OrderByDescending(o => o.date)
               .ToListAsync();

            return StatusCode(200, items);
        }

        //// GET: api/order/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult> GetOrderItems(string id)
        //{
        //    AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
        //    if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

        //    IQueryable<Order> query = _dataContext.Orders
        //        .Where(o => o.Id == id)
        //        .Include(o => o.ShippingAddress)
        //        .Include(o => o.Items)
        //        .ThenInclude(i => i.Product);

        //    if (accountDTO.Role == "MANAGER")
        //    {
        //        query.Include(o => o.Staff);
        //    }

        //    // modifying ...

        //    var order = await query
        //        .Select(o => new
        //        {
        //            id = o.Id,
        //            date = o.Date,
        //            status = o.Status,
        //            paymentMethod = o.PaymentMethod,
        //            name = o.ShippingAddress.Name,
        //            phone = o.ShippingAddress.Phone,
        //            address = o.ShippingAddress.Address,
        //            city = o.ShippingAddress.City,
        //            country = o.ShippingAddress.Country,
        //            items = o.Items.Select(i => new
        //            {
        //                id = i.ProductId,
        //                name = i.Product.Name,
        //                price = i.Product.Price,
        //                quantity = i.Quantity,
        //            })
        //        })
        //        .FirstOrDefaultAsync();

        //    return StatusCode(200, order);
        //}

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



        // PUT: api/order/{id}
        // body params: status
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderStatus(string id, OrderDTO orderDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role != "STAFF") { return StatusCode(401, "Unauthorized"); }

            var order = await _dataContext.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();

            if (order.StaffId == null) { order.StaffId = accountDTO.Id; } // Only first staff can take the order ¬_¬
            order.Status = orderDTO.Status;

            if (order.Status == "Delivered")
            {
                foreach (var item in order.Items)
                {
                    item.Product.Quantity -= item.Quantity;
                    if (item.Product.Quantity < 0) { item.Product.Quantity = 0; } // =.=
                }
            }

            await _dataContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
