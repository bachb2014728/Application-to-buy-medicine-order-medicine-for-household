namespace backend.Dto.Receiver;

public class ReceiverDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsChecked { get; set; } 
    public int? CustomerId { get; set; }
}