namespace backend.Dto.Voucher;

public class VoucherCreate
{
    public required string Title { get; set; }
    public decimal? DiscountAmount { get; set; } // Số tiền giảm cụ thể
    public decimal? DiscountPercentage { get; set; } // Phần trăm giảm giá
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int? StoreId { get; set; }
}