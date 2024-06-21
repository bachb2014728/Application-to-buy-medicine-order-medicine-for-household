using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("OrderDetails")]
public class OrderDetail
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? PriceNow { get; set; }
    public int? TotalQuantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalPrice { get; set; }
}