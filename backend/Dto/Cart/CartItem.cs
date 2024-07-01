using backend.Dto.Product;

namespace backend.Dto.Cart;

public class CartItem
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public bool? IsChecked { get; set; }
    public int? StoreId { get; set; }
    public required int Quantity { get; set; }
}