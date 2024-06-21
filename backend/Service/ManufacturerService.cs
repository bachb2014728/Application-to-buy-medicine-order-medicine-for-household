using backend.Data;
using backend.Dto;
using backend.Dto.Product.Manufacturer;
using backend.Error;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class ManufacturerService(ApplicationDbContext context) : IManufacturer
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<ManufacturerDto> CreateAsync(ManufacturerCreate manufacturerCreate)
        {
            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == manufacturerCreate.Name.ToUpper());
            if (manufacturer != null)
            {
                throw new AlreadyExistsException($"{manufacturerCreate.Name} đã tồn tại.");
            }
            var item = new Manufacturer{
                Name = manufacturerCreate.Name,
                Info = manufacturerCreate.Info,
            };
            await _context.Manufacturers.AddAsync(item);
            await _context.SaveChangesAsync();
            return ConvertManufacturerDto(item);
        }

        public async Task<IEnumerable<ManufacturerDto>> GetAllAsync()
        {
            var manufacturers = await _context.Manufacturers.OrderBy(u => u.Id).ToListAsync();
            if (manufacturers.Count == 0){
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            return manufacturers.Select(ConvertManufacturerDto);
        }

        public async Task<ManufacturerDto> GetOneByIdAsync(int id)
        {
            var item = await ManufacturerExist(id);
            return ConvertManufacturerDto(item);
        }

        private async Task<Manufacturer> ManufacturerExist(int id)
        {
            var item = await _context.Manufacturers
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"không tìm thấy dữ liệu với {id}");
            return item;
        }

        private static ManufacturerDto ConvertManufacturerDto(Manufacturer manufacturer)
        {
            return new ManufacturerDto()
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Info = manufacturer.Info
            };
        }

        public async Task<ApiObject> RemoveOneByIdAsync(int id)
        {
            var item = await ManufacturerExist(id);
            _context.Manufacturers.Remove(item);
            await _context.SaveChangesAsync();
            return new ApiObject {
                Message = "Xóa thành công"
            };
        }

        public async Task<ManufacturerDto> UpdateOneByIdAsync(int id, ManufacturerUpdate manufacturerUpdate)
        {
            var item = await ManufacturerExist(id);
            var manufactureItem = await _context.Manufacturers
                .Where(x => x != item)
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == manufacturerUpdate.Name.ToUpper());
            if (manufactureItem != null)
            {
                throw new AlreadyExistsException($"{manufacturerUpdate.Name} đã tồn tại.");
            }
            item.Name =manufacturerUpdate.Name;
            item.Info = manufacturerUpdate.Info;
            await _context.SaveChangesAsync();
            return ConvertManufacturerDto(item);
        }
    }
}