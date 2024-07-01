using backend.Dto.Comment;
using backend.Model;

namespace backend.Interface;

public interface IComment
{
    Task<List<Comment>?> GetAll(int productId);
    Task<object?> AddComment(CommentCreate commentCreate);
    Task<object?> ReplyComment(CommentReply commentReply);
}