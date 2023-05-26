namespace _1670_API.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ThumbnailUrl { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishcationDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
