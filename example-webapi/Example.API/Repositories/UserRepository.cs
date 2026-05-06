using Example.API.Common.Utilities;
using Example.API.Data;
using Example.API.Entities;
using Example.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Example.API.Repositories;

public class UserRepository : IUserRepository
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UserRepository));

    private readonly AppDbContext db;

    public UserRepository(AppDbContext _db)
    {
        LoggerUtil.GetLoggerThreadId();
        this.db = _db;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        try
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string username)
    {
        try
        {
            return await db.Users.AnyAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    public async Task AddAsync(User user)
    {
        try
        {
            await db.Users.AddAsync(user);
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
