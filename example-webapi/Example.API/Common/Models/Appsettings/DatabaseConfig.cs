namespace Example.API.Common.Models.Appsettings;
public class DatabaseConfig
{
    public string? DBType { get; set; }
    public string? DBConnectionName { get; set; }
    public string? SchemaName { get; set; }
}

public class DatabaseConnection
{
    public string? KeycloakConnectDatabase { get; set; }
}