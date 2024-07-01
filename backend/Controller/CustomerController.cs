using backend.Dto;
using backend.Dto.Customer;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/customers")]
[Authorize]
public class CustomerController(ICustomer service) : ControllerBase
{
    private readonly ICustomer _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne([FromRoute] int id)
    {
        var response = await _service.GetOne(id);
        return Ok(response);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var response = await _service.GetProfile();
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromBody] CustomerUpdate customerUpdate,[FromRoute] int id)
    {
        var response = await _service.Update(customerUpdate,id);
        return Ok(response);
    } 
}