using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("Receivers")]
public class Receiver
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public bool IsChecked { get; set; } = false;
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
}