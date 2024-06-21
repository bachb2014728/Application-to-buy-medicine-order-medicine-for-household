using backend.Dto.Voucher;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/vouchers")]
[Authorize]
public class VoucherController(IVoucher service) : ControllerBase
{
    private readonly IVoucher _service = service;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoucherCreate voucherCreate)
    {
        var response = await _service.Create(voucherCreate);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromBody] VoucherUpdate voucherUpdate, [FromRoute] int id)
    {
        var response = await _service.Update(voucherUpdate, id);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOne([FromRoute] int id)
    {
        var response = await _service.DeleteOne(id);
        return Ok(response);
    }

    [HttpGet("add-to-voucher/{id:int}")]
    public async Task<IActionResult> AddToVoucher([FromRoute] int id)
    {
        var response = await _service.AddToVoucher(id);
        return Ok(response);
    }

    [HttpGet("my-list-voucher")]
    public async Task<IActionResult> MyListVoucher()
    {
        var response = await _service.MyListVoucher();
        return Ok(response);
    }
}