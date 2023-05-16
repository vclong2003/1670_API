using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public Store store { get; set; }
        [JsonIgnore]
        public List<Order> orders { get; set; }
        [JsonIgnore]
        public List<CartItem> cartItems { get; set; }

    }
}
