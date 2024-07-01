
namespace backend.Dto.Order;

public class OrderCreate
{ 
    public decimal TotalPrice { get; set; }
    public required string ReceiverName { get; set; }
    public required string ReceiverPhone { get; set; }
    public required string ReceiverAddress { get; set; }
    public required string Payment { get; set; }
    public List<int> Carts { get; set; } = [];
    public List<int>? Vouchers { get; set; } = [];
    public int StoreId { get; set; }
}