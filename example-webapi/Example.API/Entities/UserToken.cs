namespace Example.API.Entities;

public class UserToken
{
    public int      Id           { get; set; }
    public int      UserId       { get; set; }
    public User     User         { get; set; } = null!;
    public string   RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt    { get; set; }
    public bool     IsRevoked    { get; set; }
    public DateTime CreatedAt    { get; set; }
}
