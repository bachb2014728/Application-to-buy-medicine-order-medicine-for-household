using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("OrderVouchers")]
public class OrderVouchers
{
    public int Id { get; set; }
    [ForeignKey("Order")]
    public int? OrderId { get; set; }
    public Order? Order { get; set; }
    
    [ForeignKey("Voucher")]
    public int? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }
}