using Domain.Models;

namespace Application.Abstractions;

public interface ISensorDataService
{
    Task SaveAsync(long userId, SensorData data);
}
