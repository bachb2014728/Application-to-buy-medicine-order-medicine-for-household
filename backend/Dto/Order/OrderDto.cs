using backend.Data;
using backend.Dto.Order.OrderDetail;
using backend.Dto.Voucher;

namespace backend.Dto.Order;

public class OrderDto
{
    public int Id { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? ReceiverAddress { get; set; }
    public List<OrderDetailItem>? OrderDetails { get; set; } = [];
    public DateTime CreatedOn { get; set; }
    public StatusData? OrderStatus { get; set; }
    public string? Payment { get; set; }
    public List<VoucherItem>? Vouchers { get; set; } = [];
    public int? CustomerId { get; set; }
}