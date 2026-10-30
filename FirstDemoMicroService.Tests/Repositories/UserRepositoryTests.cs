using FirstDemoMicroService.Domain.Entities;
using FirstDemoMicroService.Infrastructure.Persistence;
using FirstDemoMicroService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FirstDemoMicroService.Tests.Repositories;

public sealed class UserRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsUsersWithoutTracking()
    {
        await using var dbContext = CreateDbContext();
        dbContext.Users.AddRange(CreateUser("Alex Morgan"), CreateUser("Sam Taylor"));
        await dbContext.SaveChangesAsync();

        var repository = new UserRepository(dbContext);
        var users = await repository.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, users.Count);
        Assert.Equal(EntityState.Detached, dbContext.Entry(users[0]).State);
        Assert.Equal(EntityState.Detached, dbContext.Entry(users[1]).State);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMatchingUserWithoutTracking()
    {
        await using var dbContext = CreateDbContext();
        var user = CreateUser("Alex Morgan");
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var repository = new UserRepository(dbContext);
        var result = await repository.GetByIdAsync(user.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(EntityState.Detached, dbContext.Entry(result).State);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenUserDoesNotExist()
    {
        await using var dbContext = CreateDbContext();
        var repository = new UserRepository(dbContext);

        var result = await repository.GetByIdAsync(42, CancellationToken.None);

        Assert.Null(result);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static User CreateUser(string fullName) =>
        new()
        {
            FullName = fullName,
            Email = $"{Guid.NewGuid():N}@example.com",
            CreatedAtUtc = DateTime.UtcNow
        };
}
