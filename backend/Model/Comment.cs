
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now; 
        public Customer? Customer{ get; set; }
    }
}