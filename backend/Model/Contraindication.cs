using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Contraindications")]
public class Contraindication
{
    public int Id { get; set; }
    public required string? Name { get;set; }
    public string? Info { get; set; }
    public List<Product>? Products { get; set; } = [];
}