using backend.Dto.Voucher;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/vouchers")]
public class VoucherController(IVoucher service) : ControllerBase
{
    private readonly IVoucher _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetOne([FromRoute] int id)
    {
        var response = await _service.GetOne(id);
        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] VoucherCreate voucherCreate)
    {
        var response = await _service.Create(voucherCreate);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] VoucherUpdate voucherUpdate, [FromRoute] int id)
    {
        var response = await _service.Update(voucherUpdate, id);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteOne([FromRoute] int id)
    {
        var response = await _service.DeleteOne(id);
        return Ok(response);
    }

    [HttpPut("add-to-voucher")]
    [Authorize]
    public async Task<IActionResult> AddToVoucher([FromBody] AddToVoucher addToVoucher)
    {
        var response = await _service.AddToVoucher(addToVoucher);
        return Ok(response);
    }

    [HttpGet("my-list-voucher")]
    [Authorize]
    public async Task<IActionResult> MyListVoucher()
    {
        var response = await _service.MyListVoucher();
        return Ok(response);
    }

    [HttpGet("get-all-voucher-of-store/{storeId:int}")]
    [Authorize]
    public async Task<IActionResult> MyVoucherOfStore([FromRoute] int storeId)
    {
        var response = await _service.MyVoucherOfStore(storeId);
        return Ok(response);
    }
}