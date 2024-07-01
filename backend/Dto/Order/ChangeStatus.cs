using backend.Data;

namespace backend.Dto.Order;

public class ChangeStatus
{
    public int OrderId { get; set; }
    public StatusData OrderStatus { get; set; }
}