using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }

        public User? User { get; set; }
        public Store? Store { get; set; }

        [JsonIgnore]
        public List<OrderItem> orderItems { get; set; }
    }
}
