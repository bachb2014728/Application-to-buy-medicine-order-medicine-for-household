using backend.Dto.Cart;
using backend.Model;

namespace backend.Interface;

public interface ICart
{
    Task<List<Cart>> GetAllByCustomer();
    Task<Cart> AddToCart(CartCreate cart);
    Task DeleteOne(int id);
}