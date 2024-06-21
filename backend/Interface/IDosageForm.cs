using backend.Dto;
using backend.Dto.Product.DosageForm;

namespace backend.Interface
{
    public interface IDosageForm
    {
        Task<DosageFormDto> Create(DosageFormCreate dosageFormCreate);
        Task<IEnumerable<DosageFormDto>> GetAll();
        Task<DosageFormDto> GetOne(int id);
        Task<ApiObject> DeleteOne(int id);
        Task<DosageFormDto> Update(int id, DosageFormUpdate useUpdate);
    }
}