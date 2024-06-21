

using backend.Dto;
using backend.Dto.Category;
using backend.Model;

namespace backend.Interface
{
    public interface ICategory
    {
        Task<ApiObject> DeleteOne(int id);
        Task<ApiObject> DeleteAll();
        Task<List<Category>> GetAll();
        Task<CategoryDto> GetOne(int id);
        Task<CategoryDto> Put(int id, CategoryUpdate category);
        Task<CategoryDto> Save(CategoryCreate category);
        Task<CategoryDto> GetOneByURL(string url);
    }
}