using Microsoft.EntityFrameworkCore;

namespace _1670_API.Models
{
    [PrimaryKey(nameof(ProductId), nameof(OrderId))]
    public class OrderItem
    {
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }
    }
}
