using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model;
[Table("TypeModel")]
public class TypeModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsEnable { get; set; } = true;
}