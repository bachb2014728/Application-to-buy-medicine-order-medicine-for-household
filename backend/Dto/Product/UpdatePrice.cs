namespace backend.Dto.Product;

public class UpdatePrice
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public decimal Sale { get; set; }
}