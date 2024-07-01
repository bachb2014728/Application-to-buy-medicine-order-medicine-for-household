using backend.Dto.Order;

namespace backend.Interface;

public interface IOrder
{
    Task<IEnumerable<OrderItem>> GetAll();
    Task<IEnumerable<OrderItem>> MyListOrder();
    Task<OrderDto?> GetOne(int id);
    Task<OrderDto?> Create(OrderCreate orderCreate);
    Task<OrderDto?> Update(OrderUpdate orderUpdate,int id);
    Task<IEnumerable<OrderItem>?> GetAllOrderOfStore(int storeId);
    Task<object?> ChangeStatusOrder(ChangeStatus changeStatus, int orderId);
    Task<object?> OrderCancel(int orderId);
    Task<object?> DeleteOne(int id);
}