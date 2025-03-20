using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NotificationService
{
    private readonly IMongoCollection<Notification> _notifications;

public NotificationService(IConfiguration configuration)
    {
var connectionString = configuration["MongoDB:ConnectionString"];
    var databaseName = configuration["MongoDB:DatabaseName"];
    var collectionName = configuration["MongoDB:CollectionName"];

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("MongoDB ConnectionString is NULL or EMPTY. Check your appsettings.json!");
    }

    Console.WriteLine($"Using MongoDB Connection: {connectionString}");

    var client = new MongoClient(connectionString);
    var database = client.GetDatabase(databaseName);
    _notifications = database.GetCollection<Notification>(collectionName);
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
