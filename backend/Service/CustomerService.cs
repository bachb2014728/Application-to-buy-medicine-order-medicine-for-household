using System.Globalization;
using backend.Data;
using backend.Dto;
using backend.Dto.Customer;
using backend.Dto.Product;
using backend.Dto.Store;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class CustomerService(ApplicationDbContext context, ConvertInformation convert) : ICustomer
{
    private readonly ApplicationDbContext _context = context;
    private readonly ConvertInformation _convert = convert;
    public async Task<IEnumerable<CustomerItem>?> GetAll()
    {
        var customers = await _context.Customers
            .Include(x=>x.User)
            .ToListAsync();
        if (customers.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy thành viên nào .");
        }

        return customers.Select(ToCustomerItem);
    }

    public async Task<CustomerFullview?> GetOne(int id)
    {
        var customer = await _context.Customers
            .Include(x=>x.Carts)
            .Include(x=>x.Orders)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x=>x.Id == id);
        if (customer == null)
        {
            throw new NotFoundException($"không tìm thấy người dùng với id = {id}");
        }
        return await ToProfile(customer);
    }

    public async Task<CustomerFullview?> GetProfile() 
    {
        var user = _convert.ToAppUser(GlobalVariables.Token);
        Console.WriteLine($"Kiem tra thoi token = {GlobalVariables.Token}");
        var customer = await _context.Customers
            .Include(x=>x.Carts)
            .Include(x=>x.Orders)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x=>x.User == user);
        if (customer == null)
        {
            throw new NotFoundException($"không tìm thấy người dùng.");
        }
        return await ToProfile(customer);
    }

    public async Task<CustomerFullview?> Update(CustomerUpdate customerUpdate, int id)
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        if (customer.Id != id)
        {
            throw new RoleNotFoundException("Không có quyền chỉnh sử thông tin cá nhân.");
        }

        customer.FirstName = customerUpdate.FirstName ?? throw new NotFoundException("Không nhập họ người dùng");
        customer.LastName = customerUpdate.LastName ?? throw new NotFoundException("Không nhập tên người dùng.");
        var isZipCode = await _context.Customers
            .Where(x=>x.Id != customer.Id)
            .FirstOrDefaultAsync(x => x.ZipCode == customerUpdate.ZipCode);
        if (customerUpdate.ZipCode != null && isZipCode != null)
        {
            throw new AlreadyExistsException($"Căn cước công dân đã tồn tại ${customerUpdate.ZipCode}");
        }
        customer.ZipCode = customerUpdate.ZipCode ?? throw new NotFoundException("Không nhập cccd người dùng");
        customer.Gender = customerUpdate.Gender;
        var isEmail = await _context.Users.Where(x => x.Id != customer.UserId)
            .FirstOrDefaultAsync(x => x.Email == customerUpdate.Email);
        if (isEmail !=null)
        {
            throw new AlreadyExistsException($"Địa chỉ email đã tồn tại với {customerUpdate.Email}");
        }

        var isPhone = await _context.Users.Where(x => x.Id != customer.UserId)
            .FirstOrDefaultAsync(x => x.PhoneNumber == customerUpdate.PhoneNumber);
        if (isPhone != null)
        {
            throw new AlreadyExistsException($"Số điện thoại đã tồn tại với {customerUpdate.PhoneNumber}");
        }
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == customer.UserId);
        if (user == null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu người dùng.");
        }

        user.Email = customerUpdate.Email ?? user.Email;
        user.PhoneNumber = customerUpdate.PhoneNumber ?? user.PhoneNumber;
        customer.Address = customerUpdate.Address ?? customer.Address;
        await _context.SaveChangesAsync();
        
        return await ToProfile(customer);

    }

    private async Task<CustomerFullview> ToProfile(Customer customer)
    {
        List<StoreItem> storeItems = [];
        /*var stores = await _context.Stores.Where(x => x.CreatedBy.Id == customer.User.Id).ToListAsync();
        storeItems.AddRange(stores.Select(ToStoreItem));*/
        
        return new CustomerFullview
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Address = customer.Address,
            Gender = customer.Gender,
            ZipCode = customer.ZipCode,
            Username = customer.User.UserName ,
            Email = customer.User.Email,
            PhoneNumber = customer.User.PhoneNumber,
            Stores = storeItems
        };
    }

    private static StoreItem ToStoreItem(Store store)
    {
        return new StoreItem
        {
            Id = store.Id,
            Name = store.Name,
            URL = store.URL,
            Avatar = store.Avatar?.Id
        };
    }
    

    private static CartProductItem ToProduct(Product? product)
    {
        
        return new CartProductItem
        {
            Id = product.Id,
            Name = product.Name,
            ImageId = product.ImageIds[0],
            URL = product.URL,
            Price = product.Price,
            Sale = product.Sale
        };
    }
    private static CustomerItem ToCustomerItem(Customer customer)
    {
        return new CustomerItem
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Address = customer.Address,
            ZipCode = customer.ZipCode,
            Username = customer.User.UserName,
            Email = customer.User.Email
        };
    }
}