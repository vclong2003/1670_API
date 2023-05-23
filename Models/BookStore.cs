using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace _1670_API.Models
{
    [PrimaryKey(nameof(StoreID), nameof(BookID))]
    public class BookStore
    {
        public int StoreID { get; set; }

        [JsonIgnore]
        public Store Store { get; set; }
        public int BookID { get; set; }

        [JsonIgnore]
        public Book book { get; set; }

        public int quantity { get; set; }

    }
}

