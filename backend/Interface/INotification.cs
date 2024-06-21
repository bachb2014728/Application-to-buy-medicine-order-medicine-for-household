using backend.Model;

namespace backend.Interface;

public interface INotification
{
    Task<List<Notification>?> GetAll();
}