using Domain.Models;

namespace Application.Abstractions;

public interface IAquariumRepository
{
    Task<Aquarium?> GetByIdAsync(long id);
    Task<List<Aquarium>> GetAllByUserIdAsync(long userId);
    Task AddAsync(Aquarium aquarium);
    Task UpdateAsync(Aquarium aquarium);
    Task DeleteAsync(Aquarium aquarium);
    Task<bool> ExistsByIdAsync(long userId, long aquariumId, CancellationToken ct);
    Task<bool> ExistsByNameAsync(long userId, string name, CancellationToken ct);
}
