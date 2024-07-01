using backend.Dto.Comment;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/comments")]
public class CommentController(IComment service) : ControllerBase
{
    private readonly IComment _sevice = service;

    [HttpGet("{productId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllByProduct([FromRoute] int productId)
    {
        var response = await _sevice.GetAll(productId);
        return Ok(response);
    }

    [HttpPost("add-comment")]
    [Authorize]
    public async Task<IActionResult> AddComment([FromBody] CommentCreate commentCreate)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await _sevice.AddComment(commentCreate);
        return Ok(response);
    }

    [HttpPost("reply-comment")]
    [Authorize]
    public async Task<IActionResult> AddReply([FromBody] CommentReply commentReply)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await _sevice.ReplyComment(commentReply);
        return Ok(response);

    }
}