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

    [HttpPut("checked")]
    public async Task<IActionResult> Checked([FromBody] IsChecked isChecked)
    {
        var response = await _service.Checked(isChecked);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartCreate cart)
    {
        var response = await _service.AddToCart(cart);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromBody] CartUpdate cart, [FromRoute] int id)
    {
        var response = await _service.Update(cart, id);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOne([FromRoute] int id)
    {
        await _service.DeleteOne(id);
        return NoContent();
    }

    [HttpGet("cart-is-checked")]
    public async Task<IActionResult> GetCartIsChecked()
    {
        var response = await _service.CartIsChecked();
        return Ok(response);
    }
}