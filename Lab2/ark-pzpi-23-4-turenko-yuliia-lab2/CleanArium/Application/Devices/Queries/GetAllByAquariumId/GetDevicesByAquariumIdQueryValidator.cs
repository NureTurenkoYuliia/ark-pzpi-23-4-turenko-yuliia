using Application.Abstractions;
using FluentValidation;

namespace Application.Devices.Queries.GetAllByAquariumId;

public class GetDevicesByAquariumIdQueryValidator : AbstractValidator<GetDevicesByAquariumIdQuery>
{
    private readonly IAquariumRepository _repo;
    private readonly IUserRepository _userRepo;

    public GetDevicesByAquariumIdQueryValidator(IAquariumRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;

        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0)
            .WithMessage("User id is required to get devices of aquarium.");

        RuleFor(x => x.AquariumId).NotEmpty().GreaterThan(0)
            .WithMessage("Aquarium id is required to get its devices.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(AquariumExists).WithMessage("Aquarium not found.");
    }

    private async Task<bool> UserExists(GetDevicesByAquariumIdQuery cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> AquariumExists(GetDevicesByAquariumIdQuery cmd, CancellationToken ct)
    {
        return await _repo.ExistsByIdAsync(cmd.UserId, cmd.AquariumId, ct);
    }
}
