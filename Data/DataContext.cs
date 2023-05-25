using _1670_API.Models;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null;
        public DbSet<CartItem> CartItems { get; set; } = null;
        public DbSet<Category> Categories { get; set; } = null;
        public DbSet<Order> Orders { get; set; } = null;
        public DbSet<OrderItem> OrderItems { get; set; } = null;
        public DbSet<Product> Products { get; set; } = null;
        public DbSet<ShippingAddress> ShippingAddresses { get; set; } = null;
        public DbSet<Staff> Staffs { get; set; } = null;
    }
}
