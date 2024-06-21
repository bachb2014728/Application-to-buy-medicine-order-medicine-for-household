using backend.Dto.Order;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/orders")]
[Authorize]
public class OrderController(IOrder service) : ControllerBase
{
    private readonly IOrder _service = service;
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();
        return Ok(response);
    }

    [HttpGet("my-list-order")]
    public async Task<IActionResult> MyListOrder()
    {
        var response = await _service.MyListOrder();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne([FromRoute] int id)
    {
        var order = await _service.GetOne(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreate orderCreate)
    {
        var order = await _service.Create(orderCreate);
        return Ok(order);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OrderUpdate orderUpdate)
    {
        var order = await _service.Update(orderUpdate,id);
        return Ok(order);
    }
}