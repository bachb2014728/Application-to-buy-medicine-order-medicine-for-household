using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Carts")]
public class Cart
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow.AddHours(7);
    public bool IsChecked { get; set; } = false;
    public required int Quantity { get; set; }
    public int? CustomerId { get; set; }
}