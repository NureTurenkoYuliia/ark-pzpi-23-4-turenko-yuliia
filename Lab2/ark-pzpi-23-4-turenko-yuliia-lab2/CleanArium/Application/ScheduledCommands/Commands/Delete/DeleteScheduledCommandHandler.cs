using Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.ScheduledCommands.Commands.Delete;

public class DeleteScheduledCommandHandler : IRequestHandler<DeleteScheduledCommand>
{
    private readonly IScheduledCommandRepository _repo;
    private readonly ILogger<DeleteScheduledCommandHandler> _logger;

    public DeleteScheduledCommandHandler(IScheduledCommandRepository repo, ILogger<DeleteScheduledCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task Handle(DeleteScheduledCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.CommandId);

        await _repo.DeleteAsync(entity);

        _logger.LogInformation("Successfully deleted scheduled command: {Id} ", request.CommandId);
    }
}