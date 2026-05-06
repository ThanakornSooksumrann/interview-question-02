using Example.API.Common.Models.Appsettings;

namespace Example.API.Common.Services.Interfaces;
public interface IAppSettingsService
{
    public string GetProfileConfig();
    public string GetSystemNameConfig();
    public JwtConfig GetJwtConfig();
    public List<DatabaseConfig> GetDatabasesConfigs();
    public string GetDatabaseConnection();
}
