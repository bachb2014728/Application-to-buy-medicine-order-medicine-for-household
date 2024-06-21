using backend.Dto.Category;
using backend.Dto.Product.Comment;
using backend.Dto.Product.Contraindication;
using backend.Dto.Product.DosageForm;
using backend.Dto.Product.Manufacturer;
using backend.Dto.Product.Use;
using backend.Dto.Store;

namespace backend.Dto.Product;

public class ProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }
    public required decimal Sale { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool? Status { get; set; }
    public required string? Content { get; set; }
    public List<UseItem> Uses{ get; set; } =[];
    public ManufacturerItem? Manufacturer { get; set; }
    public List<DosageFormItem> DosageForms{ get; set; } = [];
    public List<ContraindicationItem> Contraindications { get; set; } = [];
    
    public List<CategoryItem>? Categories { get; set; } = [];
    public List<int>? Images { get; set; } = [];
    
    public List<CommentDto>? Comments { get;} = [];
    
    public StoreItem? CreatedBy { get; set; }
}