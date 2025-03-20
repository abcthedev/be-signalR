using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NotificationService
{
    private readonly IMongoCollection<Notification> _notifications;

    public NotificationService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _notifications = database.GetCollection<Notification>(settings.Value.CollectionName);
    }

    public async Task<List<Notification>> GetAllNotificationsAsync()
    {
        return await _notifications.Find(_ => true).ToListAsync();
    }

    public async Task<Notification> AddNotificationAsync(Notification notification)
    {
        await _notifications.InsertOneAsync(notification);
        return notification;
    }
}


public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}
