
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("Manufacturers")]
    public class Manufacturer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Info { get; set; }
        public List<Product>? Products { get; set; } = [];
    }
}