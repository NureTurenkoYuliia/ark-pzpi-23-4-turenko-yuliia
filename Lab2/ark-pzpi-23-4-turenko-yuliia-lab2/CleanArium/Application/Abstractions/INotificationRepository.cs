using Domain.Models;

namespace Application.Abstractions;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<List<Notification>> GetUserNotificationsAsync(long userId);
    Task MarkAsReadAsync(long notificationId);
    Task<bool> ExistsAsync(long userId, long notificationId, CancellationToken ct);
}
