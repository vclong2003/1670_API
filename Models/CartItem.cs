using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    [PrimaryKey(nameof(UserID), nameof(BookID))]
    public class CartItem
    {
        [JsonIgnore]
        public User user { get; set; }
        public int UserID { get; set; }
        [JsonIgnore]
        public Book book { get; set; }
        public int BookID { get; set; }
        public int quantity { get; set; }
    }
}
