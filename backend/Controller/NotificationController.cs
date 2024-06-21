using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/notifications")]
[Authorize]
public class NotificationController(INotification service) : ControllerBase
{
    private readonly INotification _notification = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var listNotifications = await _notification.GetAll();
        return Ok(listNotifications);
    }
    
}