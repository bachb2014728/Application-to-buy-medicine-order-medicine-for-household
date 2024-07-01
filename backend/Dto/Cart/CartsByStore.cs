using backend.Dto.Store;

namespace backend.Dto.Cart;

public class CartsByStore
{
    public List<CartItem> CartItems { get; set; } = [];
    public StoreItem? Store { get; set; }
}