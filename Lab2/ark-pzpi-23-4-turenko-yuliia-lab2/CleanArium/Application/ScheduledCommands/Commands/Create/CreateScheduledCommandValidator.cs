using Application.Abstractions;
using Domain.Enums;
using FluentValidation;

namespace Application.ScheduledCommands.Commands.Create;

public class CreateScheduledCommandValidator : AbstractValidator<CreateScheduledCommand>
{
    private readonly IDeviceRepository _deviceRepo;
    private readonly IUserRepository _userRepo;

    public CreateScheduledCommandValidator(IDeviceRepository deviceRepo, IUserRepository userRepo)
    {
        _deviceRepo = deviceRepo;
        _userRepo = userRepo;

        RuleFor(x => x.DeviceId).NotEmpty().GreaterThan(0)
            .WithMessage("Device id is required to create scheduled command.");

        RuleFor(x => x.CommandType).NotEmpty()
             .WithMessage("Command type is required to create scheduled command.");

        RuleFor(x => x.StartTime).NotEmpty()
             .WithMessage("Start time is required to create scheduled command.");

        RuleFor(x => x.IntervalMinutes).GreaterThan(0)
            .When(x => x.RepeatMode != RepeatMode.None);

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(DeviceExists).WithMessage("Device not found.")
            .MustAsync(UserOwnsDevice).WithMessage("User doesn't own this device.");
    }

    private async Task<bool> UserExists(CreateScheduledCommand cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> DeviceExists(CreateScheduledCommand cmd, CancellationToken ct)
    {
        return await _deviceRepo.ExistsByIdAsync(cmd.DeviceId, ct);
    }

    private async Task<bool> UserOwnsDevice(CreateScheduledCommand cmd, CancellationToken ct)
    {
        return await _deviceRepo.UserOwnsDeviceAsync(cmd.UserId, cmd.DeviceId, ct);
    }
}