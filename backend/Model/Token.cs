using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    [Table("Tokens")]
    public class Token
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}