using FoodOrder.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Data
{
    public class FoodOrderDbContext : DbContext
    {
        public FoodOrderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
