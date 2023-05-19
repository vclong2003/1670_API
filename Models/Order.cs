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
        public string Date { get; set; }
        [JsonIgnore]
        public User user { get; set; }
        public int? userID { get; set; }
        [JsonIgnore]
        public Store store { get; set; }
        public int? storeID { get; set; }
        [JsonIgnore]
        public List<OrderItem> orderItems { get; set; }


    }
}
