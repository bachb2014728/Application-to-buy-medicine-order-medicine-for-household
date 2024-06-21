

using backend.Dto;
using backend.Dto.Image;
using backend.Dto.Store;
using backend.Model;

namespace backend.Interface
{
    public interface IStore
    {
        Task<ApiObject> DeleteOne(int id);
        Task<List<Store>> GetAll();
        Task<List<Store>> GetAllByUser();
        Task<Store> GetOne(int id);
        Task<StoreDto> GetOneByURL(string url);
        Task<Store> Post(StoreCreate store);
        Task<Store> Update(StoreUpdate store, int id);
        Task<byte[]> UpdateAvatar(AddImage image, string url);
        Task<byte[]> UpdateBackground(AddImage image, string url);
        Task<Store> UpdateInfo(StoreUpdate store, string url);
    }
}