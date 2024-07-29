namespace Bearer_Authentication.Authentication.Models;

public sealed record AuthResult(TokenDetails AccessToken, TokenDetails RefreshToken);