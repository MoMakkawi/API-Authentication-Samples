using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using Persistence.Contracts;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Basic_Authentication.Authentication;

[Obsolete]
internal class BasicAuthenticationHandler (
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock,
    IUserRepository UsersRepository) 
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.NoResult();

        if (!AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var authHeader))
            return AuthenticateResult.Fail("Unknown scheme.");

        var encodedCredentials = authHeader.Parameter;
        var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials!));
        var userNameAndPassword = decodedCredentials.Split(':');

        var user = await UsersRepository.FindByUserNameAsync(userNameAndPassword[0]);

        if (user is null || user.Password != userNameAndPassword[1])
            return AuthenticateResult.Fail("Invalid username or password.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);

    }
}
