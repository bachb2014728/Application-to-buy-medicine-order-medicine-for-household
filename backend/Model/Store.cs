
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("Stores")]
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Info { get; set;} = String.Empty;
        public string? Status { get; set; } //Await , Active , Block
        public string? AppUserId { get; set; } 
        public AppUser? AppUser{ get; set; }

    }
}