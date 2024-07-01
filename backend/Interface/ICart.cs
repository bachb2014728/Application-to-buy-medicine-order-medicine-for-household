using backend.Dto;
using backend.Dto.Cart;
using backend.Model;

namespace backend.Interface;

public interface ICart
{
    Task<IEnumerable<CartItem>> GetAllByCustomer();
    Task<Cart> AddToCart(CartCreate cart);
    Task DeleteOne(int id);
    Task<CartItem?> Update(CartUpdate cart, int id);
    Task<ApiObject?> Checked(IsChecked isChecked);
    Task<IEnumerable<CartsByStore>?> CartIsChecked();
}