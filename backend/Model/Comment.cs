
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now.AddHours(7);
        public bool IsEnable { get; set; } = true;
        public int? CommentParentId { get; set; }
        public Customer? Customer{ get; set; }
        public Store? Store { get; set; }
    }
}