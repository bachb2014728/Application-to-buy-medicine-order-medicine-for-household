using backend.Dto.Store;

namespace backend.Dto.Product;

public class ProductItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? ImageId { get; set; }
    public required string URL { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }
    public required decimal Sale { get; set; }
    public bool Status { get; set; }
    public StoreItem? CreatedBy { get; set; }
}