using System.Text.Json.Serialization;

namespace _1670_API.Models.DTOs
{
    public class BookDTO
    {
        public string DateAdded { get; set; }
        public string thumbnailUrl { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double AuthorName { get; set; }
        public int CategoryID { get; set; }
    }
}
