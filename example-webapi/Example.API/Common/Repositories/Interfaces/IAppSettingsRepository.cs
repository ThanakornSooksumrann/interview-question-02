using Example.API.Common.Models.Appsettings;

namespace Example.API.Common.Repositories.Interfaces;
public interface IAppSettingsRepository
{
    public string GetProfileConfig();
    public string GetSystemNameConfig();
    public JwtConfig GetJwtConfig();
    public List<DatabaseConfig> GetDatabasesConfigs();
    public DatabaseConnection GetDatabaseConnection();
}
