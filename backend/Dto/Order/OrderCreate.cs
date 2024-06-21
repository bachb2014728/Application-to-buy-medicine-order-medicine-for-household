using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Dto.Order;

public class OrderCreate
{ 
    public decimal? TotalPrice { get; set; }
    public required string ReceiverName { get; set; }
    public required string ReceiverPhone { get; set; }
    public required string ReceiverAddress { get; set; }
    public List<int>? CartIds { get; set; } = [];
    public List<int>? VoucherIds { get; set; } = [];
    public int? PaymentId { get; set; }
}