using backend.Data;
using backend.Dto;
using backend.Dto.Store;
using backend.Dto.Voucher;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class VoucherService(ApplicationDbContext context,ConvertInformation convert) : IVoucher
{
    private readonly ApplicationDbContext _context = context;
    private readonly ConvertInformation _convert = convert;
    private readonly DateTime _now = DateTime.UtcNow.AddHours(7);
    public async Task<IEnumerable<VoucherItem>> GetAll()
    {
        var listItem = await _context.Vouchers.Where(v=>v.EndTime > _now).ToListAsync();
        if (listItem.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu.");
        }
        return listItem.Select(ToVoucherItem);
    }
    public async Task<VoucherDto?> GetOne(int id)
    {
        var voucher = await VoucherExist(id);
        return await ToVoucherDto(voucher);

    }

    public async Task<VoucherDto?> Create(VoucherCreate voucherCreate)
    {
        if (_now > voucherCreate.EndTime)
        {
            throw new InvalidFormatException("Thời gian cuối sự kiện đã qua thời gian hiện tại.");
        }

        if (voucherCreate.EndTime < voucherCreate.StartTime)
        {
            throw new InvalidFormatException("Thời gian cuối sự kiện sớm hơn thời gian bắt đầu.");
        }
        var listItem = await _context.Vouchers.ToListAsync();
        var newItem = new Voucher()
        {
            Title = voucherCreate.Title,
            Code = GenerateVoucherCode(listItem),
            CreatedOn = _now,
            DiscountAmount = voucherCreate.DiscountAmount,
            DiscountPercentage = voucherCreate.DiscountPercentage,
            StartTime = voucherCreate.StartTime.ToUniversalTime(),
            EndTime = voucherCreate.EndTime.ToUniversalTime(),
            StoreId = voucherCreate.StoreId == 0 ? null : voucherCreate.StoreId
        };
        await _context.Vouchers.AddAsync(newItem);
        
        var customers = await _context.Customers.ToListAsync();
        foreach (var notification in customers.Select(customer => new Notification()
                 {
                     CustomerId = customer.Id,
                     Message = $"Voucher {newItem.Title} có 1 mã khuyến mãi mới.",
                     IsRead = false,
                     CreatedOn = _now
                 }))
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
        await _context.SaveChangesAsync();
        return await ToVoucherDto(newItem);
    }

    public async Task<VoucherDto?> Update(VoucherUpdate voucherUpdate, int id)
    {
        var voucher = await VoucherExist(id);
        if (_now > voucherUpdate.EndTime)
        {
            throw new InvalidFormatException("Thời gian cuối sự kiện đã qua thời gian hiện tại.");
        }
        if (voucherUpdate.EndTime < voucherUpdate.StartTime)
        {
            throw new InvalidFormatException("Thời gian cuối sự kiện sớm hơn thời gian bắt đầu.");
        }
        voucher.Title = voucherUpdate.Title ?? voucher.Title;
        voucher.DiscountAmount = voucherUpdate.DiscountAmount;
        voucher.DiscountPercentage = voucherUpdate.DiscountPercentage;
        voucher.StartTime = voucherUpdate.StartTime.ToUniversalTime();
        voucher.EndTime = voucherUpdate.EndTime.ToUniversalTime();
        
        var customers = await _context.CustomerVouchers
            .Where(cv => cv.VoucherId == id)
            .Select(cv => cv.CustomerId)
            .ToListAsync();
        foreach (var notification in customers.Select(customerId => new Notification
                 {
                     CustomerId = customerId,
                     Message = $"Voucher {voucher.Title} đã được cập nhật.",
                     IsRead = false,
                     CreatedOn = _now
                 }))
        {
            _context.Notifications.Add(notification);
        }
        
        await _context.SaveChangesAsync();
        return await ToVoucherDto(voucher);
    }

    public async Task<ApiObject?> DeleteOne(int id)
    {
        var voucher = await VoucherExist(id);
        var listOrderVouchers = await _context.OrderVouchers.Where(or => or.VoucherId == id).ToListAsync();
        
        foreach (var item in listOrderVouchers)
        {
            _context.OrderVouchers.Remove(item);
            await _context.SaveChangesAsync();
        }

        var listCustomerVouchers = await _context.CustomerVouchers.Where(cu => cu.VoucherId == id).ToListAsync();
        foreach (var item in listCustomerVouchers)
        {
            _context.CustomerVouchers.Remove(item);
            await _context.SaveChangesAsync();
        }

        _context.Vouchers.Remove(voucher);
        await _context.SaveChangesAsync();
        return new ApiObject()
        {
            Message = "Xóa mã khuyến mãi thành công."
        };
    }

    public async Task<ApiObject?> AddToVoucher(int id)
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Id == id);
        if (voucher == null)
        {
            throw new NotFoundException($"Không tìm thấy khuyến mãi với id = {id}");
        }

        var customerVoucherExist = await _context.CustomerVouchers
            .FirstOrDefaultAsync(x => x.VoucherId == id && x.CustomerId == customer.Id);
        if (customerVoucherExist != null)
        {
            throw new AlreadyExistsException("Mã khuyến mãi đã được lưu trước đó.");
        }
        var customerVoucher = new CustomerVouchers
        {
            Customer = customer,
            Voucher = voucher,
            IsUsed = false
        };
        await _context.CustomerVouchers.AddAsync(customerVoucher);
        await _context.SaveChangesAsync();
        return new ApiObject()
        {
            Message = "Luư khuyến mãi thành công."
        };
    }

    public async Task<IEnumerable<VoucherItem>> MyListVoucher()
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var listItem = await _context.CustomerVouchers
            .Where(v => v.CustomerId == customer.Id && v.IsUsed == false)
            .Select(v=>v.VoucherId)
            .ToListAsync();
        if (listItem.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu.");
        }
        var listVoucher = await _context.Vouchers.Where(x => listItem.Contains(x.Id)).ToListAsync();
        return listVoucher.Select(ToVoucherItem);
    }


    /*public void NotifyVouchersExpiringSoon()
    {
        var tomorrow = DateTime.UtcNow.AddHours(7).AddDays(1);
        var vouchersExpiringSoon = _context.Vouchers.Where(v => v.EndTime.Date == tomorrow.Date).ToList();

        foreach (var notification in from voucher in vouchersExpiringSoon let customers = _context.CustomerVouchers
                     .Where(cv => cv.VoucherId == voucher.Id)
                     .Select(cv => cv.CustomerId)
                     .ToList() from notification in customers.Select(customerId => new Notification
                 {
                     CustomerId = customerId,
                     Message = $"Voucher {voucher.Title} sẽ hết hạn vào ngày mai.",
                     IsRead = false,
                     CreatedOn = _now
                 }) select notification)
        {
            _context.Notifications.Add(notification);
        }

        _context.SaveChanges();
    }*/

    private async Task<VoucherDto> ToVoucherDto(Voucher voucher)
    {
        if (voucher.StoreId == null)
            return new VoucherDto
            {
                Id = voucher.Id,
                Title = voucher.Title,
                Code = voucher.Code,
                DiscountAmount = voucher.DiscountAmount,
                DiscountPercentage = voucher.DiscountPercentage,
                CreatedOn = voucher.CreatedOn,
                StartTime = voucher.StartTime,
                EndTime = voucher.EndTime
            };
        var store = await _context.Stores.Include(store => store.Avatar).FirstOrDefaultAsync(v => v.Id == voucher.StoreId);
        var storeItem = ToStoreItem(store);
        return new VoucherDto
        {
            Id = voucher.Id,
            Title = voucher.Title,
            Code = voucher.Code,
            DiscountAmount = voucher.DiscountAmount,
            DiscountPercentage = voucher.DiscountPercentage,
            CreatedOn = voucher.CreatedOn,
            StartTime = voucher.StartTime,
            EndTime = voucher.EndTime,
            Store = storeItem
        };
    }

    private static StoreItem ToStoreItem(Store store)
    {
        return new StoreItem()
        {
            Id = store.Id,
            Name = store.Name,
            URL = store.URL,
            Avatar = store.Avatar?.Id
        };
    }
    private async Task<Voucher> VoucherExist(int id)
    {
        var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Id == id);
        if (voucher == null)
        {
            throw new NotFoundException($"Không tìm thấy voucher với id = {id}");
        }
        return voucher;

    }
    private static VoucherItem ToVoucherItem(Voucher voucher)
    {
        
        return new VoucherItem
        {
            Id = voucher.Id,
            Title = voucher.Title,
            Code = voucher.Code,
            DiscountAmount = voucher.DiscountAmount,
            DiscountPercentage = voucher.DiscountPercentage,
            StartTime = voucher.StartTime,
            EndTime = voucher.EndTime,
            IsGlobal = voucher.StoreId == null ? true : false
        };
    }

    private static string GenerateVoucherCode(List<Voucher> listItem)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[12];
        var random = new Random();
        string voucherCode;

        do
        {
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            voucherCode = new string(stringChars);
        }
        while (listItem.Any(item => item.Code == voucherCode));
        return voucherCode;
    }


}