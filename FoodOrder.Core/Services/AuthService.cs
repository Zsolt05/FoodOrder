using FoodOrder.Core.Constans;
using FoodOrder.Core.Models.User;
using FoodOrder.Data;
using FoodOrder.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FoodOrder.Core.Services
{
    public interface IAuthService
    {
        Task Register(User user, string password);
        Task<AuthResponseDto> Login(UserLoginDto loginDto);
        Task<User?> GetUser();
    }
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly FoodOrderDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(
            IConfiguration configuration, FoodOrderDbContext context, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<AuthResponseDto> Login(UserLoginDto loginDto)
        {
            var user = await _context.Users!
                .Include(u=>u.Roles)
                    .ThenInclude(ur=>ur.Role)
                .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(loginDto.Email.ToLower()))
                ?? throw new Exception("Rossz felhasználónév vagy jelszó");

            if (VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                return new AuthResponseDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Token = CreateToken(user)
                };
            }
            else
            {
                throw new Exception("Rossz felhasználónév vagy jelszó");
            }
        }

        public async Task Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                throw new Exception("A felhasználó már létezik");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var userRole = await _context.Roles!.FirstOrDefaultAsync(role => role.Name.Equals(Roles.User)) ?? throw new Exception("Nem található a szerepkör");

            await _context.Users!.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.UserRoles!.AddAsync(new UserRole { UserId = user.Id, RoleId = userRole.Id });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users!.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower())))
                return true;
            return false;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            ];

            user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.Role.Name)));

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Token").Value ?? throw new Exception("Token kulcs nincs megadva")));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public Task<User?> GetUser()
        {
            var email = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            return _context.Users!.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email!.ToLower()));
        }
    }
}