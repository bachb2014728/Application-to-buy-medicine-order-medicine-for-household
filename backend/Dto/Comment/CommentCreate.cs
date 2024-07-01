namespace backend.Dto.Comment;

public class CommentCreate
{
    public required string Message { get; set; }
    public required int ProductId { get; set; }
    public int? StoreId { get; set; }
}