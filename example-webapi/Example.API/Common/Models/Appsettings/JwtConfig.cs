namespace Example.API.Common.Models.Appsettings
{
    public class JwtConfig
    {
        public string? Expire { get; set; }
        public string? ExpireRemember { get; set; }
        public string? AccessTokenSecret { get; set; }
    }
}
