namespace backend.Dto.Product.Contraindication;

public class ContraindicationDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Info { get; set; }
    public List<ProductItem> Products { get; set; } = [];
}