using Application.Abstractions;
using FluentValidation;

namespace Application.Aquariums.Commands.Update;

public class UpdateAquariumCommandValidator : AbstractValidator<UpdateAquariumCommand>
{
    private readonly IAquariumRepository _repo;
    private readonly IUserRepository _userRepo;

    public UpdateAquariumCommandValidator(IAquariumRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;

        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0)
            .WithMessage("User id is required to update aquarium.");

        RuleFor(x => x.AquariumId).NotEmpty().GreaterThan(0)
            .WithMessage("Aquarium id is required to update aquarium.");

        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Name is required to update aquarium.")
            .MaximumLength(30).WithMessage("Title max length must not exceed 30 symbols.");

        RuleFor(x => x.Location)
            .MaximumLength(80).WithMessage("Location max length must not exceed 80 symbols.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(AquariumExists).WithMessage("Aquarium not found.");
    }

    private async Task<bool> UserExists(UpdateAquariumCommand cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> AquariumExists(UpdateAquariumCommand cmd, CancellationToken ct)
    {
        return await _repo.ExistsByIdAsync(cmd.UserId, cmd.AquariumId, ct);
    }
}
