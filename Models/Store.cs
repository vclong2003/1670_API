using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    public class Store
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public User user { get; set; }
        public int? userID { get; set; }
        [JsonIgnore]
        public List<Order> orders { get; set; }

    }
}
