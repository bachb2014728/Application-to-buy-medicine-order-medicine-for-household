
using System.ComponentModel.DataAnnotations.Schema;
using backend.Dto.Store;

namespace backend.Model
{
    [Table("Stores")]
    public class Store
    {
        public int Id { get; set; }
        public required string? Name { get; set; } 
        public required string? URL { get; set; }
        public Image? Avatar{ get; set; } 
        public Image? Background{ get; set; } 
        public string? Info { get; set;}
        public List<Customer?> Followers { get; set; } = [];
        public decimal Star { get; set; } = 0;
        public required string? Address { get; set; }
        public bool Mode { get; set; }
        public string? Status { get; set; } //Await , Active , Block
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public AppUser? CreatedBy{ get; set; }
    }
}