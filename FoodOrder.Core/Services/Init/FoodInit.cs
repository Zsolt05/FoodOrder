using FoodOrder.Data;
using FoodOrder.Data.Entities;

namespace FoodOrder.Core.Services.Init
{
    public class FoodInit
    {
        private readonly FoodOrderDbContext _context;

        public FoodInit(FoodOrderDbContext context)
        {
            _context = context;
        }

        public async Task Init()
        {
            if (!_context.FoodCategories.Any())
            {
                await _context.FoodCategories.AddAsync(new FoodCategory { Name = "Pizza" });
                await _context.FoodCategories.AddAsync(new FoodCategory { Name = "Burger" });
                await _context.FoodCategories.AddAsync(new FoodCategory { Name = "Tészta" });
                await _context.FoodCategories.AddAsync(new FoodCategory { Name = "Saláta" });
                await _context.FoodCategories.AddAsync(new FoodCategory { Name = "Desszert" });
                await _context.SaveChangesAsync();
            }

            if (!_context.Foods.Any())
            {
                await _context.Foods.AddAsync(new Food { Name = "Margherita Pizza", Price = 1000, CategoryId = 1 });
                await _context.Foods.AddAsync(new Food { Name = "Pepperoni Pizza", Price = 1200, CategoryId = 1 });
                await _context.Foods.AddAsync(new Food { Name = "Hawaii Pizza", Price = 1300, CategoryId = 1 });
                await _context.Foods.AddAsync(new Food { Name = "Quattro Stagioni Pizza", Price = 1400, CategoryId = 1 });
                await _context.Foods.AddAsync(new Food { Name = "Quattro Formaggi Pizza", Price = 1500, CategoryId = 1 });
                await _context.Foods.AddAsync(new Food { Name = "Cheeseburger", Price = 1500, CategoryId = 2 });
                await _context.Foods.AddAsync(new Food { Name = "Baconburger", Price = 1600, CategoryId = 2 });
                await _context.Foods.AddAsync(new Food { Name = "Fishburger", Price = 1700, CategoryId = 2 });
                await _context.Foods.AddAsync(new Food { Name = "Wrapper Burger", Price = 1400, CategoryId = 2 });
                await _context.Foods.AddAsync(new Food { Name = "Csirkeburger", Price = 1300, CategoryId = 2 });
                await _context.Foods.AddAsync(new Food { Name = "Carbonaras Tészta", Price = 1300, CategoryId = 3 });
                await _context.Foods.AddAsync(new Food { Name = "Csirkemelles Tészta", Price = 1400, CategoryId = 3 });
                await _context.Foods.AddAsync(new Food { Name = "Bolognai", Price = 1500, CategoryId = 3 });
                await _context.Foods.AddAsync(new Food { Name = "Spagetti", Price = 1200, CategoryId = 3 });
                await _context.Foods.AddAsync(new Food { Name = "Lasagne", Price = 1600, CategoryId = 3 });
                await _context.Foods.AddAsync(new Food { Name = "Cézár Saláta", Price = 1200, CategoryId = 4 });
                await _context.Foods.AddAsync(new Food { Name = "Görög Saláta", Price = 1300, CategoryId = 4 });
                await _context.Foods.AddAsync(new Food { Name = "Csirkesaláta", Price = 1400, CategoryId = 4 });
                await _context.Foods.AddAsync(new Food { Name = "Tonhalas Saláta", Price = 1500, CategoryId = 4 });
                await _context.Foods.AddAsync(new Food { Name = "Cobb Saláta", Price = 1600, CategoryId = 4 });
                await _context.Foods.AddAsync(new Food { Name = "Tiramisu", Price = 800, CategoryId = 5 });
                await _context.Foods.AddAsync(new Food { Name = "Panna cotta", Price = 900, CategoryId = 5 });
                await _context.Foods.AddAsync(new Food { Name = "Palacsinta", Price = 600, CategoryId = 5 });
                await _context.Foods.AddAsync(new Food { Name = "Sorbet", Price = 700, CategoryId = 5 });
                await _context.Foods.AddAsync(new Food { Name = "Pohárkrém", Price = 800, CategoryId = 5 });
                await _context.SaveChangesAsync();
            }
        }
    }
}
