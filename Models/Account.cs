using System.ComponentModel.DataAnnotations;

namespace _1670_API.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [RegularExpression("^(CUSTOMER|STAFF|MANAGER)$", ErrorMessage = "Invalid role")]
        public string Role { get; set; }
    }

    public class AccountDTO
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}