using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    [PrimaryKey(nameof(ProductId), nameof(OrderId))]
    public class OrderItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }

        public int Quantity { get; set; }
    }

    public class OrderItemDTO
    {
        public int? ProductId { get; set; }
        public string? OrderId { get; set; }
        public int? Quantity { get; set; }
    }
}
