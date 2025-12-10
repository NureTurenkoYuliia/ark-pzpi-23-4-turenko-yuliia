using Domain.Models;

namespace Application.Abstractions;

public interface IExecutedCommandRepository
{
    Task AddAsync(ExecutedCommand command);
    Task<List<ExecutedCommand>> GetByDeviceAsync(long deviceId, DateTime from, DateTime to);
    Task<List<ExecutedCommand>> GetByPeriodAsync(DateTime from, DateTime to);
    Task<int> CountByDeviceAsync(long deviceId, DateTime from, DateTime to);
}
