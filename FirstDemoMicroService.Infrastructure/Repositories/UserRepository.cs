using FirstDemoMicroService.Domain.Entities;
using FirstDemoMicroService.Domain.Repositories;
using FirstDemoMicroService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FirstDemoMicroService.Infrastructure.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Users
            .AsNoTracking()
            .OrderBy(user => user.Id)
            .ToListAsync(cancellationToken);

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
}
