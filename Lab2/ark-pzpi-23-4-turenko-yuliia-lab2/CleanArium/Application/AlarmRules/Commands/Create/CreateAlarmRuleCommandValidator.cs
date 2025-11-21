using Application.Abstractions;
using FluentValidation;

namespace Application.AlarmRules.Commands.Create;

public class CreateAlarmRuleCommandValidator : AbstractValidator<CreateAlarmRuleCommand>
{
    private readonly IAlarmRuleRepository _alarmRuleRepo;
    private readonly IDeviceRepository _deviceRepo;
    private readonly IUserRepository _userRepo;

    public CreateAlarmRuleCommandValidator(IAlarmRuleRepository alarmRuleRepo, IDeviceRepository deviceRepo, IUserRepository userRepo)
    {
        _alarmRuleRepo = alarmRuleRepo;
        _deviceRepo = deviceRepo;
        _userRepo = userRepo;

        RuleFor(x => x.DeviceId).NotEmpty().GreaterThan(0)
            .WithMessage("Device id is required to create alarm rule.");

        RuleFor(x => x.Parameter).NotEmpty()
            .WithMessage("Parameter for rule is required.");

        RuleFor(x => x.Condition).NotEmpty()
           .WithMessage("Condition for rule is required.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(DeviceExists).WithMessage("Device not found.")
            .MustAsync(UserOwnsDevice).WithMessage("User doesn't own this device.")
            .MustAsync(ExceedsNumberOfRules).WithMessage("Maximum 2 rules per parameter for this device.");
    }

    private async Task<bool> UserExists(CreateAlarmRuleCommand cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> DeviceExists(CreateAlarmRuleCommand cmd, CancellationToken ct)
    {
        return await _deviceRepo.ExistsByIdAsync(cmd.DeviceId, ct);
    }

    private async Task<bool> UserOwnsDevice(CreateAlarmRuleCommand cmd, CancellationToken ct)
    {
        return await _deviceRepo.UserOwnsDeviceAsync(cmd.UserId, cmd.DeviceId, ct);
    }

    private async Task<bool> ExceedsNumberOfRules(CreateAlarmRuleCommand cmd, CancellationToken ct)
    {
        var existing = await _alarmRuleRepo.CountByDeviceAndParameter(cmd.DeviceId, cmd.Parameter, ct);
        return existing >= 2;
    }
}