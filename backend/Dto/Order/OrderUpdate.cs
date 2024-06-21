namespace backend.Dto.Order;

public class OrderUpdate
{
    public decimal? TotalPrice { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? ReceiverAddress { get; set; }
    public List<int>? OrderDetailIds { get; set; } = [];
    public List<int>? VoucherIds { get; set; } = [];
    public string? OrderStatus { get; set; }
}