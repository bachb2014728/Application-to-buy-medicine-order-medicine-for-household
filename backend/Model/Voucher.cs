using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Vouchers")]
public class Voucher
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Code { get; set; }
    public decimal? DiscountAmount { get; set; } // Số tiền giảm cụ thể
    public decimal? DiscountPercentage { get; set; } // Phần trăm giảm giá
    public DateTime CreatedOn { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int? StoreId { get; set; }
    public Store? Store { get; set; }
}