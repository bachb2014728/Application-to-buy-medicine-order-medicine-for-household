namespace backend.Dto.Product.Contraindication;

public class ContraindicationCreate
{
    public required string Name { get; init; }
    public string? Info { get; init; }
}