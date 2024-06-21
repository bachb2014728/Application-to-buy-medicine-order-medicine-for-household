
using backend.Dto.Customer;
using backend.Dto.Product;
using backend.Dto.Voucher;

namespace backend.Dto.Store
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public string? URL { get; set; }
        public int? Avatar{ get; set; } 
        public int? Background { get; set; }
        public string? Info { get; set;}
        public List<int?>? Followers { get; set; } = [];
        public required string? Address { get; set; }
        public decimal Star { get; set; } = 0;
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomerItem? CreatedBy{ get; set; }
        public string? RoleName { get; set; }
        public bool? IsBoss { get; set; }
        public List<ProductItem>? Products { get; set; } = [];
        public List<VoucherItem>? Vouchers { get; set; } = [];
    }
}