using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    [PrimaryKey(nameof(OrderID), nameof(BookID))]
    public class OrderItem
    {
        [JsonIgnore]
        public Order order { get; set; }
        public int OrderID { get; set; }
        [JsonIgnore]
        public Book book { get; set; }
        public int BookID { get; set; }
        public int quantity { get; set; }
    }
}
