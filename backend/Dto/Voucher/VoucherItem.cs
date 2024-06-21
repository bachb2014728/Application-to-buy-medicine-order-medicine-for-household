namespace backend.Dto.Voucher;

public class VoucherItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Code { get; set; }
    public decimal? DiscountAmount { get; set; } // Số tiền giảm cụ thể
    public decimal? DiscountPercentage { get; set; } // Phần trăm giảm giá
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsGlobal { get; set; }
}