using backend.Data;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class NotificationService(ApplicationDbContext context, ConvertInformation convert) : INotification
{
    private readonly ApplicationDbContext _context = context;
    private readonly DateTime _now = DateTime.UtcNow.AddHours(7);
    private readonly ConvertInformation _convert = convert;
    public async Task<List<Notification>?> GetAll()
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var listItem = await _context.Notifications.OrderByDescending(n => n.CreatedOn)
            .Where(n => n.CustomerId == customer.Id).ToListAsync();
        return listItem;
    }
}