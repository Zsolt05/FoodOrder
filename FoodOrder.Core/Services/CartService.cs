using FoodOrder.Core.Models;
using FoodOrder.Core.Models.Food;
using FoodOrder.Data;
using FoodOrder.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Core.Services
{
    public interface ICartService
    {
        Task AddToCart(int foodId, int quantity);
        Task RemoveFromCart(int foodId);
        Task ClearCart();
        Task<PagedResult<FoodOrderDto>> GetCartFoods(int pageNumber, int pageSize);
        Task FisishOrder();
    }

    public class CartService : ICartService
    {
        private readonly FoodOrderDbContext _context;
        private readonly User _user;

        public CartService(FoodOrderDbContext context, IAuthService authService)
        {
            _context = context;
            _user = authService.GetUser().Result ?? throw new ArgumentNullException("Felhasználó nem található");
        }

        public async Task AddToCart(int foodId, int quantity)
        {
            if (!await _context.Foods.AnyAsync(f=>f.Id == foodId))
            {
                throw new Exception("Étel nem található");
            }
            var cart = await GetActiveCart();
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = _user.Id,
                    Status = CartStatus.Open
                };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            var cartFood = await _context.CartFoods
                .FirstOrDefaultAsync(cf => cf.CartId == cart.Id && cf.FoodId == foodId);
            if (cartFood == null)
            {
                cartFood = new CartFood
                {
                    CartId = cart.Id,
                    FoodId = foodId,
                    Quantity = quantity
                };
                await _context.CartFoods.AddAsync(cartFood);
                await _context.SaveChangesAsync();
            }
            else
            {
                cartFood.Quantity += quantity;
                _context.CartFoods.Update(cartFood);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCart()
        {
            var cart = await GetActiveCart();
            if (cart != null)
            {
                var cartFoods = await _context.CartFoods
                    .Where(cf => cf.CartId == cart.Id)
                    .ToListAsync();
                _context.CartFoods.RemoveRange(cartFoods);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("A kosár nem található");
            }
        }

        public async Task FisishOrder()
        {
            var cart = await GetActiveCart();
            if (cart != null)
            {
                cart.Status = CartStatus.Finished;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("A kosár nem található");
            }
        }

        public async Task<PagedResult<FoodOrderDto>> GetCartFoods(int pageNumber, int pageSize)
        {
            var cart = await GetActiveCart();
            if (cart == null)
            {
                return new PagedResult<FoodOrderDto>();
            }
            var cartFoods = await _context.CartFoods
                .Include(cf => cf.Food)
                .Where(cf => cf.CartId == cart.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(cf => new FoodOrderDto
                {
                    CartFoodId = cf.Food.Id,
                    FoodName = cf.Food.Name,
                    Price = cf.Food.Price * cf.Quantity,
                    Quantity = cf.Quantity
                })
                .ToListAsync();
            return cartFoods.Create(await _context.CartFoods.CountAsync(), pageNumber, pageSize);
        }

        public async Task RemoveFromCart(int foodId)
        {
            if (!await _context.Foods.AnyAsync(f => f.Id == foodId))
            {
                throw new Exception("Étel nem található");
            }
            var cart = await GetActiveCart();
            if (cart != null)
            {
                var cartFood = await _context.CartFoods
                    .FirstOrDefaultAsync(cf => cf.CartId == cart.Id && cf.FoodId == foodId);
                if (cartFood != null)
                {
                    _context.CartFoods.Remove(cartFood);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("A kosár nem található");
            }
        }

        private async Task<Cart?> GetActiveCart()
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == _user.Id && c.Status == CartStatus.Open);
        }
    }
}