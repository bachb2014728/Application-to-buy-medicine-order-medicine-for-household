
using backend.Data;
using backend.Dto;
using backend.Dto.Customer;
using backend.Dto.Image;
using backend.Dto.Product;
using backend.Dto.Store;
using backend.Dto.Voucher;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class StoreService(ApplicationDbContext context, ConvertInformation convert) : IStore
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ConvertInformation _convert = convert;
        public async Task<ApiObject> DeleteOne(int id)
        {
            var item = await _context.Stores.FirstOrDefaultAsync(x=>x.Id == id) ?? throw new NotFoundException($"Không thể tìm thấy dữ liệu với : {id}");
            item.Status = "BLOCK";
            await _context.SaveChangesAsync();
            return new ApiObject{
                Message = "Khóa nhà thuốc thành công."
            };
        }

        public async Task<List<Store>> GetAll()
        {
            var listItem = await _context.Stores
                .Include(x=>x.Avatar)
                .Include(x=>x.CreatedBy)
                .ToListAsync();
            if(listItem.Count == 0){
                throw new NotFoundException("Không tìm thấy dữ liệu nào.");
            }
            return listItem;
        }

        public async Task<List<Store>> GetAllByUser()
        {
            var user = _convert.ToAppUser(GlobalVariables.Token) ?? throw new NotFoundException("Khong tim thay du lieu");
            var listItem = await _context.Stores
                .Include(x=>x.Avatar)
                .Include(x=>x.CreatedBy)
                .Where(x=>x.CreatedBy == user).ToListAsync();
            return listItem;
        }

        public async Task<Store> GetOne(int id)
        {
            var item = await _context.Stores
                .Where(x=>x.Status == "ACTIVE")
                .Include(x=>x.Background)
                .Include(x=>x.Avatar)
                .FirstOrDefaultAsync(x=>x.Id == id) ?? throw new NotFoundException($"Không thể tìm thấy dữ liệu với : {id}");
            return item;
        }

        public async Task<StoreDto> GetOneByURL(string url)
        {
            var item = await _context.Stores
                .Where(x=>x.Status == "ACTIVE")
                .Include(x=>x.Background)
                .Include(x=>x.CreatedBy)
                .Include(x=>x.Avatar)
                .FirstOrDefaultAsync(x=>x.URL == url) ?? throw new NotFoundException($"Không thể tìm thấy dữ liệu với : {url}");
            return await ToStoreDto(item);
        }

        private async Task<StoreDto> ToStoreDto(Store store)
        {
            var customer = await _context.Customers.Include(customer => customer.User)
                .FirstOrDefaultAsync(x => x.User == store.CreatedBy);
            if (customer == null)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            
            var user = _convert.ToAppUser(GlobalVariables.Token);
            var isBoss = customer.User == user;
            var customerItem = new CustomerItem
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address
            };
            var products = await _context.Products
                .Where(x => x.CreatedBy == store).ToArrayAsync();
            List<VoucherItem> voucherItems = [];
            var vouchers = await _context.Vouchers.Where(x => x.StoreId == store.Id).ToListAsync();
            voucherItems.AddRange(vouchers.Select(ToVoucherItem));
            
            List<ProductItem> productItems = [];
            productItems.AddRange(products.Select(item => new ProductItem
            {
                Id = item.Id,
                Name = item.Name,
                URL = item.URL,
                Quantity = item.Quantity,
                Price = item.Price,
                Sale = item.Sale,
                ImageId = item.ImageIds?[0]
            }));
            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                URL = store.URL,
                Avatar = store.Avatar?.Id,
                Background = store.Background?.Id,
                Info = store.Info,
                Followers = null,
                Address = store.Address,
                Star = 0,
                IsBoss = isBoss,
                Status = store.Status,
                CreatedAt = store.CreatedAt,
                CreatedBy = customerItem,
                Products = productItems,
                Vouchers = voucherItems
            };
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
                IsGlobal = false
            };
        }
        public async Task<Store> Post(StoreCreate storeCreate)
        {
            if (await _context.Stores.AnyAsync(x => x.Name == storeCreate.Name)){
                throw new AlreadyExistsException($"{storeCreate.Name} đã tồn tại.");
            }
            var role = _convert.ToRole(GlobalVariables.Token);
            var store = new Store {
                Name = storeCreate.Name,
                Info = storeCreate.Info,
                URL = storeCreate.URL,
                Address = storeCreate.Address,
                Status = "ACTIVE",
                CreatedAt = DateTime.UtcNow.AddHours(7),
                CreatedBy = _convert.ToAppUser(GlobalVariables.Token)
            };
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<Store> Update(StoreUpdate storeUpdate, int id)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(x=>x.Id == id) ?? throw new NotFoundException("Không tìm thấy dữ liệu.");
            var item = await _context.Stores.Where(x=>x.Id != id).FirstOrDefaultAsync(x=>x.Name == storeUpdate.Name);
            if (item != null){
                throw new AlreadyExistsException($"{storeUpdate.Name} đã tồn tại.");
            }
            
            store.Name = storeUpdate.Name;
            store.Address = storeUpdate.Address;
            store.Info = storeUpdate.Info;
            store.URL = storeUpdate.URL;
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<byte[]> UpdateAvatar(AddImage imageId, string url)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(x=>x.URL == url) ?? throw new NotFoundException("Không tìm thấy dữ liệu.");
            var image = await _context.Images.FirstOrDefaultAsync(x=>x.Id == imageId.Image) ?? throw new NotFoundException("Không tìm thấy dữ liệu.");
            store.Avatar = image;
            await _context.SaveChangesAsync();
            return image.File;                                                        
            
        }

        public async Task<byte[]> UpdateBackground(AddImage imageId, string url)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(x=>x.URL == url) ?? throw new NotFoundException("Không tìm thấy dữ liệu.");
            var image = await _context.Images.FirstOrDefaultAsync(x=>x.Id == imageId.Image) ?? throw new NotFoundException("Không tìm thấy dữ liệu.");
            store.Background = image;
            await _context.SaveChangesAsync();
            return image.File;         
        }

        public async Task<Store> UpdateInfo(StoreUpdate storeUpdate, string url)
        {
            
            var store = await _context.Stores.FirstOrDefaultAsync(x=>x.URL == url) ?? throw new NotFoundException("Không tìm thấy dữ liệu.");
            var item = await _context.Stores.Where(x=>x.URL != url).FirstOrDefaultAsync(x=>x.Name == storeUpdate.Name);
            if (item != null){
                throw new AlreadyExistsException($"{storeUpdate.Name} đã tồn tại.");
            }
            store.Name = storeUpdate.Name;
            store.Address = storeUpdate.Address;
            store.Info = storeUpdate.Info;
            store.URL = storeUpdate.URL;
            await _context.SaveChangesAsync();
            return store;
        }
    }
}