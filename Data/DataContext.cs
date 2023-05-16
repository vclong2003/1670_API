using _1670_API.Models;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; } = null;
        public DbSet<Category> Categories { get; set; } = null;
        public DbSet<BookStore> BookStores { get; set; } = null;
        public DbSet<CartItem> CartItems { get; set; } = null;
        public DbSet<Order> Orders { get; set; } = null;
        public DbSet<OrderItem> OrderItems { get; set; } = null;
        public DbSet<Store> Stores { get; set; } = null;
        public DbSet<User> Users { get; set; } = null;
    }
}
