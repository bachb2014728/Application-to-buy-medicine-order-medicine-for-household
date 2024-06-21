using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("CustomersVouchers")]
public class CustomerVouchers
{
    public int Id { get; set; }
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public required Customer Customer { get; set; }
    [ForeignKey("Voucher")]
    public int VoucherId { get; set; }
    public required Voucher Voucher { get; set; }
    public bool IsUsed { get; set; }
}