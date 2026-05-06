using Example.API.Entities;

namespace Example.API.Repositories.Interfaces;

public interface IUserTokenRepository
{
    Task AddAsync(UserToken token);
    Task<UserToken?> GetByRefreshTokenAsync(string refreshToken);
    Task RevokeAllByUserIdAsync(int userId);
    Task SaveChangesAsync();
}
