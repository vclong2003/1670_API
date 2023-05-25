using Microsoft.EntityFrameworkCore;

namespace _1670_API.Models
{
    [PrimaryKey(nameof(CustomerId), nameof(ProductId))]
    public class CartItem
    {

        public string CustomerId { get; set; }
        public Account Customer { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
