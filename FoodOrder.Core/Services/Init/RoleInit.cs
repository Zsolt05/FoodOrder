using FoodOrder.Core.Constans;
using FoodOrder.Data;
using FoodOrder.Data.Entities;

namespace FoodOrder.Core.Services.Init
{
    public class RoleInit
    {
        private readonly FoodOrderDbContext _context;
        public RoleInit(FoodOrderDbContext context)
        {
            _context = context;
        }

        public async Task Init()
        {
            if (!_context.Roles.Any())
            {
                await _context.Roles.AddAsync(new Role { Name = Roles.Admin });
                await _context.Roles.AddAsync(new Role { Name = Roles.User });
                await _context.SaveChangesAsync();
            }
        }
    }
}
