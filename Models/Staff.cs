namespace _1670_API.Models
{
    public class Staff
    {
        public string Id { get; set; }

        public string AccountId { get; set; }
        public Account Account { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public ICollection<Order> Orders { get; } = new List<Order>();
    }
}
