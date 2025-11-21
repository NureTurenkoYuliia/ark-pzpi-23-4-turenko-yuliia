using Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.ScheduledCommands.Commands.Update;

public class UpdateScheduledCommandHandler : IRequestHandler<UpdateScheduledCommand>
{
    private readonly IScheduledCommandRepository _repo;
    private readonly ILogger<UpdateScheduledCommandHandler> _logger;

    public UpdateScheduledCommandHandler(IScheduledCommandRepository repo, ILogger<UpdateScheduledCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task Handle(UpdateScheduledCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.CommandId);

        entity.CommandType = request.CommandType;
        entity.StartTime = request.StartTime;
        entity.RepeatMode = request.RepeatMode;
        entity.IntervalMinutes = request.IntervalMinutes;
        entity.IsActive = request.IsActive;

        await _repo.UpdateAsync(entity);

        _logger.LogInformation("ScheduledCommand updated: {Id}", request.CommandId);
    }
}