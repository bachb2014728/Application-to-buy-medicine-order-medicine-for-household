using backend.Data;
using backend.Dto;
using backend.Dto.Product;
using backend.Dto.Product.Contraindication;
using backend.Dto.Store;
using backend.Error;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class ContraindicationService(ApplicationDbContext context) : IContraindication
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ContraindicationDto> GetOne(int id)
    {
        var item = await ContraindicationExist(id);
        return ConvertContraindicationDto(item);
    }

    private async Task<Contraindication> ContraindicationExist(int id)
    {
        var item = await _context.Contraindications.FirstOrDefaultAsync(x=>x.Id == id);
        if (item == null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu.");
        }
        return item;
    }

    private static ContraindicationDto ConvertContraindicationDto(Contraindication contraindication)
    {
        return new ContraindicationDto()
        {
            Id = contraindication.Id,
            Name = contraindication.Name,
            Info = contraindication.Info,
        };
    }

    public async Task<ContraindicationDto> Create(ContraindicationCreate contraindicationCreate)
    {
        var item = await _context.Contraindications.FirstOrDefaultAsync(x => x.Name == contraindicationCreate.Name);
        if (item != null)
        {
            throw new AlreadyExistsException($"{contraindicationCreate.Name} đã tồn tại.");
        }

        var newItem = new Contraindication()
        {
            Name = contraindicationCreate.Name,
            Info = contraindicationCreate.Info
        };
        await _context.Contraindications.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return ConvertContraindicationDto(newItem);
    }

    public async Task<List<Contraindication>> GetAll()
    {
        var listItem = await _context.Contraindications.OrderBy(u => u.Id).ToListAsync();
        if (listItem.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu.");
        }
        return listItem;
    }

    public async Task<ApiObject> DeleteOne(int id)
    {
        var item = await ContraindicationExist(id);
        _context.Contraindications.Remove(item);
         await _context.SaveChangesAsync();
        return new ApiObject()
        {
            Message = "Xóa dữ liệu thành công."
        };
    }

    public async Task<ContraindicationDto> Update(int id, ContraindicationUpdate contraindication)
    {
        var item = await ContraindicationExist(id);
        var contraindicationItem = await _context.Contraindications.Where(x => x != item)
            .FirstOrDefaultAsync(x => x.Name == contraindication.Name);
        if (contraindicationItem != null)
        {
            throw new AlreadyExistsException($"{contraindication.Name} đã tồn tại.");
        }
        item.Name = contraindication.Name;
        item.Info = contraindication.Info;
        await _context.SaveChangesAsync();
        return ConvertContraindicationDto(item);
    }
}