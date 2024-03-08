using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
