
using backend.Dto;
using backend.Dto.Product;
using backend.Model;

namespace backend.Interface
{
    public interface IProduct
    {
        Task<ProductDto> Create(CreateProductRequest createProductRequest); 
        Task<IEnumerable<ProductItem>> GetAll(); 
        Task<ProductDto> GetById(int id);
        Task<ProductDto?> Update(UpdateProductRequest updateProductRequest, int id);
        Task<ApiObject?> Delete(int id);
        Task<ProductDto?> GetByUrl(string url);
    }
}