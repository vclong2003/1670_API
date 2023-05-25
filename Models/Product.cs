namespace _1670_API.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishcationDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public int Stock { get; set; }
    }
}
