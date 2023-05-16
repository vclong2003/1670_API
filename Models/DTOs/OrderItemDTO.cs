using System.Text.Json.Serialization;

namespace _1670_API.Models.DTOs
{
    public class OrderItemDTO
    {
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int quantity { get; set; }
    }
}
