namespace Bearer_Authentication.Options;

public class JwtOption
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int AccessTokenLifetime { get; set; }
    public int RefreshTokenLifetime { get; set; }
    public required string SigningKey { get; set; }
}
