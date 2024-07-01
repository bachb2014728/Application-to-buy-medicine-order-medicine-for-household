using backend.Dto.Receiver;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("/api/v1/receivers")]
[Authorize]
public class ReceiverController(IReceiver receiver) : ControllerBase
{
    private readonly IReceiver _receiver = receiver;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _receiver.GetAll();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ReceiverCreate receiverCreate)
    {
        var response = await _receiver.Create(receiverCreate);
        return Ok(response);
    }

    [HttpPut("checked")]
    public async Task<IActionResult> Checked([FromBody] IsCheckedOfReceiver isCheckedOfReceiver)
    {
        var response = await _receiver.Checked(isCheckedOfReceiver);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromBody] ReceiverUpdate receiverUpdate, [FromRoute] int id)
    {
        var response = await _receiver.Update(receiverUpdate, id);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _receiver.Delete(id);
        return Ok(response);
    }
    
}