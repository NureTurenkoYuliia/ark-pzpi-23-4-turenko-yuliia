using Application.Abstractions;
using FluentValidation;

namespace Application.Aquariums.Commands.Create;

public class CreateAquariumCommandValidator : AbstractValidator<CreateAquariumCommand>
{
    private readonly IAquariumRepository _repo;
    private readonly IUserRepository _userRepo;

    public CreateAquariumCommandValidator(IAquariumRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;

        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0)
            .WithMessage("User id is required to create aquarium.");

        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Name is required to create aquarium.")
            .MaximumLength(30).WithMessage("Title max length must not exceed 30 symbols.");

        RuleFor(x => x.Location)
            .MaximumLength(80).WithMessage("Location max length must not exceed 80 symbols.");

        RuleFor(x => x)
            .MustAsync(UserExists).WithMessage("User not found.")
            .MustAsync(AquariumWithSameNameExists).WithMessage("Aquarium with such name already exists.");
    }

    private async Task<bool> UserExists(CreateAquariumCommand cmd, CancellationToken ct)
    {
        return await _userRepo.ExistsByIdAsync(cmd.UserId, ct);
    }

    private async Task<bool> AquariumWithSameNameExists(CreateAquariumCommand cmd, CancellationToken ct)
    {
        return !await _repo.ExistsByNameAsync(cmd.UserId, cmd.Name, ct);
    }
}
