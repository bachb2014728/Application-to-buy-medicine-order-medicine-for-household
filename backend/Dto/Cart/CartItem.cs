using backend.Dto.Product;

namespace backend.Dto.Cart;

public class CartItem
{
    public int Id { get; set; }
    public ProductItem? Product { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public required int Quantity { get; set; }
}