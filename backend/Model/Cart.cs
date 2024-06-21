using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Carts")]
public class Cart
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? PriceNow { get; set; }
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public required int Quantity { get; set; }
    public int? CustomerId { get; set; }
}