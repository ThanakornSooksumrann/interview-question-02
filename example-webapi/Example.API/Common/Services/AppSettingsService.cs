using Example.API.Common.Models.Appsettings;
using Example.API.Common.Repositories.Interfaces;
using Example.API.Common.Services.Interfaces;
using Example.API.Common.Utilities;

namespace Example.API.Common.Services;
public class AppSettingsService : IAppSettingsService
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppSettingsService));

    private readonly IAppSettingsRepository appSettingsRepository;
    public AppSettingsService(IAppSettingsRepository appSettingsRepository)
    {
        LoggerUtil.GetLoggerThreadId();
        this.appSettingsRepository = appSettingsRepository;
    }

    public JwtConfig GetJwtConfig()
    {
        try
        {
            JwtConfig jwtConfig = appSettingsRepository.GetJwtConfig();
            return jwtConfig;
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
            return appSettingsRepository.GetProfileConfig();
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
            return appSettingsRepository.GetSystemNameConfig();
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
            return appSettingsRepository.GetDatabasesConfigs();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string GetDatabaseConnection()
    {
        try
        {
            DatabaseConnection databaseConnection = appSettingsRepository.GetDatabaseConnection();
            return databaseConnection.KeycloakConnectDatabase ?? string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
