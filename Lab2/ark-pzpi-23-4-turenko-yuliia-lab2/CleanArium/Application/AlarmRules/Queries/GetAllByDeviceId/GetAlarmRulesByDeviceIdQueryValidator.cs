using Application.Abstractions;
using FluentValidation;

namespace Application.AlarmRules.Queries.GetAllByDeviceId;

public class GetAlarmRulesByDeviceIdQueryValidator : AbstractValidator<GetAlarmRulesByDeviceIdQuery>
{
    private readonly IDeviceRepository _deviceRepo;
    private readonly IUserRepository _userRepo;

    public GetAlarmRulesByDeviceIdQueryValidator(IDeviceRepository deviceRepo, IUserRepository userRepo)
    {
        _deviceRepo = deviceRepo;
        _userRepo = userRepo;

        RuleFor(x => x.DeviceId).NotEmpty().GreaterThan(0)
            .WithMessage("Device id is required to alarm rule.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(DeviceExists).WithMessage("Device not found.")
            .MustAsync(UserOwnsDevice).WithMessage("User doesn't own this device.");
    }

    private async Task<bool> UserExists(GetAlarmRulesByDeviceIdQuery cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> DeviceExists(GetAlarmRulesByDeviceIdQuery cmd, CancellationToken ct)
    {
        return await _deviceRepo.ExistsByIdAsync(cmd.DeviceId, ct);
    }

    private async Task<bool> UserOwnsDevice(GetAlarmRulesByDeviceIdQuery cmd, CancellationToken ct)
    {
        return await _deviceRepo.UserOwnsDeviceAsync(cmd.UserId, cmd.DeviceId, ct);
    }
}