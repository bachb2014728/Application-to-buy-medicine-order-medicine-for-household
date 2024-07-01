using backend.Data;
using backend.Dto;
using backend.Dto.Comment;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class CommentService(ApplicationDbContext context, ConvertInformation convert) : IComment
{
    private readonly ApplicationDbContext _context = context;
    private readonly ConvertInformation _convert = convert;
    public async Task<List<Comment>?> GetAll(int productId)
    {
        var product = await _context.Products.Include(product => product.Comments)
            .FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null)
        {
            throw new NotFoundException($"không tìm thấy sản phẩm với id = {productId}");
        }

        var comments = await _context.Comments
            .Include(x=>x.Customer)
            .Where(x => product.Comments.Select(item => item.Id).Contains(x.Id))
            .ToListAsync();
        return comments;
    }

    public async Task<object?> AddComment(CommentCreate commentCreate)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == commentCreate.ProductId);
        if (product == null)
        {
            throw new NotFoundException("không tìm thấy sản phẩm");
        }

        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        if (commentCreate.StoreId == null)
        {
            var comment = new Comment
            {
                Message = commentCreate.Message,
                CreatedOn = DateTime.UtcNow.AddHours(7),
                IsEnable = true,
                Customer = customer
            };
            await _context.Comments.AddAsync(comment);
            product.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        else
        {
            var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == commentCreate.StoreId);
            if (store == null)
            {
                throw new NotFoundException("không tìm thấy sản phẩm");
            }
            var comment = new Comment
            {
                Message = commentCreate.Message,
                CreatedOn = DateTime.UtcNow.AddHours(7),
                IsEnable = true,
                Store = store
            };
            await _context.Comments.AddAsync(comment);
            product.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        
    }

    public async Task<object?> ReplyComment(CommentReply commentReply)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == commentReply.ProductId);
        if (product == null)
        {
            throw new NotFoundException("không tìm thấy sản phẩm");
        }
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var commentParent = await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentReply.CommentParentId);
        if (commentParent != null)
        {
            if (commentReply.StoreId == null)
            {
                var newItem = new Comment
                {
                    Message = commentReply.Message,
                    CreatedOn = DateTime.UtcNow.AddHours(7),
                    IsEnable = true,
                    CommentParentId = commentParent.Id,
                    Customer = customer
                };
                await _context.Comments.AddAsync(newItem);
                product.Comments.Add(newItem);
                await _context.SaveChangesAsync();
                return newItem;
            }
            else
            {
                var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == commentReply.StoreId);
                if (store == null)
                {
                    throw new NotFoundException("không tìm thấy sản phẩm");
                }
                var newItem = new Comment
                {
                    Message = commentReply.Message,
                    CreatedOn = DateTime.UtcNow.AddHours(7),
                    IsEnable = true,
                    CommentParentId = commentParent.Id,
                    Store = store
                };
                await _context.Comments.AddAsync(newItem);
                product.Comments.Add(newItem);
                await _context.SaveChangesAsync();
                return newItem;
            }
        }
        else
        {
            throw new NotFoundException($"Không tìm thấy bình luận trước đó với {commentReply.CommentParentId}");
        }
    }
}