using AutoMapper;
using FoodOrder.Core.Models;
using FoodOrder.Core.Models.Food;
using FoodOrder.Data;
using FoodOrder.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Core.Services
{
    public interface IFoodService
    {
        Task<PagedResult<Food>> GetFoods(int pageNumber, int pageSize);
        Task<PagedResult<FoodCategory>> GetCategories(int pageNumber, int pageSize);
        Task<PagedResult<Food>> CreateFood(CreateFoodDto createFoodDto);
        Task<PagedResult<Food>> UpdateFood(int id, CreateFoodDto updateFoodDto);
        Task<PagedResult<Food>> DeleteFood(int id);
    }

    public class FoodService : IFoodService
    {
        private readonly FoodOrderDbContext _context;
        private readonly IMapper _mapper;

        public FoodService(FoodOrderDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<Food>> CreateFood(CreateFoodDto createFoodDto)
        {
            if (await _context.FoodCategories.AnyAsync(c => c.Id == createFoodDto.CategoryId))
            {
                var food = _mapper.Map<Food>(createFoodDto);
                _context.Foods.Add(food);
                await _context.SaveChangesAsync();
                return await GetFoods(1, 10);
            }
            else
            {
                throw new Exception("Kategória nem található");
            }

        }

        public async Task<PagedResult<Food>> DeleteFood(int id)
        {
            var food = await _context.Foods.FindAsync(id) ?? throw new Exception("Étel nem található");
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
            return await GetFoods(1, 10);
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

        public async Task<PagedResult<Food>> UpdateFood(int id, CreateFoodDto updateFoodDto)
        {
            if (await _context.FoodCategories.AnyAsync(c => c.Id == updateFoodDto.CategoryId))
            {
                var food = await _context.Foods.FindAsync(id) ?? throw new Exception("Étel nem található");
                _mapper.Map(updateFoodDto, food);
                _context.Foods.Update(food);
                await _context.SaveChangesAsync();
                return await GetFoods(1, 10);
            }
            else
            {
                throw new Exception("Kategória nem található");
            }
        }
    }
}
