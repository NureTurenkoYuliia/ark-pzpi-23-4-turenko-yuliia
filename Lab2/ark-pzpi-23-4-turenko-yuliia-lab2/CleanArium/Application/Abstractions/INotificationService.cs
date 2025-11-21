namespace Application.Abstractions;

public interface INotificationService
{
    Task CreateAsync(long userId, string title, string content);
}
