using FirstDemoMicroService.Domain.Entities;

namespace FirstDemoMicroService.Domain.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
