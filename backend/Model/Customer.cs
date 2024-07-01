
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("Customers")]
    public class Customer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ZipCode { get; set; }
        public string? Gender { get; set; }
        public List<Cart>? Carts { get; set; } = [];
        public List<Order>? Orders { get; set; } = [];
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}