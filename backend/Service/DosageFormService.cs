using backend.Data;
using backend.Dto;
using backend.Dto.Product.DosageForm;
using backend.Error;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class DosageFormService(ApplicationDbContext context) : IDosageForm
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<DosageFormDto> Create(DosageFormCreate dosageFormCreate)
        {
            var dosageForm = await _context.DosageForms.FirstOrDefaultAsync(x =>
                    x.Name.ToUpper() == dosageFormCreate.Info.ToUpper());
            if (dosageForm != null)
            {
                throw new AlreadyExistsException($"{dosageFormCreate.Info} đã tồn tại.");
            }
            var item = new DosageForm{
                Name = dosageFormCreate.Name,
                Info = dosageFormCreate.Info,
            };
            await _context.DosageForms.AddAsync(item);
            await _context.SaveChangesAsync();
            return ConvertDosageFormDto(item);
        }
        public async Task<IEnumerable<DosageFormDto>> GetAll()
        {
            var listItem = await _context.DosageForms.OrderBy(u => u.Id).ToListAsync();
            if (listItem.Count == 0){
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            return listItem.Select(ConvertDosageFormDto);
        }

        public async Task<DosageFormDto> GetOne(int id)
        {
            var item = await DosageFormExist(id);
            return ConvertDosageFormDto(item);
        }

        private static DosageFormDto ConvertDosageFormDto(DosageForm dosageForm)
        {
            return new DosageFormDto()
            {
                Id = dosageForm.Id,
                Name = dosageForm.Name,
                Info = dosageForm.Info,
            };
        }

        private async Task<DosageForm> DosageFormExist(int id)
        {
            var item = await _context.DosageForms.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"không tìm thấy dữ liệu với {id}");
            return item;
        }

        public async Task<ApiObject> DeleteOne(int id)
        {
            var item = await DosageFormExist(id);
            _context.DosageForms.Remove(item);
            await _context.SaveChangesAsync();
            return new ApiObject {
                Message = "Xóa thành công"
            };
        }
        public async Task<DosageFormDto> Update(int id, DosageFormUpdate dosageFormUpdate)
        {
            var item = await DosageFormExist(id);
            var dosageItem = await _context.DosageForms.Where(x => x == item)
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == dosageFormUpdate.Info.ToUpper());
            if (dosageItem != null)
            {
                throw new AlreadyExistsException($"{dosageFormUpdate.Info} đã tồn tại.");
            }
            item.Name = dosageFormUpdate.Name;
            item.Info = dosageFormUpdate.Info;
            await _context.SaveChangesAsync();
            return ConvertDosageFormDto(item);
        }
    }
}