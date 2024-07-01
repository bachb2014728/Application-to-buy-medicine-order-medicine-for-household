using backend.Dto.Product.Contraindication;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/contraindications")]

public class ContraindicationController(IContraindication contraindication) : ControllerBase
{
    private readonly IContraindication _contraindication = contraindication;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var listItem = await _contraindication.GetAll();
        return Ok(listItem);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOne([FromRoute] int id)
    {
        var item = await _contraindication.GetOne(id);
        return Ok(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ContraindicationCreate item )
    {
        var response = await _contraindication.Create(item);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ContraindicationUpdate item)
    {
        var response = await _contraindication.Update(id, item);
        return Ok(response);
    }
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteOne([FromRoute] int id)
    {
        var response = await _contraindication.DeleteOne(id);
        return Ok(response);
    }
}