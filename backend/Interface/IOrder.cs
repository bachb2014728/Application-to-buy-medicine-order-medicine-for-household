using backend.Dto.Order;

namespace backend.Interface;

public interface IOrder
{
    Task<IEnumerable<OrderItem>> GetAll();
    Task<IEnumerable<OrderItem>> MyListOrder();
    Task<OrderDto?> GetOne(int id);
    Task<OrderDto?> Create(OrderCreate orderCreate);
    Task<OrderDto?> Update(OrderUpdate orderUpdate,int id);
}