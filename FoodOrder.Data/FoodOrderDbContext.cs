using FoodOrder.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Data
{
    public class FoodOrderDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartFood> CartFoods { get; set; }
    }
}
