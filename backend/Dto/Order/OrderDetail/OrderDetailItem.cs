namespace backend.Dto.Order.OrderDetail;

public class OrderDetailItem
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public int? TotalQuantity { get; set; }
    public decimal? TotalPrice { get; set; }
}