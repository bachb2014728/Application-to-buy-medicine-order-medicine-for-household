
using backend.Dto.Cart;
using backend.Dto.Store;

namespace backend.Dto.Auth
{
    public class CustomerDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RoleName { get; set; }
        public List<StoreItem>? Stores { get; set; } = [];
        public List<CartItem>? Carts { get; set; } = [];
    }
}