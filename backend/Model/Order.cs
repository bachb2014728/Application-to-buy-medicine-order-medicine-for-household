using System.ComponentModel.DataAnnotations.Schema;
using backend.Data;

namespace backend.Model;
[Table("Orders")]
public class Order
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(18,2)")] 
    public required decimal TotalPrice { get; set; }
    public required string ReceiverName { get; set; }
    public required string ReceiverPhone { get; set; }
    public required string ReceiverAddress { get; set; }
    public List<int>? OrderDetailIds { get; set; } = [];
    public List<OrderDetail>? OrderDetails { get; set; } = [];
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow.AddHours(7);
    public StatusData OrderStatus { get; set; }
    
    public List<int>? VoucherIds { get; set; } = [];
    public List<Voucher>? Vouchers { get; set; } = [];
    public required string Payment { get; set; }
    public int? StoreId { get; set; }
    public Store? Store { get; set; }
    [ForeignKey("Customer")]
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
}