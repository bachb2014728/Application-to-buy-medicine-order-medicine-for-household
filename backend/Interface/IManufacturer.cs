using backend.Dto;
using backend.Dto.Product.Manufacturer;

namespace backend.Interface
{
    public interface IManufacturer
    {
        Task<ManufacturerDto> CreateAsync(ManufacturerCreate manufacturerCreate);
        Task<IEnumerable<ManufacturerDto>> GetAllAsync();
        Task<ManufacturerDto> GetOneByIdAsync(int id);
        Task<ApiObject> RemoveOneByIdAsync(int id);
        Task<ManufacturerDto> UpdateOneByIdAsync(int id, ManufacturerUpdate manufacturerUpdate);
    }
}