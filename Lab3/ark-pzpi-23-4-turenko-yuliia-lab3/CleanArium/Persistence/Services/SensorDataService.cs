using Application.Abstractions;
using Domain.Models;
using Persistence.Application;

namespace Persistence.Services;

public class SensorDataService : ISensorDataService
{
    private readonly CleanAriumDbContext _db;
    private readonly IAlarmRuleProcessor _processor;

    public SensorDataService(CleanAriumDbContext db, IAlarmRuleProcessor processor)
    {
        _db = db;
        _processor = processor;
    }

    public async Task SaveAsync(long userId, SensorData data)
    {
        _db.SensorData.Add(data);
        await _db.SaveChangesAsync();

        await _processor.ProcessAsync(userId, data);
    }
}