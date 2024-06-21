namespace backend.Dto.Cart;

public class CartCreate
{
    public required int ProductId { get; set; }
    public required int Quantity { get; set; }
}