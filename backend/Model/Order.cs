using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Orders")]
public class Order
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public required decimal? TotalPrice { get; set; }
    public required string ReceiverName { get; set; }
    public required string ReceiverPhone { get; set; }
    public required string ReceiverAddress { get; set; }
    public List<int>? OrderDetailIds { get; set; } = [];
    public List<OrderDetail>? OrderDetails { get; set; } = [];
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow.AddHours(7);
    public required string OrderStatus { get; set; }
    [ForeignKey("Customer")]
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
}