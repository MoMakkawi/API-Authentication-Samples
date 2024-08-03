using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Bearer_Authentication.Authentication.Models;
using Bearer_Authentication.Options;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Persistence.Contracts;
using Persistence.Models;

namespace Bearer_Authentication.Authentication.Services;

public class JwtAuthenticationService(JwtOptions jwtOptions, IUserRepository userRepository)
{
    private readonly JwtOptions jwtOptions = jwtOptions;

    public async Task<AuthResult> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userRepository.FindByUserNameAsync(loginRequest.UserName) 
            ?? throw new ArgumentException($"There no user by this username : {loginRequest.UserName}");

        if (!string.Equals(user.Password, loginRequest.Password))
            throw new ArgumentException("Password or username is wrong.");

        return new AuthResult(GenerateAccessToken(user), GenerateRefreshToken());
    }

    #region Tokens Generators
    private TokenDetails GenerateAccessToken(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtOptions.AccessTokenLifetime);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            Expires = expiresAt,

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),

            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                ])
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return new TokenDetails(token, expiresAt);
    }
    private TokenDetails GenerateRefreshToken()
    {
        var expiresAt = DateTime.UtcNow.AddMonths(jwtOptions.RefreshTokenLifetime);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            Expires = expiresAt,

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return new TokenDetails(token, expiresAt);
    }
    #endregion
}
