using backend.Data;
using backend.Dto;
using backend.Dto.Receiver;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class ReceiverService (ApplicationDbContext context,ConvertInformation convert) : IReceiver
{
    private readonly ApplicationDbContext _context = context;
    private readonly ConvertInformation _convert = convert;
    public async Task<IEnumerable<ReceiverItem>?> GetAll()
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var items = await _context.Receivers.Where(x => x.CustomerId == customer.Id).ToListAsync();
        if (items.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu.");
        }

        return items.Select(ToReceiverItem);
    }

    private static ReceiverItem ToReceiverItem(Receiver receiver)
    {
        return new ReceiverItem
        {
            Id = receiver.Id,
            Name = receiver.Name,
            Phone = receiver.Phone,
            Address = receiver.Address,
            IsChecked = receiver.IsChecked
        };
    }

    public async Task<ReceiverDto?> Create(ReceiverCreate receiverCreate)
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        if (customer == null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu người dùng.");
        }
        var newItem = new Receiver
        {
            Name = receiverCreate.Name,
            Phone = receiverCreate.Phone,
            Address = receiverCreate.Address,
            Customer = customer
        };
        await _context.Receivers.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return ToReceiverDto(newItem);
    }

    private static ReceiverDto ToReceiverDto(Receiver receiver)
    {
        return new ReceiverDto
        {
            Id = receiver.Id,
            Name = receiver.Name,
            Phone = receiver.Phone,
            Address = receiver.Address,
            IsChecked = receiver.IsChecked,
            CustomerId = receiver.CustomerId
        };
    }

    public async Task<ApiObject> Checked(IsCheckedOfReceiver isCheckedOfReceiver)
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var items = await _context.Receivers.Where(item => item.CustomerId == customer.Id).ToListAsync();
        foreach (var item in items)
        {
            item.IsChecked = false;
            await _context.SaveChangesAsync();
        }
        var receiver = await _context.Receivers.FirstOrDefaultAsync(x => x.Id == isCheckedOfReceiver.ReceiverId);
        if (receiver == null)
        {
            throw new NotFoundException("Không tìm thấy địa chỉ người nhận.");
        }
        receiver.IsChecked = true;
        await _context.SaveChangesAsync();
        return new ApiObject()
        {
            Message = "Thay đổi trạng thái thành công."
        };
    }

    public async Task<ReceiverDto?> Update(ReceiverUpdate receiverUpdate, int id)
    {
        var receiver = await _context.Receivers.FirstOrDefaultAsync(x => x.Id == id);
        if (receiver == null)
        {
            throw new NotFoundException("Không tìm thấy địa chỉ người nhận.");
        }
        receiver.Name = receiverUpdate.Name ?? receiver.Name;
        receiver.Phone = receiverUpdate.Phone ?? receiver.Phone;
        receiver.Address = receiverUpdate.Address ?? receiver.Address;
        await _context.SaveChangesAsync();
        return ToReceiverDto(receiver);
    }

    public async Task<ApiObject?> Delete(int id)
    {
        var receiver = await _context.Receivers.FirstOrDefaultAsync(x => x.Id == id);
        if (receiver == null)
        {
            throw new NotFoundException($"Không tìm thấy thông tin người nhận với id = {id}.");
        }

        _context.Receivers.Remove(receiver);
        await _context.SaveChangesAsync();
        return new ApiObject()
        {
            Message = "Xóa dữ liệu thành công."
        };
    }
}