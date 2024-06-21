using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("Image")]
    public class Image
    {
        public int Id { get; set; }
        public required string Name { get; set; } 
        public required string Type { get; set;} 
        public required byte[] File { get; set; }
    }
}