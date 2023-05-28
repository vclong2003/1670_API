namespace _1670_API.Models
{
    public class Order
    {
        public string Id { get; set; }

        public int CustomerId { get; set; }
        public Account Customer { get; set; }

        public int ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        public int? StaffId { get; set; }
        public Staff? Staff { get; set; }

        public DateTime Date { get; set; }

        public double ShippingFee { get; set; }

        public ICollection<OrderItem> Items { get; } = new List<OrderItem>();
    }

    public class OrderDTO
    {
        public string? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? StaffId { get; set; }
        public DateTime? Date { get; set; }
        public double? ShippingFee { get; set; }
    }
}
