using Application.Abstractions;
using Domain.Models;
using MediatR;

namespace Application.ExecutedCommands.Commands.Create;

public class CreateExecutedCommandHandler : IRequestHandler<CreateExecutedCommand>
{
    private readonly IExecutedCommandRepository _repo;

    public CreateExecutedCommandHandler(IExecutedCommandRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(CreateExecutedCommand command, CancellationToken ct)
    {
        var entity = new ExecutedCommand
        {
            DeviceId = command.DeviceId,
            CommandType = command.CommandType,
            CommandStatus = command.CommandStatus,
            IssuedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(entity);
    }
}
