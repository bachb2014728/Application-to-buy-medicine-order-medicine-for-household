using backend.Dto.Cart;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/carts")]
[Authorize]
public class CartController(ICart service) : ControllerBase
{
    private readonly ICart _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllByCustomer();
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartCreate cart)
    {
        var response = await _service.AddToCart(cart);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOne([FromRoute] int id)
    {
        await _service.DeleteOne(id);
        return NoContent();
    }
}