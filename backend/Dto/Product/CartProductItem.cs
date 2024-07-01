namespace backend.Dto.Product;

public class CartProductItem
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? ImageId { get; set; }
    public string? URL { get; set; }
    public decimal? Price { get; set; }
    public decimal? Sale { get; set; }
}