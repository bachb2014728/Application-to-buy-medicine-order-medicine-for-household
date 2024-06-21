
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("DosageForms")]
    public class DosageForm
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Info { get; set; }
        public List<Product>? Products { get; set; } = [];
    }
}