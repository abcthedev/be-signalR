using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using API.Hubs;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly NotificationService _notificationService;

    public NotificationsController(IHubContext<NotificationHub> hubContext, NotificationService notificationService)
    {
        _hubContext = hubContext;
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Notification notification)
    {
        await _notificationService.AddNotificationAsync(notification);
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"User {notification.UserId}: {notification.Message}");
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<Notification>>> GetNotifications()
    {
        var notifications = await _notificationService.GetAllNotificationsAsync();
        return Ok(notifications);
    }
}

public class Notification
{
    public string Id { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
    public string UserId { get; set; }
    public string Message { get; set; }
}
