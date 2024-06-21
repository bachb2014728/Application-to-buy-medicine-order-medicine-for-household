namespace backend.Dto.Order;

public class OrderItem
{
    public int Id { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? ReceiverInfo { get; set; }
    public List<int>? OrderDetailIds { get; set; } = [];
    public DateTime CreatedOn { get; set; }
    public required string OrderStatus { get; set; }
    public int? CustomerId { get; set; }
}