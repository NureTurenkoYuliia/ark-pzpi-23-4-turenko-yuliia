using Application.Abstractions;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Persistence.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(INotificationRepository repo, ILogger<NotificationService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task CreateAsync(long userId, string title, string content)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Content = content
        };

        await _repo.AddAsync(notification);

        _logger.LogInformation("Notification '{Title}' sent to User: {UserId}", title, userId);
    }
}