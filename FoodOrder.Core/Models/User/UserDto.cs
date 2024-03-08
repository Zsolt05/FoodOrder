using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Core.Models.User
{
    public class UserDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDto : UserDto
    {
    }

    public class UserRegisterDto : UserDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }

    public class AuthResponseDto
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
