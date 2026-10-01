using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Repository.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HealthFlowDbContext _context;

    public UserRepository(HealthFlowDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(
            user,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
