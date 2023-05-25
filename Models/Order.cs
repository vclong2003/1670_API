namespace _1670_API.Models
{
    public class Order
    {
        public string Id { get; set; }

        public string CustomerId { get; set; }
        public Account Customer { get; set; }

        public string ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        public string? StaffId { get; set; }
        public Staff? Staff { get; set; }

        public double ShippingFee { get; set; }

        public ICollection<OrderItem> Items { get; } = new List<OrderItem>();
    }
}
