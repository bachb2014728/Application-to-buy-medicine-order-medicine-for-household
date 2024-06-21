
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;

[Table("Products")]
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string URL { get; set; }
    public required int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public required decimal Price { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public required decimal Sale { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    
    public required string? Content { get; set; }
    
    public int? ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public List<Use> Uses{ get; set; } =[];
    
    public List<DosageForm> DosageForms{ get; set; } = [];
    
    public List<Contraindication> Contraindications { get; set; } = [];
    public List<Category>? Categories { get; set; } = [];
    public List<int>? ImageIds { get; set; } = [];
    public List<Image> Images { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
    public bool Status { get; set; } = true;
    public Store? CreatedBy { get; set; }
}