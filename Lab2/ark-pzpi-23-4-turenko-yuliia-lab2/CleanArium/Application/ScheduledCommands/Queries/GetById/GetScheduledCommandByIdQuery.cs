using Application.DTOs.ScheduledCommands;
using MediatR;

namespace Application.ScheduledCommands.Queries.GetById;

public record GetScheduledCommandByIdQuery(long UserId, long CommandId) : IRequest<ScheduledCommandDto>;