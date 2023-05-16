using Microsoft.Identity.Client;
using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string DateAdded { get; set; }
        public string thumbnailUrl { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double AuthorName { get; set; }
        [JsonIgnore]
        public Category category { get; set; }
        public int CategoryID { get; set; }
        [JsonIgnore]
        public List<OrderItem> orderItems { get; set; }
        [JsonIgnore]
        public List<CartItem> cartItems { get; set; }
        [JsonIgnore]
        public List<BookStore> bookStores { get; set; }



    }
}
