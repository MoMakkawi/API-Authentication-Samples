using Bearer_Authentication.Authentication.Models;
using Bearer_Authentication.Authentication.Services;

using Microsoft.AspNetCore.Mvc;

namespace Bearer_Authentication.Endpoints;
internal static class UserEndpoints
{
    private const string prefix = "/users";
    internal static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost(prefix + "/login", async ([FromBody] LoginRequest loginRequest, [FromServices] JwtAuthenticationService jwtAuthenticationService) =>
        {
            try
            {
                return await jwtAuthenticationService.LoginAsync(loginRequest);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
        }).WithTags("Users");
    }
}
