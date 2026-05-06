using Example.API.Common.Utilities;
using Example.API.Data;
using Example.API.Entities;
using Example.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Example.API.Repositories;

public class UserTokenRepository : IUserTokenRepository
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UserTokenRepository));

    private readonly AppDbContext db;

    public UserTokenRepository(AppDbContext _db)
    {
        LoggerUtil.GetLoggerThreadId();
        this.db = _db;
    }

    public async Task AddAsync(UserToken token)
    {
        try
        {
            await db.UserTokens.AddAsync(token);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    public async Task<UserToken?> GetByRefreshTokenAsync(string refreshToken)
    {
        try
        {
            return await db.UserTokens
                .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken
                                       && !t.IsRevoked
                                       && t.ExpiresAt > DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    public async Task RevokeAllByUserIdAsync(int userId)
    {
        try
        {
            List<UserToken> tokens = await db.UserTokens
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToListAsync();

            foreach (UserToken token in tokens)
            {
                token.IsRevoked = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }
}
