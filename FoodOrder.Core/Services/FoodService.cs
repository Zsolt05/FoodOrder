using FoodOrder.Core.Models;
using FoodOrder.Data;
using FoodOrder.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Core.Services
{
    public interface IFoodService
    {
        Task<PagedResult<Food>> GetFoods(int pageNumber, int pageSize);
        Task<PagedResult<FoodCategory>> GetCategories(int pageNumber, int pageSize);
    }

    public class FoodService : IFoodService
    {
        private readonly FoodOrderDbContext _context;

        public FoodService(FoodOrderDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FoodCategory>> GetCategories(int pageNumber, int pageSize)
        {
            var categories = await _context.FoodCategories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = categories.Create(await _context.FoodCategories.CountAsync(), pageNumber, pageSize);
            return result;
        }

        public async Task<PagedResult<Food>> GetFoods(int pageNumber, int pageSize)
        {
            var foods = await _context.Foods.Include(f => f.Category).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = foods.Create(await _context.Foods.CountAsync(), pageNumber, pageSize);
            return result;
        }
    }
}
