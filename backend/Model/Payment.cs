using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Payments")]
public class Payment
{
    public int Id { get; set; }
    [ForeignKey("PaymentMethod")]
    public int? PaymentMethodId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    [ForeignKey("Order")]
    public int? OrderId { get; set; }
    public Order? Order { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
    public DateTime CreateOn { get; set; } = DateTime.UtcNow.AddHours(7);
}