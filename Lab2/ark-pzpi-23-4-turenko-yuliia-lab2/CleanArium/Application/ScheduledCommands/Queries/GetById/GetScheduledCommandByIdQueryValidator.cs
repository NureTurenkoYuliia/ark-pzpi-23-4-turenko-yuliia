using Application.Abstractions;
using FluentValidation;

namespace Application.ScheduledCommands.Queries.GetById;

public class GetScheduledCommandByIdQueryValidator : AbstractValidator<GetScheduledCommandByIdQuery>
{
    private readonly IScheduledCommandRepository _scheduledCommandRepo;
    private readonly IUserRepository _userRepo;

    public GetScheduledCommandByIdQueryValidator(IScheduledCommandRepository scheduledCommandRepo, IUserRepository userRepo)
    {
        _scheduledCommandRepo = scheduledCommandRepo;
        _userRepo = userRepo;

        RuleFor(x => x.CommandId).NotEmpty().GreaterThan(0)
            .WithMessage("Scheduled Command id is required to retrieve.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(CommandExists).WithMessage("Scheduled Command not found.");
    }

    private async Task<bool> UserExists(GetScheduledCommandByIdQuery cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> CommandExists(GetScheduledCommandByIdQuery cmd, CancellationToken ct)
    {
        return await _scheduledCommandRepo.ExistsAsync(cmd.UserId, cmd.CommandId, ct);
    }
}