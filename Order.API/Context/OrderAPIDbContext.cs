using Microsoft.EntityFrameworkCore;
using Order.API.Models;
namespace Order.API.Context
{
    public class OrderAPIDbContext : DbContext
    {
        public OrderAPIDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
    }
}
