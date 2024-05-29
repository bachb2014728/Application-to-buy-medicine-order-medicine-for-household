
using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Auth
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Không đủ 11 chữ số")]
        public string? Phone { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}