
using backend.Dto;
using backend.Dto.Product.Use;
using backend.Model;

namespace backend.Interface
{
    public interface IUse
    {
        Task<UseDto> CreateAsync(UseCreate useCreate);
        Task<IEnumerable<UseDto>> GetAllAsync();
        Task<UseDto> GetUseByIdAsync(int id);
        Task<ApiObject?> RemoveUseByIdAsync(int id);
        Task<UseDto> UpdateUseByIdAsync(int id, UseUpdate useUpdate);
    }
}