using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Notifications")]
public class Notification
{
    public int Id { get; set; }
    public int? CustomerId { get; set; } 
    public int? StoreId { get; set; }
    public required string Message { get; set; } 
    public bool IsRead { get; set; } // Trạng thái đã đọc hay chưa
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}
