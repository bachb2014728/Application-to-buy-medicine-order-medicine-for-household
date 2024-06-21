namespace backend.Dto.Product.Comment;

public class CommentDto
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now; 
    public int? Customer{ get; set; }
}