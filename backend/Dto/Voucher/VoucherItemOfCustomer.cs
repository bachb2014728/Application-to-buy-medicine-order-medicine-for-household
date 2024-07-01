namespace backend.Dto.Voucher;

public class VoucherItemOfCustomer
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Code { get; set; }
    public decimal? DiscountAmount { get; set; } // Số tiền giảm cụ thể
    public decimal? DiscountPercentage { get; set; } // Phần trăm giảm giá
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsGlobal { get; set; }
    public bool? isUsed { get; set; }
    public int? StoreId { get; set; }
}