using Application.Abstractions;
using FluentValidation;

namespace Application.AlarmRules.Commands.Update;

public class UpdateAlarmRuleCommandValidator : AbstractValidator<UpdateAlarmRuleCommand>
{
    private readonly IAlarmRuleRepository _alarmRuleRepo;
    private readonly IUserRepository _userRepo;

    public UpdateAlarmRuleCommandValidator(IAlarmRuleRepository alarmRuleRepo, IUserRepository userRepo)
    {
        _alarmRuleRepo = alarmRuleRepo;
        _userRepo = userRepo;

        RuleFor(x => x.RuleId).NotEmpty().GreaterThan(0)
            .WithMessage("Rule id is required to update alarm rule.");

        RuleFor(x => x.Parameter).NotEmpty()
            .WithMessage("Parameter for rule is required.");

        RuleFor(x => x.Condition).NotEmpty()
           .WithMessage("Condition for rule is required.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(RuleExists).WithMessage("Rule not found.")
            .MustAsync(ExceedsNumberOfRules).WithMessage("Maximum 2 rules per parameter for this device.");
    }

    private async Task<bool> UserExists(UpdateAlarmRuleCommand cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> RuleExists(UpdateAlarmRuleCommand cmd, CancellationToken ct)
    {
        return await _alarmRuleRepo.ExistsByUserIdAsync(cmd.UserId, cmd.RuleId, ct);
    }

    private async Task<bool> ExceedsNumberOfRules(UpdateAlarmRuleCommand cmd, CancellationToken ct)
    {
        var rule = await _alarmRuleRepo.GetByIdAsync(cmd.RuleId, ct);
        var existing = await _alarmRuleRepo.CountByDeviceAndParameter(rule.DeviceId, cmd.Parameter, ct);
        return existing >= 2;
    }
}