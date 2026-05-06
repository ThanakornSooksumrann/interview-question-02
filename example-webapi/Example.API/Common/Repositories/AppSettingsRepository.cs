using Microsoft.Extensions.Configuration;
using System.Text;
using Example.API.Common.Repositories.Interfaces;
using Example.API.Common.Models.Appsettings;
using Example.API.Common.Utilities;

namespace Example.API.Common.Repositories;
public class AppSettingsRepository : IAppSettingsRepository
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppSettingsRepository));

    private readonly IConfiguration configuration;

    public AppSettingsRepository(IConfiguration configuration)
    {
        LoggerUtil.GetLoggerThreadId();
        this.configuration = configuration;
    }

    public JwtConfig GetJwtConfig()
    {
        try
        {
            return configuration.GetSection("JWT").Get<JwtConfig>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string GetProfileConfig()
    {
        try
        {
            return configuration.GetSection("Profile").Get<string>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string GetSystemNameConfig()
    {
        try
        {
            return configuration.GetSection("SystemName").Get<string>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public List<DatabaseConfig> GetDatabasesConfigs()
    {
        try
        {
            return configuration.GetSection("Databases").Get<List<DatabaseConfig>>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DatabaseConnection GetDatabaseConnection()
    {
        try
        {
            return configuration.GetSection("ConnectionStrings").Get<DatabaseConnection>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
