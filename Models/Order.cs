using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    public class Order
    {
        public string Id { get; set; }

        public int CustomerId { get; set; }
        public Account Customer { get; set; }

        // Nullable: when delete address, the order still exists
        public int? ShippingAddressId { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }

        public int? StaffId { get; set; }
        [JsonIgnore]
        public Staff? Staff { get; set; }

        public DateTime Date { get; set; }

        [RegularExpression(@"^(Bank transfer|Pay when received)$")]
        public string PaymentMethod { get; set; }

        [RegularExpression(@"^(Pending|Processing|Delivering|Delivered|Cancelled)$")]
        public string Status { get; set; }

        public ICollection<OrderItem> Items { get; } = new List<OrderItem>();


    }

    public class OrderDTO
    {
        public string? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? StaffId { get; set; }
        public DateTime? Date { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
    }
}
