using backend.Data;
using backend.Dto.Cart;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class CartService(ApplicationDbContext context, ConvertInformation convert) : ICart
{
    private readonly ApplicationDbContext _context = context;
    private readonly ConvertInformation _convert = convert;
    public async Task<List<Cart>> GetAllByCustomer()
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var listItem = await _context.Carts.Where(x => x.CustomerId == customer.Id).ToListAsync();
        if (listItem == null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu giỏ hàng.");
        }
        foreach (var item in listItem)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product != null) continue;
            _context.Carts.Remove(item);
            await _context.SaveChangesAsync();
        }
        return listItem;
    }

    public async Task<Cart> AddToCart(CartCreate cart)
    {
        var user =_convert.ToAppUser(GlobalVariables.Token);
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.User == user);
        if (customer == null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu của khách hàng.");
        }
        var cartItem = await _context.Carts.Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.CustomerId == customer.Id);
        if (cartItem != null)
        {
            cartItem.Quantity += cart.Quantity;
            if (cartItem.Quantity == 0)
            {
                _context.Carts.Remove(cartItem);
            }
            await _context.SaveChangesAsync();
            return cartItem;
        }
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == cart.ProductId);
        if (product == null)
        {
            throw new NotFoundException("Không tìm thấy sản phẩm để thêm vào giỏ hàng.");
        }

        var newCart = new Cart()
        {
            Product = product,
            CreatedOn = DateTime.UtcNow,
            Quantity = cart.Quantity <= 0 
                ? throw new AlreadyExistsException("Giá trị không thể âm khi thêm vào giỏ hàng: Quantity") 
                : cart.Quantity,
            CustomerId = customer.Id
        };
        await _context.Carts.AddAsync(newCart);
        await _context.SaveChangesAsync();
        return newCart;
    }
    
    public async Task DeleteOne(int id)
    {
        var itemCart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == id);
        if (itemCart == null)
        {
            throw new NotFoundException("Đã xóa hoặc không tồn tại sản phẩm đó trong giỏ hàng.");
        }

        _context.Carts.Remove(itemCart);
        await _context.SaveChangesAsync();
        return;
    }

    private CartItem ToCartItem(Cart cart)
    {
        
        return new CartItem
        {
            Id = cart.Id,
            Product = null,
            CreatedOn = cart.CreatedOn,
            Quantity = cart.Quantity
        };
    }
}