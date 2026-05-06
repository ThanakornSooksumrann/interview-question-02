using Example.API.Entities;

namespace Example.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}
