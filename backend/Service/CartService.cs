using backend.Data;
using backend.Dto;
using backend.Dto.Cart;
using backend.Dto.Store;
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
    public async Task<IEnumerable<CartItem>> GetAllByCustomer()
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var listItem = await _context.Carts
            .Include(x=>x.Product)
            .Where(x => x.CustomerId == customer.Id).ToListAsync();
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

        List<CartItem> items = [];
        foreach (var item in listItem)
        {
            var cart = await ToCartItem(item);
            items.Add(cart);
        }
        return items;
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
            .FirstOrDefaultAsync(x => x.CustomerId == customer.Id && x.ProductId == cart.ProductId);
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
            CreatedOn = DateTime.UtcNow.AddHours(7),
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

    public async Task<CartItem?> Update(CartUpdate cart, int id)
    {
        var cartItem = await _context.Carts
            .Include(x=>x.Product)
            .FirstOrDefaultAsync(x => x.Id == cart.CartId);
        if (cartItem==null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu.");
        }

        if (cart.Quantity == 1)
        {
            cartItem.Quantity += 1;
            await _context.SaveChangesAsync();
            return await ToCartItem(cartItem);
        }
        
        if (cartItem.Quantity == 1)
        {
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return await ToCartItem(cartItem);
        }

        cartItem.Quantity -= 1;
        await _context.SaveChangesAsync();
        return await ToCartItem(cartItem);
    }

    public async Task<ApiObject?> Checked(IsChecked isChecked)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(item => item.Id == isChecked.IdCart);
        if (cart == null)
        {
            throw new NotFoundException("Không tìm thấy sản phẩm trong giỏ hàng");
        }

        cart.IsChecked = !cart.IsChecked;
        await _context.SaveChangesAsync();
        return new ApiObject()
        {
            Message = "Thành công."
        };
    }

    public async Task<IEnumerable<CartsByStore>?> CartIsChecked()
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var carts = await _context.Carts
            .Where(x => x.IsChecked == true &&  x.CustomerId == customer.Id).ToListAsync();
        var stores = await _context.Stores.Include(store => store.Avatar).ToListAsync();
        List<CartsByStore> items = [];
        foreach (var store in stores)
        {
            var cartByStore = new CartsByStore
            {
                Store = new StoreItem
                {
                    Id = store.Id,
                    Name = store.Name,
                    URL = store.URL,
                    Avatar = store.Avatar?.Id
                }
            };
            foreach (var cart in carts)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == cart.ProductId);
                if (product?.CreatedBy == store)
                {
                    cartByStore.CartItems.Add(await ToCartItem(cart));
                }
            }

            if (cartByStore.CartItems.Count > 0)
            {
                items.Add(cartByStore);
            }
        }

        return items;
    }

    private async Task<CartItem> ToCartItem(Cart cart)
    {
        var product = await _context.Products.Include(product => product.CreatedBy).FirstOrDefaultAsync(x => x.Id == cart.ProductId);
        if (product == null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu.");
        }
        decimal price = 0;
        price = product.Sale == 0 ? product.Price : product.Sale;
        return new CartItem
        {
            Id = cart.Id,
            ProductId = cart.ProductId,
            Name = product.Name,
            IsChecked = cart.IsChecked,
            Price = price,
            StoreId = product.CreatedBy.Id,
            Quantity = cart.Quantity
        };
    }
}