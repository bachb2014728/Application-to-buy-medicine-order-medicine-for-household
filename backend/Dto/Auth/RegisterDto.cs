
using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Không để trống username")]
        public required string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Address { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Không đủ 11 chữ số")]
        public required string Phone { get; set; }
        [Required(ErrorMessage = "Không để trống địa chỉ email")]
        [EmailAddress]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Không để trống mật khẩu")]
        public required string Password { get; set; }
    }
}