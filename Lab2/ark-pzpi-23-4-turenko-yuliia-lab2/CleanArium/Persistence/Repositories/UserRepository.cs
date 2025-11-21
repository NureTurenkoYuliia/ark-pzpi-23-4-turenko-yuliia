using Application.Abstractions;
using Domain.Models;
using Persistence.Application;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CleanAriumDbContext _db;

    public UserRepository(CleanAriumDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        _db.Users.FirstOrDefaultAsync(x => x.Email == email, ct);

    public Task<User?> GetByIdAsync(long id, CancellationToken ct) =>
        _db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(User user, CancellationToken ct)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByIdAsync(long userId, CancellationToken ct)
        => await _db.Users.AnyAsync(x => x.Id == userId, ct);
}
