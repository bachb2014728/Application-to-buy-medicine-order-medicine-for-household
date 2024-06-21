
using backend.Data;
using backend.Dto;
using backend.Dto.Product.Use;
using backend.Error;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class UseService(ApplicationDbContext context) : IUse
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<UseDto> CreateAsync(UseCreate useCreate)
        {
            var use = await _context.Uses
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == useCreate.Name.ToUpper());
            if (use == null){
                var newUse = new Use {
                    Name = useCreate.Name,
                    Info = useCreate.Info,
                };
                await _context.Uses.AddAsync(newUse);
                await _context.SaveChangesAsync();
                return ConvertUseDto(newUse);
            }else{
                throw new AlreadyExistsException($"{useCreate.Name} đã tồn tại.");
            }
        }
        public async Task<IEnumerable<UseDto>> GetAllAsync()
        {
            var uses = await _context.Uses.OrderBy(u => u.Id).ToListAsync();
            if (uses.Count > 0){
                return uses.Select(ConvertUseDto);
            }else{
                throw new NotFoundException("Không tìm thấy dữ liệu");
            }
        }

        public async Task<UseDto> GetUseByIdAsync(int id)
        {
            var use = await UseExist(id);
            return ConvertUseDto(use);
        }
        private async Task<Use> UseExist(int id)
        {
            var use = await _context.Uses.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Không tìm thấy dữ liệu với {id}");
            return use;
        }
        private static UseDto ConvertUseDto(Use use)
        {
            return new UseDto()
            {
                Id = use.Id,
                Name = use.Name,
                Info = use.Info
            };
        }
        public async Task<ApiObject?> RemoveUseByIdAsync(int id)
        {
            var use = await UseExist(id);
            _context.Uses.Remove(use);
            await _context.SaveChangesAsync();
            return new ApiObject {
                Message = "Xóa thành công"
            };
        }

        public async Task<UseDto> UpdateUseByIdAsync(int id, UseUpdate useUpdate)
        {
            var use = await UseExist(id);
            var useItem = await _context.Uses
                .Where(x => x != use)
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == useUpdate.Name.ToUpper());
            if (useItem != null)
            {
                throw new AlreadyExistsException($"{useItem.Name} đã tồn tại.");
            }
            use.Name = useUpdate.Name;
            use.Info = useUpdate.Info;
            await _context.SaveChangesAsync();
            return ConvertUseDto(use);
        }
    }
}