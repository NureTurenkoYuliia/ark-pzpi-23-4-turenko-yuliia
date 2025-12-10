using Domain.Models;

namespace Application.Abstractions;

public interface IAlarmRuleProcessor
{
    Task ProcessAsync(long userId, SensorData data);
}
