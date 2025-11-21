using Domain.Models;

namespace Application.Abstractions;

public interface IScheduledCommandRepository
{
    Task<ScheduledCommand?> GetByIdAsync(long id);
    Task<IEnumerable<ScheduledCommand>> GetAllByDeviceIdAsync(long deviceId);
    Task<ScheduledCommand> AddAsync(ScheduledCommand command);
    Task UpdateAsync(ScheduledCommand command);
    Task DeleteAsync(ScheduledCommand command);
    Task<bool> ScheduledCommandBelongsToUserAsync(long scheduledCommandId, long userId);
    Task<bool> ExistsAsync(long userId, long commandId, CancellationToken ct);
}
