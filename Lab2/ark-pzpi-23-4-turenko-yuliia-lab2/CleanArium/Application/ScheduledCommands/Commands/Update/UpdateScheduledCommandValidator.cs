using Application.Abstractions;
using FluentValidation;

namespace Application.ScheduledCommands.Commands.Update;

public class UpdateScheduledCommandValidator : AbstractValidator<UpdateScheduledCommand>
{
    private readonly IScheduledCommandRepository _scheduledCommandRepo;
    private readonly IUserRepository _userRepo;

    public UpdateScheduledCommandValidator(IScheduledCommandRepository scheduledCommandRepo, IUserRepository userRepo)
    {
        _scheduledCommandRepo = scheduledCommandRepo;
        _userRepo = userRepo;

        RuleFor(x => x.CommandId).NotEmpty().GreaterThan(0)
            .WithMessage("Scheduled Command id is required to update scheduled command.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(CommandExists).WithMessage("Scheduled Command not found.");
    }

    private async Task<bool> UserExists(UpdateScheduledCommand cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> CommandExists(UpdateScheduledCommand cmd, CancellationToken ct)
    {
        return await _scheduledCommandRepo.ExistsAsync(cmd.UserId, cmd.CommandId, ct);
    }
}