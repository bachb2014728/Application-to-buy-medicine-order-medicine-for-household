namespace backend.Dto.Comment;

public class CommentReply
{
    public required string Message { get; set; }
    public required int ProductId { get; set; }
    public required int CommentParentId { get; set; }
    public int? StoreId { get; set; }
}