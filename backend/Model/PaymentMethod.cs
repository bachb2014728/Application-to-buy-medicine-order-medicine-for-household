using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("PaymentMethods")]
public class PaymentMethod
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; }
    public bool IsActive { get; set; } = true;
}