using Application.Abstractions;
using FluentValidation;

namespace Application.Devices.Commands.Create;

public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    private readonly IAquariumRepository _repo;
    private readonly IUserRepository _userRepo;

    public CreateDeviceCommandValidator(IAquariumRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;

        RuleFor(x => x.AquariumId).NotEmpty().GreaterThan(0)
            .WithMessage("Aquarium id is required to create device.");

        RuleFor(x => x.DeviceType).NotEmpty()
            .WithMessage("Choose Device Type to create new device.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(AquariumExists).WithMessage("Aquarium not found.");
    }

    private async Task<bool> UserExists(CreateDeviceCommand cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> AquariumExists(CreateDeviceCommand cmd, CancellationToken ct)
    {
        return await _repo.ExistsByIdAsync(cmd.UserId, cmd.AquariumId, ct);
    }
}