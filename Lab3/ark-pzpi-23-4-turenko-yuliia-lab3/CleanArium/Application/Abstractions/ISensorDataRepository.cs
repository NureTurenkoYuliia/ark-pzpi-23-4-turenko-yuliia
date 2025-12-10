using Domain.Models;

namespace Application.Abstractions;

public interface ISensorDataRepository
{
    Task AddAsync(SensorData data);
    Task<SensorData?> GetLatestByDeviceAsync(long deviceId);
    Task<List<SensorData>> GetByDeviceAsync(long deviceId, DateTime from, DateTime to);
    Task<List<SensorData>> GetRecentAsync(long deviceId, TimeSpan interval);
    Task<double?> GetAverageValueAsync(long deviceId, DateTime from, DateTime to);
    Task<double?> GetTrendSlopeAsync(long deviceId, DateTime from, DateTime to);
}
